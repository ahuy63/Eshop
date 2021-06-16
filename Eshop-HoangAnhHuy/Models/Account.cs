using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eshop_HoangAnhHuy.Models
{
    public class Account
    {
        public int Id { get; set; }

        [DisplayName("Tên tài khoản")]
        [Required(ErrorMessage = "Can not be blanked")]
        public string Username { get; set; }

        [DisplayName("Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Can not be blanked")]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public string Avatar { get; set; }
        public bool Status { get; set; }

        public List<Invoice> Invoices { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
