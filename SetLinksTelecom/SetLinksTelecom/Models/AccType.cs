﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class AccType
    {
        public int AccTypeId { get; set; }
        [MaxLength(25)]
        //[Required]
        public string TypeName { get; set; }
    }
}