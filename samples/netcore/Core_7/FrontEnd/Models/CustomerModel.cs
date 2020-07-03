using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Models
{
    public class CustomerModel
    {
        [Required(ErrorMessage = "Customer name should be provided")]
        [StringLength(10)]
        [Display(Name = "Customer Name")]
        public string Name { get; set; }
    }
}