using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_HoangAnhHuy.Models
{
    public class Product
    {
        public int Id { get; set; }

        [DisplayName("SKU: ")]
        [Required(ErrorMessage = "Can not be blanked")]
        public string SKU { get; set; }

        [DisplayName("Product name: ")]
        [Required(ErrorMessage = "Can not be blanked")]
        public string Name { get; set; }

        [DisplayName("Description: ")]
        public string Description { get; set; }

        [DisplayName("Price: ")]
        [Required(ErrorMessage = "Must be number")]
        public int Price { get; set; }

        [DisplayName("Stock: ")]
        [Required(ErrorMessage = "Must be number")]
        public int Stock { get; set; }

        [DisplayName("Category: ")]
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
