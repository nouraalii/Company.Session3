using System.ComponentModel.DataAnnotations;

namespace Company.Session3.PL.Dtos
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email is Required!!")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
