using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Models
{
    public class City
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "Град")]
        public string name { get; set; }
    }
}
