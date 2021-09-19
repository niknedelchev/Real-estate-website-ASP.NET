using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Models
{
    public class CreateRoleModel
    {
        [Display(Name = "Роля")]
        [Required]
        public string RoleName { get; set; }
      

    }
}
