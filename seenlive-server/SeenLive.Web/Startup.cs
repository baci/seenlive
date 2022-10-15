using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SeenLive.DataAccess;
using SeenLive.DataAccess.Settings.MongoSettings;
using SeenLive.Web.Handler;

namespace SeenLive.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public ILifetimeScope AutofacContainer { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            
            services.AddSwaggerGen(options =>
                {
                    const string versionString = "v1";

                    options.SwaggerDoc("seenlive-" + versionString,
                        new OpenApiInfo
                        {
                            Title = "SeenLive API", 
                            Version = versionString, 
                            Contact = new OpenApiContact{ Name="Till Riemer", Email="seenlive@tillriemer.de" }
                        }
                    );
                    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        In = ParameterLocation.Header,
                        Description = "Basic Auth header using bearer scheme."
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            Array.Empty<string>()
                        }
                    });
                }
            );
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder => builder.AllowAnyOrigin());
                options.AddPolicy("AllowAnyMethod", builder => builder.AllowAnyMethod());
                options.AddPolicy("AllowHeader", builder => builder.AllowAnyHeader());
            });

            // TODO add bearer authentication    

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            
            app.UseCors(option =>
            {
                option.AllowAnyOrigin();
                option.AllowAnyMethod();
                option.AllowAnyHeader();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // TODO switched off until we have a certificate
            //else
            //{
            //    app.UseHsts();
            //}
            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/seenlive-v1/swagger.json", "SeenLive API");
            });
            app.UseMvc();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(env?.ContentRootPath),
                RequestPath = new PathString("")
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var databaseSettings =
                Configuration.GetSection("SeenLiveDatabaseSettings").Get<SeenLiveDatabaseSettings>();
            
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule(new DataAccessModule(databaseSettings));
            builder.RegisterModule(new WebHandlerModule());
        }
    }
}
