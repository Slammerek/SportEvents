using SportEvents.Controllers.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEvents.Models
{
    public class User
    {
        public int Id { get; set; }

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
              
        [NotMapped]
        [DisplayName("Potvrzení hesla")]
        [Required(ErrorMessage = "Vyplňte prosím potvrzovací heslo.")]
        [DataType(DataType.Password)]
        [CompareAttribute("Password", ErrorMessage = "Hesla se neshodují.")]
        public string PasswordComparison { get; set; }

        [DisplayName("Jméno")]
        [Required(ErrorMessage = "Vyplňte prosím křestní jméno.")] 
        public string FirstName { get; set; }

        [DisplayName("Příjmení")]
        [Required(ErrorMessage = "Vyplňte prosím příjmení.")] 
        public string Surname { get; set; }

        [DisplayName("Telefon")]
        [RegularExpression ("^[+]?[()/0-9. -]{9,}$", ErrorMessage = "Neplatné telefonní číslo.")] 
        public string Telephone { get; set; }
        
        public DateTime RegistrationTime { get; set; }
    }
}