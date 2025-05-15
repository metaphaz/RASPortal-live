using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; 

namespace RASPortal.Models
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "E-posta")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        public string Email { get; set; }

        [Display(Name = "Telefon Numarası")]
        [Phone(ErrorMessage = "Geçersiz telefon numarası.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        
        public string? ProfilePictureFilePath { get; set; }
        
        public string? ResumeFilePath { get; set; }

        [Display(Name = "Cinsiyet")]
        public GenderType Gender { get; set; }
        
        [Display(Name = "Profil Resmi")]
        public IFormFile? ProfilePictureFile { get; set; }

        [Display(Name = "CV")]
        public IFormFile? ResumeFile { get; set; }
    }
}