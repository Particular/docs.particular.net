using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Models
{
    public class ViewModel
    {
        [Required(ErrorMessage = "Number should be provided")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer number")]
        [Display(Name = "Number")]
        public int Number { get; set; }
    }
}