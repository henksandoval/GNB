﻿using GNB.Api.App.Business;
using GNB.Api.App.Clients;
using GNB.Api.App.Models;
using GNB.Api.App.Services;
using GNB.Api.App.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GNB.Api
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
            IConfigurationSection uriHerokuApp = Configuration.GetSection("UriHerokuApp");

            services.AddSingleton<IRateService<RateModel>, RateService<RateModel>>();
            services.AddSingleton<ICurrencyConverter, CurrencyConverter>();
            services.AddSingleton<IDuplicateRatesCleaner, DuplicateRatesCleaner>();

            services.AddSingleton<ITransactionBusiness, TransactionBusiness>();
            services.AddSingleton<ITransactionService<TransactionModel>, TransactionService<TransactionModel>>();

            services.AddHttpClient<IHerokuAppClient, HerokuAppClient>(c => c.BaseAddress = new Uri(uriHerokuApp.Value));

            //services.AddSingleton<IHerokuAppClient, HerokuAppClientFromJsonFile>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
