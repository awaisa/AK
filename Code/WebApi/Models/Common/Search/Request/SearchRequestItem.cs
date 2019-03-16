using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.Models.Common.Search.Request
{
    public class SearchRequestItem
    {
        public string Value { get; set; }
        public string Regex { get; set; }
    }
}
