using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DentiCare.Models.Patients
{
    public class DocumentModel
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public string Url { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Data dodania")]
        public DateTime UploadDate { get; set; }

        [Display(Name = "Plik dokumentu")]
        [Required(ErrorMessage = "Wybierz plik.")]
        [DataType(DataType.Upload)]
        [ValidateDocument(ErrorMessage = "Dokument musi mieć rozszerzenie .doc, docx, .pdf, .jpg, .jpeg lub .png i rozmiar niewiększy niż 3MB.")]
        public HttpPostedFileBase DocumentFile { get; set; }
    }

    public class ValidateDocumentAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            const int MAX_CONTENT_LENGTH = 3145728;

            string[] AllowedFileExtensions = new string[] { ".doc", ".docx", ".pdf", ".jpg", ".png", ".jpeg" };
            var file = value as HttpPostedFileBase;

            var extension = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();

            if (file == null)
            {
                return false;
            }
            else if (!AllowedFileExtensions.Contains(extension))
            {
                ErrorMessage = "Akceptowalne są tylko pliki .doc, .docx, .pdf, .jpg, .jpeg i .png.";
                return false;
            }
            else if (file.ContentLength > MAX_CONTENT_LENGTH)
            {
                ErrorMessage = "Rozmiar pliku musi być niewiększy niż 3MB";
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