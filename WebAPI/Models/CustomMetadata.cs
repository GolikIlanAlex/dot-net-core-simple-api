using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebAPI.Models
{
    public class CustomMetadata
    {
        [JsonProperty("query")]
        public string Query { get; set; }
    }
}
