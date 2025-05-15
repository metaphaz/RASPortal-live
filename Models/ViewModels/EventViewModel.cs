using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // IFormFile için eklendi

namespace RASPortal.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Etkinlik adı gereklidir.")]
        [Display(Name = "Etkinlik Adı")]
        public string EventName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Topluluk adı gereklidir.")]
        [Display(Name = "Topluluk Adı")]
        public EventSocietyType EventSociety { get; set; } = EventSocietyType.RoboticsAndAutomationSociety;
        
        [Required(ErrorMessage = "Etkinlik açıklaması gereklidir.")]
        [Display(Name = "Etkinlik Açıklaması")]
        public string EventDescription { get; set; } = String.Empty;

        [Required(ErrorMessage = "Başlangıç tarihi gereklidir.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime EventStartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Bitiş tarihi gereklidir.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime EventEndDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Etkinlik konumu gereklidir.")]
        [Display(Name = "Etkinlik Konumu")]
        public string EventLocation { get; set; } = String.Empty;

        [Display(Name = "Kayıt URL'si")]
        public string EventRegisterUrl { get; set; } = String.Empty;

        // Medya alanları
        [Display(Name = "Etkinlik Resimleri")]
        public IFormFile[] EventImageFiles { get; set; } // Resim yükleme için

        [Display(Name = "Mevcut Resimler")]
        public string[] ExistingImages { get; set; } // Mevcut resim dosyaları için
        
        [Display(Name = "Etkinlik Kayıtları")]
        public string[] EventRecordings { get; set; } = []; // Etkinlik kayıtları için
    }
}