using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessCore.Utilities
{
    public class JsonAccount
    {
        public int AccountCode { get; set; }
        public string AccountName { get; set; }
        public string ParentAccountCode { get; set; }
        public int AccountClassId { get; set; }
        public bool IsContraAccount { get; set; }
        public bool IsCash { get; set; }
        public string Sign { get; set; }
    }
}
