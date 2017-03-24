using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DentiCare.Models.UserSettings
{
    public class SetPhotoModel
    {
        [Display(Name = "Plik zdjęcia")]
        [Required(ErrorMessage = "Wybierz plik.")]
        [DataType(DataType.Upload)]
        [ValidatePhoto(ErrorMessage = "Zdjęcie musi mieć rozszerzenie .jpg, .jpeg lub .png i rozmiar niewiększy niż 3MB.")]
        public HttpPostedFileBase PhotoFile { get; set; }
    }

    public class ValidatePhotoAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            const int MAX_CONTENT_LENGTH = 3145728;

            string[] AllowedFileExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            var file = value as HttpPostedFileBase;

            var extension = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();

            if (file == null)
            {
                return false;
            }
            else if (!AllowedFileExtensions.Contains(extension))
            {
                ErrorMessage = "Akceptowalne są tylko pliki .jpg, .jpeg i .png.";
                return false;
            }
            else if (file.ContentLength > MAX_CONTENT_LENGTH)
            {
                ErrorMessage = "Rozmiar zdjęcia musi być niewiększy niż 3MB";
                return false;
            }
            else
            {
                return true;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }
}