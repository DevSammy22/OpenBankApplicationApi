using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos
{
    public class AuthenticateModel
    {
        [Required]
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$")]
        public string AccountNumber { get; set; }
        [Required]
        //[RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must be 4-digit")]
        public string Pin { get; set; }
    }
}
