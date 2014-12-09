using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEvents.Models
{
    public enum TypeOfPayment
    {
       
        Cash,
       AfterPeriod
    }
    public class Group
    {
        public int Id { get; set; }
        public virtual User Creator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime StartOfPaymentPeriod { get; set; }
        public DateTime EndOfPaymentPeriod { get; set; }
        public TypeOfPayment Payment { get; set;  }
        public virtual ICollection<User> Users { get; set; }

        public Group()
        {
            Users = new HashSet<User>();
        }



    }
}