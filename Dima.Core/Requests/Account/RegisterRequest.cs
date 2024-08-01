using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Account
{
    public class RegisterRequest : BaseRequest
    {
        [Required(ErrorMessage = "E-Mail")]
        [EmailAddress(ErrorMessage = "E-Mail inválido")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Senha inválida")]
        public string Password { get; set; } = string.Empty;
    }
}
