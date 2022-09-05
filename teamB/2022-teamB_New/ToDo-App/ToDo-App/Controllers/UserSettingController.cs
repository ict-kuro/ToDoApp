using Microsoft.AspNetCore.Mvc;
using ToDo_App.Models;

namespace ToDo_App.Controllers
{
    public class UserSettingController : Controller
    {
        //DB
        private readonly TeamBContext _db;
        public UserSettingController(TeamBContext db) => _db = db;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserSetting(string select_distance)
        {


            return View();
        }
    }
}
