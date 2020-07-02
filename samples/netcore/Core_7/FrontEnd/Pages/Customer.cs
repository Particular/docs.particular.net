using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Pages
{
    public class Customer
    {
        [Required, StringLength(10)]
        public string Name { get; set; }
    }
}