using Microsoft.AspNetCore.Mvc;

namespace ToDo_App.Controllers
{
    public class test : Controller
    {
        public IActionResult Index(string id)
        {
            Console.WriteLine(id);
            return RedirectToAction("Index", "Display"); 
        }
    }
}
