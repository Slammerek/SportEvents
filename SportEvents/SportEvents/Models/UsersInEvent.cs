using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEvents.Models
{
    public enum State
    {
        Yes, No, NotDecided
    }
    public class UsersInEvent
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsersInEventId { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public State State { get; set; }
    }
}