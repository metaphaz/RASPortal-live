using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RASPortal.Models;
using RASPortal.Data; // Add this using statement for ApplicationDbContext
using Microsoft.EntityFrameworkCore; // Add this for ToListAsync and other EF Core methods
using System.Linq; // Add this for OrderByDescending and Take

namespace RASPortal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context; // Add ApplicationDbContext field

    // Modify constructor to inject ApplicationDbContext
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context; // Initialize ApplicationDbContext
    }

    // Modify Index action to fetch and pass events
// RASPortal/Controllers/HomeController.cs - No changes needed in the query
    public async Task<IActionResult> Index()
    {
        var latestEvents = await _context.Events
            .OrderByDescending(e => e.EventStartDate)
            .ThenByDescending(e => e.CreatedAt)
            .Take(3)
            .ToListAsync();
        return View(latestEvents);
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}