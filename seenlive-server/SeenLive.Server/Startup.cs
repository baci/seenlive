using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace SeenLive.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("seenlive-v1",
                        new OpenApiInfo
                        {
                            Title = "SeenLive API", 
                            Version = "v1", 
                            Contact = new OpenApiContact{ Name="Till Riemer", Email="till.riemer@gmail.com" }
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
                options.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
                options.AddPolicy("AllowAnyMethod", options => options.AllowAnyMethod());
                options.AddPolicy("AllowHeader", options => options.AllowAnyHeader());
            });
            // TODO add bearer authentication
            // TODO configure DI for application services, e.g. services.AddScoped<IService, Service>(); 

            //services.AddDirectoryBrowser(); // TODO necessary?
            //services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // configure entry point for client - TODO necessary?
            //PhysicalFileProvider fileProvider = new PhysicalFileProvider(env.ContentRootPath);
            //DefaultFilesOptions defOptions = new DefaultFilesOptions();
            //defOptions.DefaultFileNames.Clear();
            //defOptions.FileProvider = fileProvider;
            //defOptions.DefaultFileNames.Add("index.html");
            //app.UseDefaultFiles(defOptions);

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
            app.UseHttpsRedirection();
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
            //app.UseStaticFiles(); // TODO necessary?
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(env.ContentRootPath), // TODO use file provider from above?
                RequestPath = new PathString("")
            });
        }
    }
}
