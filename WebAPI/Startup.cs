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
using WebAPI.GiphyServices;
using WebAPI.Interfaces;
using WebAPI.Managers;

namespace WebAPI
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
            var fetchingServiceaApiKey = "9Ev6Di0GqvPyf46zANsee44nfUsF7tld";
            var cashingMechanism = new BasicCashingMechanism();
            services.AddTransient<IFetchingManager, FetchingManager>();
            //services.AddTransient<ISupportedCreditProviders, SupportedCreditProviders>();
            services.AddSingleton<ICashingMechanism>(cashingMechanism);
            services.AddTransient<IFetchingService>(provider => new GiphyWrapper(fetchingServiceaApiKey));
            //services.AddHttpClient("HttpVisaClient").AddPolicyHandler(GetVisaRetryPolicy());
            //services.AddHttpClient("HttpMasterCardClient").AddPolicyHandler(GetMasterCardRetryPolicy());
            services.AddControllers();
        }

        //private IAsyncPolicy<HttpResponseMessage> GetMasterCardRetryPolicy()
        //{
        //    return HttpPolicyExtensions
        //        .HandleTransientHttpError()
        //        .OrResult(msg =>
        //        {
        //            var resp = msg.ToMasterCardResponse();
        //            var conditions = new List<string>
        //            {
        //                resp.MasterCardDeclineReason,
        //            };
        //            if (resp.ChargeResult == "Success") return false;
        //            var merchantIdentifier = msg.RequestMessage.Headers.GetValues("identifier").First();
        //            _storage.HandleDeclined(merchantIdentifier, resp).Wait();
        //            return !conditions.Any(s => string.Equals(s, "Insufficient funds"));
        //        })
        //        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        //}

        //private IAsyncPolicy<HttpResponseMessage> GetVisaRetryPolicy()
        //{
        //    return HttpPolicyExtensions
        //        .HandleTransientHttpError()
        //        .OrResult(msg =>
        //        {
        //            var resp = msg.ToVisaResponse();
        //            var conditions = new List<string>
        //            {
        //                resp.DeclineReason,
        //                resp.ResultReason
        //            };
        //            if (resp.ChargeResult == "Success") return false;
        //            var merchantIdentifier = msg.RequestMessage.Headers.GetValues("identifier").First();
        //            _storage.HandleDeclined(merchantIdentifier, resp).Wait();
        //            return !conditions.Any(s => string.Equals(s, "Insufficient funds"));
        //        })
        //        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        //}

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
