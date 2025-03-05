using BUILD_WEEK_4_TEAM_7.Models;
using Microsoft.AspNetCore.Mvc;

namespace BUILD_WEEK_4_TEAM_7.Controllers {
    public class LoginController : Controller {
        public IActionResult LoginView() {
            return View();
        }

        [HttpPost]
        public IActionResult LoginView(LoginModel user) {
            if (ModelState.IsValid) {

                TempData["Username"] = user.Username;
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }
    }
}