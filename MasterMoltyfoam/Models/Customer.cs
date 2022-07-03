using System.ComponentModel.DataAnnotations;

namespace MasterMoltyfoam.Models
{
    public class Customer
    {
        [Key]
        [Display(Name ="Customer No")]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Customer Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; }
        public string Address { get; set; }
        [Display(Name = "Profile Photo")]
        public byte[] Picture { get; set; }
    }
}
