using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AGH.Models.DTO
{
    public class userLogin
    {
        [Required]
        public int User_ID { get; set; }
        
        [Required]
        public string User_Password { get; set; }
    }
}