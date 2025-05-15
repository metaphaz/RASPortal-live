using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace RASPortal.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Proje adı gereklidir.")]
        [Display(Name = "Proje Adı")]
        public string ProjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proje açıklaması gereklidir.")]
        [Display(Name = "Proje Açıklaması")]
        public string ProjectDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proje yazarı gereklidir.")]
        [Display(Name = "Proje Yazarı")]
        public string ProjectAuthor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proje zorluğu gereklidir.")]
        [Display(Name = "Proje Zorluğu")]
        public ProjectDifficulty ProjectDifficulty { get; set; } = ProjectDifficulty.Basit;

        [Display(Name = "Proje Etiketleri")]
        public ProjectTags[] ProjectTags { get; set; }

        [Display(Name = "Proje Talimatları")]
        public List<InstructionItem> ProjectInstructions { get; set; } = new List<InstructionItem>();

        [Display(Name = "Proje Resimleri")]
        public IFormFile[] ProjectImageFiles { get; set; }

        [Display(Name = "Mevcut Resimler")]
        public string[] ExistingImages { get; set; }

        [Display(Name = "Proje Dosyaları")]
        public IFormFile[] ProjectFilesUpload { get; set; }

        [Display(Name = "Mevcut Dosyalar")]
        public string[] ExistingFiles { get; set; }
    }

    public class InstructionItem
    {
        public string Text { get; set; }
        public string ImagePath { get; set; }
    }
}