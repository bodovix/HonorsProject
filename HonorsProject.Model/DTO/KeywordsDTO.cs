using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.DTO
{
    internal class KeywordsDTO
    {
        [JsonProperty("keywords")]
        public List<Keyword> Keywords { get; set; }
    }
}