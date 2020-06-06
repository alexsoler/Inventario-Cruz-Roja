using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Dtos
{
    public class PaginationDto<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
    }
}
