using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Models
{
    public class UserRolesModel
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public bool isSelected { get; set; }
    }
}
