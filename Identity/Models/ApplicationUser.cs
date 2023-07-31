using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class ApplicationUser: IdentityUser
    {
     
        override public string? Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
