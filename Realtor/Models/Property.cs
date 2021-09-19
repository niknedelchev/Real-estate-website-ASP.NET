using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realtor.Models
{
    public class Property
    {   
        [Key]
        public int propertyId { get; set; }
      
        [Display(Name = "Град")]
        public int cityID { get; set; }
        [Display(Name = "Град")]
        [ForeignKey("cityID")]
        public virtual City city { get; set; }

        [Display(Name = "Тип имот")]
        public PropertyType propertyType { get; set; }
        
        [Display(Name = "Тип обява")]
        public Transaction transaction { get; set; }

        [Display(Name = "Заглавие")]
        public string header { get; set; }
        
        [Display(Name = "Описание")]
        public string description { get; set; }
        
        [Display(Name = "Телефон")]
        public string phoneNumber { get; set; }
        
        [Display(Name = "Цена")]
        public int price { get; set; }

        [Display(Name = "Изображение")]
        public IEnumerable<Image> image { get; set; }
        
        [Display(Name = "Потребител")]
        public string userID { get; set; }
    }
}
