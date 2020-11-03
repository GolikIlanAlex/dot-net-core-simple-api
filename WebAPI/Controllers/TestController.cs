using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {

        private readonly ILogger<ApiController> _logger;
        private readonly ICreditServiceProviderFactory _creditServiceProviderFactory;
        private readonly IParser _parser;
        private readonly IDeclineStorage _storage;

        public ApiController(ILogger<ApiController> logger, ICreditServiceProviderFactory creditServiceProviderFactory, 
            IParser parser, IDeclineStorage storage)
        {
            _logger = logger;
            _creditServiceProviderFactory = creditServiceProviderFactory;
            _parser = parser;
            _storage = storage;
        }

        [HttpPost("charge")]
        public async Task<IActionResult> Charge([FromBody] ChargeDto item)
        {
            try
            {
                if (item == null)
                {
                    return BadRequest();
                }

                if (!Request.Headers.TryGetValue("identifier", out var value)) return BadRequest();
                var merchantIdentifier = value.First();
                var response = await _creditServiceProviderFactory.GetCreditService(item.CreditCardCompany).Charge(item, merchantIdentifier);
                return _parser.Parse(response);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("chargeStatuses")]
        public async Task<IActionResult> ChargeStatuses()
        {
            if (!Request.Headers.TryGetValue("identifier", out var value)) return new NotFoundResult();
            var response = await _storage.GetHandledDeclinedRequests(value);
            var contentResult = new ContentResult
            {
                Content = JsonConvert.SerializeObject(response.GroupBy(s => s)
                    .Select(grouping => new {reason = grouping.Key, count = grouping.Count()}))
            };
            return contentResult;
        }
    }
}
