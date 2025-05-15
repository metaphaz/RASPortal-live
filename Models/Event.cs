using System;
using System.ComponentModel.DataAnnotations;

namespace RASPortal.Models;

public enum EventSocietyType
{
    CommunicationsSociety,
    ComputerSociety,
    EducationSociety,
    PowerAndEnergySociety,
    RoboticsAndAutomationSociety,
    WomenInEngineeringSociety,
    SponsorlukKomitesi,
    EtkinlikKoorinatörlüğü,
    TanıtımTasarımKomitesi,
}
public class Event
{
    [Key]
    public int Id { get; set; }
    public String EventName { get; set; } = String.Empty;
    public EventSocietyType EventSociety { get; set; } = EventSocietyType.RoboticsAndAutomationSociety;
    public String EventDescription { get; set; } = String.Empty;
    public DateTime EventStartDate { get; set; } = DateTime.Today;
    public DateTime EventEndDate { get; set; } = DateTime.Today;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public String EventLocation { get; set; } = String.Empty;

    // This is the new EventMedia property
    // It will store an array of paths, each relative to wwwroot
    // e.g., "/uploads/events/event1/image1.jpg", "/uploads/events/event1/video_promo.mp4"
    public string[] EventMedia { get; set; } = Array.Empty<string>();

    public String EventRegisterUrl { get; set; } = String.Empty;
        
    // EventRecordings can remain if it serves a different purpose (e.g., external recording links)
    // Or it could be merged/replaced by EventMedia if they are conceptually the same.
    // For now, I'll assume EventRecordings is distinct or you'll handle its consolidation.
    public String[] EventRecordings { get; set; } = Array.Empty<string>();

    // Ensure any old definition of EventMedia, especially the one using Directory.GetFiles, is removed.
}