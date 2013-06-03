namespace CASServer.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }
}