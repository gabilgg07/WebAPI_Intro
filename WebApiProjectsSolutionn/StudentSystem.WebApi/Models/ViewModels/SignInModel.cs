using System.ComponentModel.DataAnnotations;

namespace StudentSystem.WebApi.Models.ViewModels
{
    public class SignInModel
    {
        [Required]
        [MinLength(3)]
        public string UserName { get; set; }

        [Required]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

