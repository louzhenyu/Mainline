using System.Collections.Generic;
using Newtonsoft.Json;

namespace JinRi.Fx.WebUI.Models.DD
{
    public class Pagination<T>
    {
        public Pagination()
        {
            this.Rows = new List<T>();
        }
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
        [JsonProperty(PropertyName = "rows")]
        public IList<T> Rows { get; set; }
    }
}