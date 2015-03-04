﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEvents.Models
{
    public class Article
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

    }
}