using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.DTO
{
    internal class Keyword
    {
        
        [JsonProperty("keyword")]
        public string Key { get; set; }

        [JsonProperty("confidence_score")]
        public float Confidence { get; set; }
    }
}