using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RMK.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не заполнен пин код")]
        [DataType(DataType.Password)]
        public string PinCode { get; set; }
    }
}
