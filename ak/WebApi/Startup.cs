using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Encodings.Web;
using AlbumViewerAspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Serilog;
using BusinessCore.Data;
using BusinessCore.Services.Security;
using BusinessCore.Services.Inventory;
using BusinessCore.Services.Financial;
using BusinessCore.Services.Purchasing;
using BusinessCore.Services.Sales;
using BusinessCore.Services.TaxSystem;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using WebApiCore.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AlbumViewerNetCore
{
    public class Startup
    {
        readonly IHostingEnvironment HostingEnvironment;

        IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            HostingEnvironment = env;
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(builder =>
            {
                string useSqLite = Configuration["Data:useSqLite"];
                if (useSqLite != "true")
                {
                    var connStr = Configuration["Data:SqlServerConnectionString"];
                    builder.UseSqlServer(connStr);
                }
                else
                {
                    // Note this path has to have full  access for the Web user in order 
                    // to create the DB and write to it.
                    var connStr = "Data Source=" +
                                  Path.Combine(HostingEnvironment.ContentRootPath, "AlbumViewerData.sqlite");
                    builder.UseSqlite(connStr);
                }
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //services.AddAuthenticationCore();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            // Make configuration available for EF configuration
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddTransient<IDbContext, ApplicationContext>();

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<IFinancialService, FinancialService>();
            services.AddTransient<IPurchasingService, PurchasingService>();
            services.AddTransient<ISalesService, SalesService>();
            services.AddTransient<ITaxService, TaxService>();

            //services.AddTransient<AlbumRepository>();
            //services.AddTransient<ArtistRepository>();

            //Log.Logger = new LoggerConfiguration()
            //        .WriteTo.RollingFile(pathFormat: "logs\\log-{Date}.log")
            //        .CreateLogger();
            //services.AddSingleton(Log.Logger);

            services.AddScoped<ApiExceptionFilter>();

            // Add framework services
            services.AddMvc(options =>
                {
                    // options.Filters.Add(new ApiExceptionFilter());
                })
            .AddJsonOptions(opt =>
            {
                var resolver = opt.SerializerSettings.ContractResolver;
                if (resolver != null)
                {
                    var res = resolver as DefaultContractResolver;
                    res.NamingStrategy = null;
                }
            });            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            ApplicationContext albumContext,
            IConfiguration configuration)
        {

            Log.Logger = new LoggerConfiguration()
                    .WriteTo.RollingFile(pathFormat: "logs\\log-{Date}.log")
                    .CreateLogger();

            if (env.IsDevelopment())
            {
                //loggerFactory.WithFilter(new FilterLoggerSettings
                //    {
                //        {"Trace",LogLevel.Trace },
                //        {"Default", LogLevel.Trace},
                //        {"Microsoft", LogLevel.Warning}, // very verbose
                //        {"System", LogLevel.Warning}
                //    })
                //    .AddConsole()
                //    .AddSerilog();

                app.UseDeveloperExceptionPage();             
            }
            else
            {
                //loggerFactory.WithFilter(new FilterLoggerSettings
                //    {
                //        {"Trace",LogLevel.Trace },
                //        {"Default", LogLevel.Trace},
                //        {"Microsoft", LogLevel.Warning}, // very verbose
                //        {"System", LogLevel.Warning}
                //    })
                //    .AddSerilog();

                app.UseExceptionHandler(errorApp =>

                    // Application level exception handler here - this is just a place holder
                        errorApp.Run(async (context) =>
                        {
                            context.Response.StatusCode = 500;
                            context.Response.ContentType = "text/html";
                            await context.Response.WriteAsync("<html><body>\r\n");
                            await
                                context.Response.WriteAsync(
                                    "We're sorry, we encountered an un-expected issue with your application.<br>\r\n");

                            // Capture the exception
                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                // This error would not normally be exposed to the client
                                await
                                    context.Response.WriteAsync("<br>Error: " +
                                                                HtmlEncoder.Default.Encode(error.Error.Message) +
                                                                "<br>\r\n");
                            }
                            await context.Response.WriteAsync("<br><a href=\"/\">Home</a><br>\r\n");
                            await context.Response.WriteAsync("</body></html>\r\n");
                            await context.Response.WriteAsync(new string(' ', 512)); // Padding for IE
                        }));


                //loggerFactory.AddConsole();
                //app.UseExceptionHandler("/");
                //app.UseExceptionHandler("/Home/Error");
            }

            // enable this to log ASP.NET messages into the log
            // not very useful except for troubleshooting
            //loggerFactory.WithFilter(new FilterLoggerSettings
            //        {
            //            {"Trace",LogLevel.Information },
            //            {"Default", LogLevel.Information},
            //            {"Microsoft", LogLevel.Warning}, // very verbose
            //            {"System", LogLevel.Warning}
            //        })
            //        .AddConsole()
            //        .AddSerilog();
            // loggerFactory.AddSerilog(new LoggerConfiguration()                
            //    .WriteTo.RollingFile(pathFormat: "asp-logs\\log-{Date}.log")
            //    .CreateLogger();

            //app.UseCors("CorsPolicy");

            // Enable Cookie Auth with automatic user policy
            app.UseAuthentication();

            app.UseDatabaseErrorPage();
            app.UseStatusCodePages();

            app.UseDefaultFiles(); // so index.html is not required
            app.UseStaticFiles();

            // put last so header configs like CORS or Cookies etc can fire
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });            



            //AlbumViewerDataImporter.EnsureAlbumData(albumContext,
            //    Path.Combine(env.ContentRootPath, "albums.js"));

        }
    }
}

