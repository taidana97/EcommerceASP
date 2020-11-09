﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Models
{
    [Table("SlideShow")]
    public class SlideShow
    {
              
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

    }
}
