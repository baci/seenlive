using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SeenLive.DataAccess;
using SeenLive.DataAccess.Settings.MongoSettings;
using SeenLive.Web.Filters;
using SeenLive.Web.Handler;

namespace SeenLive.Web
{
    public class Startup
    {
        private const string CorsPolicyName = "CorsPolicy";

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; } = null!;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json-patch+json"));
                options.Filters.Add(new ConsumesAttribute("application/json-patch+json"));
                
            });
            
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
                    
                    options.CustomOperationIds(e => $"{e.RelativePath}");
                    options.DescribeAllParametersInCamelCase();
                    options.SchemaFilter<RequireNonNullablePropertiesSchemaFilter>();
                    options.SupportNonNullableReferenceTypes();           
                    options.UseAllOfToExtendReferenceSchemas();
                    options.UseAllOfForInheritance();
                    options.UseOneOfForPolymorphism();
                    options.SelectSubTypesUsing(baseType =>
                        baseType.Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType)));
                }
            );
            services.AddCors(
                options => options.AddPolicy(
                    name: CorsPolicyName,
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    }));

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            
            app.UseRouting();
            app.UseCors(CorsPolicyName);
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            }); 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseAuthentication();
            
            //app.UseSession();
            
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "SeenLive API v1");
            });

            app.UseCookiePolicy();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var databaseSettings =
                Configuration.GetSection("SeenLiveDatabaseSettings").Get<SeenLiveDatabaseSettings>()!;
            
            builder.RegisterModule(new DataAccessModule(databaseSettings));
            builder.RegisterModule(new WebHandlerModule());
        }
    }
}
