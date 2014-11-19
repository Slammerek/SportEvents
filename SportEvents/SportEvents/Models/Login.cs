using SportEvents.Controllers.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportEvents.Models
{
    public class Login
    {
        [DisplayName("E-mail(login)")]
        [Required(ErrorMessage = "Vyplňte prosím email sloužící jako login.")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$",
            ErrorMessage = "Neplatný formát e-mailu.")]
        public string Email { get; set; }

        [DisplayName("Heslo")]
        [Required(ErrorMessage = "Vyplňte prosím heslo.")]
        [RegularExpression("^[a-zA-Z0-9]{8,}$", ErrorMessage = "Povolené znaky jsou a-z, A-Z a 0-9 s minimální délkou 8 znaků.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}