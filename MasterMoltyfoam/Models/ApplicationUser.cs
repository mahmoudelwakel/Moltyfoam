using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MasterMoltyfoam.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public byte[] Picture { get; set; }
    }
}
