using Microsoft.AspNetCore.Mvc;

namespace WebApp.Command.Commands
{
    public class CreatePdfTableActionCommand<T> : ITableActionCommand
    {
        private readonly PDFFile<T> _pdfFile;

        public CreatePdfTableActionCommand(PDFFile<T> pdfFile)
        {
            _pdfFile = pdfFile;
        }
        public IActionResult Execute()
        {
            var excelMemoryStream = _pdfFile.Create();
            return new FileContentResult(excelMemoryStream.ToArray(), _pdfFile.FileType) { FileDownloadName = _pdfFile.FileName };
        }
    }
}
