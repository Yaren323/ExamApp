using ExamApp.Service.DTOs;
using ExamApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.Web.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        public async Task<IActionResult> Start()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var questions = await _examService.GetQuestionsAsync();

            // Toplam soru sayısını ve puanı view'a gönder
            ViewBag.TotalQuestions = questions.Count();
            ViewBag.TotalPoints = questions.Sum(q => q.Points);

            return View(questions);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Dictionary<int, string> answers)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var submission = new ExamSubmissionDto
            {
                UserId = userId.Value,
                Answers = answers
            };

            var result = await _examService.SubmitExamAsync(submission);
            return RedirectToAction("Result", new { id = result.Id });
        }

        public async Task<IActionResult> Result(int id)
        {
            var result = await _examService.GetExamResultAsync(id);
            if (result == null)
                return NotFound();

            return View(result);
        }
    }
}