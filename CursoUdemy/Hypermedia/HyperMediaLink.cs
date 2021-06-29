using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursoUdemy.Hypermedia
{
    public class HyperMediaLink
    {
        public string Rel { get; set; }
        private string href;
        public string Href 
        {
            get
            {
                var _lock = new object();
                lock (_lock)
                {
                    var sb = new StringBuilder(href);
                    return sb.Replace("%2F", "/").ToString();
                }
            }
            set
            {
                href = value;
            } 
        }
        public string Type { get; set; }
        public string Action { get; set; }
    }
}
