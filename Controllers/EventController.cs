using Microsoft.AspNetCore.Mvc;
using RASPortal.Models;
using RASPortal.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace RASPortal.Controllers;

public class EventController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public EventController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
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

        var events = _context.Events.ToList();
        var eventViewModels = events.Select(e => new EventViewModel
        {
            Id = e.Id,
            EventName = e.EventName,
            EventSociety = e.EventSociety,
            EventDescription = e.EventDescription,
            EventStartDate = e.EventStartDate,
            EventEndDate = e.EventEndDate,
            EventLocation = e.EventLocation,
            EventRegisterUrl = e.EventRegisterUrl,
            ExistingImages = Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "EventMedia", e.Id.ToString()))
                ? Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "EventMedia", e.Id.ToString()))
                : Array.Empty<string>(),
            EventRecordings = e.EventRecordings // Yeni eklenen özellik
        }).ToList();

        return View(eventViewModels);
    }
    
    public IActionResult GetEvent(int Id)
    {
        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return View("~/Views/Shared/Forbidden.cshtml");
        }
        
        var getEvent = _context.Events.FirstOrDefault(q => q.Id == Id);

        if (getEvent == null)
        {
            return NotFound(); // Etkinlik bulunamazsa 404 döndür
        }

        var eventViewModel = new EventViewModel
        {
            Id = getEvent.Id,
            EventName = getEvent.EventName,
            EventSociety = getEvent.EventSociety,
            EventDescription = getEvent.EventDescription,
            EventStartDate = getEvent.EventStartDate,
            EventEndDate = getEvent.EventEndDate,
            EventLocation = getEvent.EventLocation,
            EventRegisterUrl = getEvent.EventRegisterUrl,
            ExistingImages = Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "EventMedia", getEvent.Id.ToString()))
                ? Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "Source", "EventMedia", getEvent.Id.ToString()))
                : Array.Empty<string>(),
            EventRecordings = getEvent.EventRecordings // Yeni eklenen özellik
        };

        return View(eventViewModel);
    }
}