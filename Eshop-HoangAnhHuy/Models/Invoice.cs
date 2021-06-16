using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_HoangAnhHuy.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [DisplayName("Account Id")]
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public DateTime IssuedDate { get; set; }
        [DisplayName("Shipping Address")]
        public string ShippingAddress { get; set; }
        [DisplayName("Shipping Phone")]
        public string ShippingPhone { get; set; }
        public int Total { get; set; }
        public bool Status { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
