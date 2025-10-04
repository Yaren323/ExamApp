using ExamApp.Core.Models;
using ExamApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExamApp.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public AdminController(IAdminService adminService, IUserService userService)
        {
            _adminService = adminService;
            _userService = userService;
        }

        // Admin giriş kontrolü
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }

        public IActionResult Login()
        {
            if (IsAdmin())
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userService.LoginAsync(new Service.DTOs.LoginDto
            {
                Username = username,
                Password = password
            });

            if (user != null && user.Role == UserRole.Admin)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", UserRole.Admin.ToString()); // Enum'ı string'e çevir
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Admin girişi başarısız";
            return View();
        }

        [AdminOnly]
        public async Task<IActionResult> Dashboard()
        {
            if (!IsAdmin()) return RedirectToAction("Login");

            var questions = await _adminService.GetAllQuestionsAsync();
            var users = await _adminService.GetAllUsersAsync();

            ViewBag.TotalQuestions = questions.Count();
            ViewBag.TotalUsers = users.Count();

            return View(questions);
        }

        [AdminOnly]
        public IActionResult AddQuestion()
        {
            if (!IsAdmin()) return RedirectToAction("Login");

            // Boş bir model oluştur
            var question = new Question
            {
                Points = 10 
            };

            return View(question);
        }

        [HttpPost]
        [AdminOnly]
        public async Task<IActionResult> AddQuestion(Question question)
        {
            if (!IsAdmin()) return RedirectToAction("Login");

            // Validation kontrolü
            if (!ModelState.IsValid)
            {
                return View(question);
            }

            // Doğru cevabın şıklardan biri olduğunu kontrol et
            var validOptions = new List<string> { question.OptionA, question.OptionB, question.OptionC, question.OptionD };
            if (!validOptions.Contains(question.CorrectAnswer))
            {
                ModelState.AddModelError("CorrectAnswer", "Doğru cevap şıklardan biri olmalıdır");
                return View(question);
            }

            await _adminService.AddQuestionAsync(question);
            return RedirectToAction("Dashboard");
        }

        [AdminOnly]
        public async Task<IActionResult> EditQuestion(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login");

            var question = await _adminService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                // Hata mesajı göster veya dashboard'a yönlendir
                TempData["Error"] = "Soru bulunamadı!";
                return RedirectToAction("Dashboard");
            }

            return View(question);
        }

        [HttpPost]
        [AdminOnly]
        public async Task<IActionResult> EditQuestion(Question question)
        {
            if (!IsAdmin()) return RedirectToAction("Login");

            // Validation kontrolü
            if (!ModelState.IsValid)
            {
                return View(question);
            }

            // Doğru cevabın şıklardan biri olduğunu kontrol et
            var validOptions = new List<string> { question.OptionA, question.OptionB, question.OptionC, question.OptionD };
            if (!validOptions.Contains(question.CorrectAnswer))
            {
                ModelState.AddModelError("CorrectAnswer", "Doğru cevap şıklardan biri olmalıdır");
                return View(question);
            }

            await _adminService.UpdateQuestionAsync(question);
            return RedirectToAction("Dashboard");
        }

        [AdminOnly]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login");

            await _adminService.DeleteQuestionAsync(id);
            return RedirectToAction("Dashboard");
        }

        [AdminOnly]
        public async Task<IActionResult> Users()
        {
            if (!IsAdmin()) return RedirectToAction("Login");

            var users = await _adminService.GetAllUsersAsync();
            return View(users);
        }

        [AdminOnly]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login");

            await _adminService.DeleteUserAsync(id);
            return RedirectToAction("Users");
        }

        [AdminOnly]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

    // Admin Only Attribute
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                context.Result = new RedirectToActionResult("Login", "Admin", null);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}