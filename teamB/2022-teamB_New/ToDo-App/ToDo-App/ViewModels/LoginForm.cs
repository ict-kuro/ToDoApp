
using System.ComponentModel.DataAnnotations;

namespace ToDo_App.ViewModels
{
    public class LoginForm
    {
        [Required(ErrorMessage ="ユーザーIDは必須項目です。")]
        [EmailAddress(ErrorMessage = "メールアドレスが正しくありません。")]
        [MaxLength(255, ErrorMessage = "文字制限をオーバーしています。")]
        [RegularExpression("^[a-zA-Z0-9!-/:-@¥[-`{-~]+$", ErrorMessage = "半角英数字で入力してください。")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "パスワードは必須項目です。")]
        [MaxLength(255, ErrorMessage = "文字制限をオーバーしています。")]
        [MinLength(4, ErrorMessage = "パスワードは4文字以上です。")]
        [RegularExpression("^[a-zA-Z0-9!-/:-@¥[-`{-~]+$", ErrorMessage = "半角英数字で入力してください。")]
        public string Password { get; set; }

    }

}
