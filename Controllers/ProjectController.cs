using Microsoft.AspNetCore.Mvc;
using RASPortal.Models;
using RASPortal.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace RASPortal.Controllers;

public class ProjectController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProjectController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return View("~/Views/Shared/Forbidden.cshtml");
        }

        var projects = _context.Projects.ToList();
        var projectViewModels = projects.Select(p => new ProjectViewModel
        {
            Id = p.Id,
            ProjectName = p.ProjectName,
            ProjectDescription = p.ProjectDescription,
            ProjectAuthor = p.ProjectAuthor,
            ProjectDifficulty = p.ProjectDifficulty,
            ProjectTags = p.ProjectTags,
            ProjectInstructions = ParseInstructions(p.ProjectInstructions, p.Id),
            ExistingImages = Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "Projects", p.Id.ToString(), "Images"))
                ? Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "Projects", p.Id.ToString(), "Images"))
                : Array.Empty<string>(),
            ExistingFiles = Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "Projects", p.Id.ToString(), "Files"))
                ? Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "Projects", p.Id.ToString(), "Files"))
                : Array.Empty<string>()
        }).ToList();

        return View(projectViewModels);
    }

    public IActionResult GetProject(int id)
    {
        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return View("~/Views/Shared/Forbidden.cshtml");
        }

        var project = _context.Projects.FirstOrDefault(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        var projectViewModel = new ProjectViewModel
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            ProjectDescription = project.ProjectDescription,
            ProjectAuthor = project.ProjectAuthor,
            ProjectDifficulty = project.ProjectDifficulty,
            ProjectTags = project.ProjectTags,
            ProjectInstructions = ParseInstructions(project.ProjectInstructions, project.Id),
            ExistingImages = Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "Projects", project.Id.ToString(), "Images"))
                ? Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "Projects", project.Id.ToString(), "Images"))
                : Array.Empty<string>(),
            ExistingFiles = Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "Projects", project.Id.ToString(), "Files"))
                ? Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "Projects", project.Id.ToString(), "Files"))
                : Array.Empty<string>()
        };

        return View(projectViewModel);
    }

    private List<InstructionItem> ParseInstructions(string[] instructions, int projectId)
    {
        var instructionItems = new List<InstructionItem>();
        if (instructions == null)
            return instructionItems;

        foreach (var instruction in instructions)
        {
            var match = Regex.Match(instruction, @"\[image:(.*?)\]");
            if (match.Success)
            {
                var imagePath = match.Groups[1].Value;
                instructionItems.Add(new InstructionItem
                {
                    ImagePath = $"/Source/Projects/{projectId}/Instructions/{imagePath}"
                });
            }
            else
            {
                instructionItems.Add(new InstructionItem
                {
                    Text = instruction
                });
            }
        }
        return instructionItems;
    }
}