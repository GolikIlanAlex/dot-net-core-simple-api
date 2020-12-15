using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GiphyDotNet.Manager;
using GiphyDotNet.Model.Parameters;
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
        private readonly IFetchingManager _fetchingManager;

        public ApiController(ILogger<ApiController> logger, IFetchingManager fetchingManager)
        {
            _logger = logger;
            _fetchingManager = fetchingManager;
        }

        [HttpDelete("deleteTrendingKey")]
        public IActionResult DeleteTrendingKey([FromBody] DeleteKeyQuery key)
        {
            try
            {
                _fetchingManager.DeleteFromTrendingStorageByKey(key);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.ToString());
                return BadRequest($"failed to delete trending key {key.Key}");
            }
        }

        [HttpDelete("deleteSearchKey")]
        public IActionResult DeleteSearchKey([FromBody] DeleteKeyQuery key)
        {
            try
            {
                _fetchingManager.DeleteFromSearchStorageByKey(key);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.ToString());
                return BadRequest($"failed to delete trending key {key.Key}");
            }
        }


        [HttpGet("fetchTrendingGifs")]
        public async Task<IActionResult> FetchTrendingGifs()
        {
            try
            {
                var urls = await _fetchingManager.GetTrendingsURlsOfTheDay();
                return Ok(JsonConvert.SerializeObject(urls));
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.ToString());
                return BadRequest("failed to fetch trending gifs");
            }
        }

        [HttpGet("fetchGifsByMetadata")]
        public async Task<IActionResult> FetchGifsByMetadata([FromBody]CustomMetadata metadata)
        {
            try
            {
                var urls = await _fetchingManager.GetByCustomMetadata(metadata);
                return Ok(JsonConvert.SerializeObject(urls));
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.ToString());
                return BadRequest("failed to fetch gifs by metadata");
            }
        }
    }
}
