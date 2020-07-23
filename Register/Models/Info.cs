using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Register.Models
{
    public class Info
    {
        [Required]
        [BindProperty]
        public int Id { get; set; }
        [StringLength(10, MinimumLength = 1)]
        public string userId { get; set; }

        
        public string userName { get; set; }
      
        [StringLength(8, MinimumLength = 1)]
        public string userPassword { get; set; }
        
        public bool login { get; set; }

        [Compare("userPassword")]
        public string ConfirmPassword { get; set; }
    }
}
