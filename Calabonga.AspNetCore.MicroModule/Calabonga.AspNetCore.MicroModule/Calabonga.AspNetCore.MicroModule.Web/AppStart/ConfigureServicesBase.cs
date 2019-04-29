﻿using System;
using System.Text;
using AutoMapper;
using Calabonga.AspNetCore.MicroModule.Data;
using Calabonga.AspNetCore.MicroModule.Web.Infrastructure.Attributes;
using Calabonga.AspNetCore.MicroModule.Web.Infrastructure.Settings;
using Calabonga.EntityFrameworkCore.UOW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Calabonga.AspNetCore.MicroModule.Web.AppStart
{
    /// <summary>
    /// ASP.NET Core services registration and configurations
    /// </summary>
    public static class ConfigureServicesBase
    {
        /// <summary>
        /// Configure Services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            // file upload dependency
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddDbContextPool<ApplicationDbContext>(config =>
            {
                config.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddUnitOfWork<ApplicationDbContext>();

            services.AddMemoryCache();

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = null;
                options.User.RequireUniqueEmail = true;
            });

            services.AddOptions();

            services.Configure<CurrentAppSettings>(configuration.GetSection(nameof(CurrentAppSettings)));

            services.AddHttpContextAccessor();
            services.AddResponseCaching();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                });
          
            services.AddAuthorization();
            services
                .AddMvc(options => { options.Filters.Add<ValidateModelStateAttribute>(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                });
        }
    }
}