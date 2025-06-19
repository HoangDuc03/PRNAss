using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        public int? MemberId { get; set; }

        public DateOnly? OrderDate { get; set; }

        public DateOnly? RequiredDate { get; set; }

        public DateOnly? ShippedDate { get; set; }

        public decimal? Freight { get; set; }
    }
}
