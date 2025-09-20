
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExamApp.Service.Interfaces;
namespace ExamApp.Web.Controllers;


[Authorize]
public class ExamController : Controller
{
    private readonly IExamService _examService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExamController(IExamService examService, IHttpContextAccessor httpContextAccessor)
    {
        _examService = examService;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> StartExam()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var examResult = await _examService.StartExamAsync(userId.Value);
        return RedirectToAction("Question", new { examResultId = examResult.Id, questionIndex = 0 });
    }

    [HttpGet]
    public async Task<IActionResult> Question(int examResultId, int questionIndex)
    {
        var questions = await _examService.GetAllQuestionsAsync();
        if (questionIndex >= questions.Count)
            return RedirectToAction("CompleteExam", new { examResultId });

        ViewBag.ExamResultId = examResultId;
        ViewBag.QuestionIndex = questionIndex;
        ViewBag.TotalQuestions = questions.Count;
        ViewBag.TimeLimit = questions[questionIndex].TimeLimitSeconds;

        return View(questions[questionIndex]);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAnswer(int examResultId, int questionId, string selectedAnswer, int timeSpent, int questionIndex)
    {
        var (isCorrect, correctAnswer) = await _examService.SubmitAnswerAsync(examResultId, questionId, selectedAnswer, timeSpent);

        return Json(new
        {
            isCorrect,
            correctAnswer,
            nextUrl = Url.Action("Question", new { examResultId, questionIndex = questionIndex + 1 })
        });
    }

    [HttpGet]
    public async Task<IActionResult> CompleteExam(int examResultId)
    {
        var examResult = await _examService.CompleteExamAsync(examResultId);
        return View(examResult);
    }

    [HttpGet]
    public async Task<IActionResult> Results()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var results = await _examService.GetUserExamResultsAsync(userId.Value);
        return View(results);
    }

    [HttpGet]
    public async Task<IActionResult> IncorrectAnswers()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var incorrectAnswers = await _examService.GetUserIncorrectAnswersAsync(userId.Value);
        return View(incorrectAnswers);
    }
}