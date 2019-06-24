using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerSideJWT.DAL;
using ServerSideJWT.Data;
using ServerSideJWT.Infra;
using ServerSideJWT.Models;
using ServerSideJWT.Services;

namespace ServerSideJWT
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
            // Inject AppSettings
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IRepository, Repository>();
            services.AddDbContext<AuthContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            services.AddDefaultIdentity<AppUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthContext>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddSingleton<IEAEmailSender, EAEmailSender>();

            //services.AddDefaultIdentity<AppUser>(config =>
            //{
            //    config.SignIn.RequireConfirmedEmail = true;
            //})
            //.AddRoles<IdentityRole>()
            //.AddEntityFrameworkStores<AuthContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            });

            services.AddCors();

            // Set JWT Authentication

            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:Jwt_Secret"].ToString());

            services.AddAuthentication(x =>
          {
              x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
              x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
          }
            ).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
           builder.WithOrigins(Configuration["ApplicationSettings:Client_URL"].ToString())
           .AllowAnyHeader()
           .AllowAnyMethod());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}