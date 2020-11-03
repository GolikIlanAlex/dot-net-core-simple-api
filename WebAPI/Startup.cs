using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Extensions.Http;
using WebAPI.Controllers;
using WebAPI.CreditServices;
using WebAPI.Helpers;
using WebAPI.Interfaces;

namespace WebAPI
{
    public class Startup
    {
        private IDeclineStorage _storage;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _storage = new DeclineStorage();
            services.AddTransient<ICreditServiceProviderFactory, CreditServiceProviderFactory>();
            services.AddTransient<ISupportedCreditProviders, SupportedCreditProviders>();
            services.AddTransient<IParser, Parser>();
            services.AddSingleton<IDeclineStorage>(_storage);
            services.AddHttpClient("HttpVisaClient").AddPolicyHandler(GetVisaRetryPolicy());
            services.AddHttpClient("HttpMasterCardClient").AddPolicyHandler(GetMasterCardRetryPolicy());
            services.AddControllers();
        }

        private IAsyncPolicy<HttpResponseMessage> GetMasterCardRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg =>
                {
                    var resp = msg.ToMasterCardResponse();
                    var conditions = new List<string>
                    {
                        resp.MasterCardDeclineReason,
                    };
                    if (resp.ChargeResult == "Success") return false;
                    var merchantIdentifier = msg.RequestMessage.Headers.GetValues("identifier").First();
                    _storage.HandleDeclined(merchantIdentifier, resp).Wait();
                    return !conditions.Any(s => string.Equals(s, "Insufficient funds"));
                })
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private IAsyncPolicy<HttpResponseMessage> GetVisaRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg =>
                {
                    var resp = msg.ToVisaResponse();
                    var conditions = new List<string>
                    {
                        resp.DeclineReason,
                        resp.ResultReason
                    };
                    if (resp.ChargeResult == "Success") return false;
                    var merchantIdentifier = msg.RequestMessage.Headers.GetValues("identifier").First();
                    _storage.HandleDeclined(merchantIdentifier, resp).Wait();
                    return !conditions.Any(s => string.Equals(s, "Insufficient funds"));
                })
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
