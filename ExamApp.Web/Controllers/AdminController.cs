namespace ExamApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExamApp.Core.Interfaces;

[Authorize(Policy = "AdminOnly")]
public class AdminController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public AdminController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Users()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return View(users);
    }

    public async Task<IActionResult> Questions()
    {
        var questions = await _unitOfWork.Questions.GetAllAsync();
        return View(questions);
    }

    [HttpGet]
    public IActionResult AddQuestion()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddQuestion(Question question)
    {
        await _unitOfWork.Questions.AddAsync(question);
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction("Questions");
    }
}