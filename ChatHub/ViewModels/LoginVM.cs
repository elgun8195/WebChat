using System.ComponentModel.DataAnnotations;

namespace ChatHub.ViewModels
{
    public class LoginVM
    {
        public string Username { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
