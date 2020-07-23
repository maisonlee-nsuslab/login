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
        public string UserId { get; set; }

        [StringLength(10, MinimumLength = 1)]
        public string UserName { get; set; }
      
        [StringLength(8, MinimumLength = 1)]
        public string UserPassword { get; set; }
        
        public bool isLogin { get; set; }

        [Compare("UserPassword")]
        [StringLength(8, MinimumLength = 1)]
        public string ConfirmPassword { get; set; }
    }
}
