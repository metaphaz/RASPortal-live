using Microsoft.AspNetCore.Identity;
using System;

namespace RASPortal.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Custom properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        // Profile files and role-specific information
        public string? ProfilePictureFilePath { get; set; }
        public string? ResumeFilePath { get; set; }

        // You can add an enum property for Gender or any other custom field
        public GenderType Gender { get; set; }
    }
    
    public enum GenderType
    {
        Erkek,
        Kadın
    }
}