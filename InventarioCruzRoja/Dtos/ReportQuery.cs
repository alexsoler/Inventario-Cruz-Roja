using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Dtos
{
    public class ReportQuery
    {
        public string Format { get; set; }
        public bool Inline { get; set; }
        public string Parameter { get; set; }
    }
}
