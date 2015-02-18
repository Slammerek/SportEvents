using SportEvents.Controllers.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportEvents.Models
{
    public class ForgotPassword
    {
        [DisplayName("E-mail(login)")]
        [Required(ErrorMessage = "Vyplňte prosím email sloužící jako login.")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$",
            ErrorMessage = "Neplatný formát e-mailu.")]
        public string Email { get; set; }
    }
}