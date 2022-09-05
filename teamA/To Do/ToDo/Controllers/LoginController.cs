using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using ToDo.Models;
using ToDo.ViewModels;

namespace ToDo.Controllers
{
    public class LoginController : Controller
    {

        private readonly TeamAContext _db;

        public LoginController(TeamAContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginForm loginForm) // ログイン認証
        {
            if (!ModelState.IsValid)
            {
                return View("Index", loginForm);
            }


            var user = _db.UserTables.Where(x => x.UserId == loginForm.UserId).FirstOrDefault();

            if (user == null)
            {
                ViewData["message"] = "入力されたユーザーIDが見つかりませんでした";
                return View("Index");
            }

            if (loginForm.UserId.Equals(user.UserId) && loginForm.Password.Equals(user.Password))
            {
                HttpContext.Session.SetString("UserId",user.UserId);
                ViewData["message"] = "ログインできました";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["message"] = "Passwordが正しくありません";
                return View("Index");
            }

        }
        public IActionResult User()
        {
            return View();
        }
        public IActionResult Adduser_mode() // ユーザー新規登録画面に遷移
        {
            return RedirectToAction("User", "Login");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserTable User) //　ユーザー新規登録
        {

            if (System.String.IsNullOrEmpty(User.UserId) || System.String.IsNullOrEmpty(User.Password)) //未入力のエラー処理
            {
                ViewData["message"] = "全ての項目を入力してください";
                return View("User");
            }

            var UserCheck = _db.UserTables.Where(x => x.UserId == User.UserId).FirstOrDefault();
            if (UserCheck != null)
            {
                ViewData["message"] = "そのユーザーIDは既に使用されています";
                return View("User");
            }

            bool ID_mozi = Regex.IsMatch(User.UserId, "^[a-zA-Z0-9!-/:-@¥[-`{-~]+$");
            bool pass_mozi = Regex.IsMatch(User.Password, "^[a-zA-Z0-9!-/:-@¥[-`{-~]+$");

            if (ID_mozi == false || pass_mozi == false) //半角英数字かどうかの判定
            {
                ViewData["message"] = "半角英数字で入力してください";
                return View("User");
            }

            int ID_len = new StringInfo(User.UserId).LengthInTextElements;
            if (ID_len > 12) //IDが12文字以内かどうかの判定
            {
                ViewData["message"] = "IDは12文字以内で入力してください";
                return View("User");
            }

            int pass_len = new StringInfo(User.Password).LengthInTextElements;
            if (pass_len < 6 || pass_len > 18) //IDが６文字以上、１８文字以内かどうかの判定
            {
                ViewData["message"] = "パスワードは６文字以上、１８文字以内で入力してください";
                return View("User");
            }

            if (ModelState.IsValid) //ユーザー登録
            {
                _db.Add(User) ;
                await _db.SaveChangesAsync();
                return View("Index");
            }
            return View("User");
        }
    }
}
