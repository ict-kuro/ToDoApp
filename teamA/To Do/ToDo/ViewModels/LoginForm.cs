using System.ComponentModel.DataAnnotations;

namespace ToDo.ViewModels
{
    public class LoginForm
    {
        [Required(ErrorMessage = "ユーザーIDは必須です")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "パスワードは必須です")]
        public string Password { get; set; } = null!;

    }
}
