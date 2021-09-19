using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Models
{
    public class Image
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Наименование")]
        public string name { get; set; }

        [Display(Name = "Изображение")]
        public string path { get; set; }

        public int propertyId { get; set; }
    }
}
