using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uwp_App
{
    public class Users
    {
        [Key]
        public int _id { get; set; }
        public int id { get; set; }
        public string login { get; set; }
        public string haslo { get; set; }
    }
}
