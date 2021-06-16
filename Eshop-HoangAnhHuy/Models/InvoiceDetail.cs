using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_HoangAnhHuy.Models
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        [Required]
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int UnitPrice { get; set; }
    }
}
