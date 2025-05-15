using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace RASPortal.Models
{
    public enum ProjectTags
    {
        Elektronik,
        Mekanik,
        Yazılım,
        IoT,
        AI,
        Robotik,
        Görüntüİşleme,
    }

    public enum ProjectDifficulty
    {
        Basit,
        Orta,
        Zor,
    }

    public class Project
    {
        [Key]
        public int Id { get; set; }

        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public string ProjectAuthor { get; set; } = string.Empty;
        public ProjectDifficulty ProjectDifficulty { get; set; } = ProjectDifficulty.Basit;
        public ProjectTags[] ProjectTags { get; set; }
        public string[] ProjectInstructions { get; set; } = Array.Empty<string>();

        public string[] ProjectImages
        {
            get
            {
                var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Source", "Projects", Id.ToString(), "Images");
                if (Directory.Exists(imageDirectory))
                {
                    return Directory.GetFiles(imageDirectory);
                }
                return Array.Empty<string>();
            }
        }

        public string[] ProjectFiles
        {
            get
            {
                var fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Source", "Projects", Id.ToString(), "Files");
                if (Directory.Exists(fileDirectory))
                {
                    return Directory.GetFiles(fileDirectory);
                }
                return Array.Empty<string>();
            }
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}