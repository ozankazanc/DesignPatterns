using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO.Compression;
using WebApp.Command.Commands;
using WebApp.Command.Models;

namespace WebApp.Command.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppIdentityDbContext _appIdentityDbContext;

        public ProductsController(AppIdentityDbContext appIdentityDbContext)
        {
            _appIdentityDbContext = appIdentityDbContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _appIdentityDbContext.Products.ToListAsync());
        }


        public async Task<IActionResult> CreateFile(int type)
        {
            var products = await _appIdentityDbContext.Products.ToListAsync();

            FileCreateInvoker fileCreateInvoker = new FileCreateInvoker();


            EFileType fileType = (EFileType)type;

            switch (fileType)
            {
                case EFileType.Excel:
                    ExcelFile<Product> excelFile = new ExcelFile<Product>(products);
                    fileCreateInvoker.SetCommand(new CreateExcelTableActionCommand<Product>(excelFile));
                    break;
                case EFileType.Pdf:
                    PDFFile<Product> pdfFile = new PDFFile<Product>(products, HttpContext);
                    fileCreateInvoker.SetCommand(new CreatePdfTableActionCommand<Product>(pdfFile));
                    break;
                default:
                    break;
            }

            return fileCreateInvoker.CreateFile();
        }

        public async Task<IActionResult> CreateFiles()
        {
            var products = await _appIdentityDbContext.Products.ToListAsync();

            FileCreateInvoker fileCreateInvoker = new FileCreateInvoker();
            ExcelFile<Product> excelFile = new ExcelFile<Product>(products);
            PDFFile<Product> pdfFile = new PDFFile<Product>(products, HttpContext);

            fileCreateInvoker.AddCommand(new CreateExcelTableActionCommand<Product>(excelFile));
            fileCreateInvoker.AddCommand(new CreatePdfTableActionCommand<Product>(pdfFile));

            var fileResult = fileCreateInvoker.CreateFiles();

            using (var zipMemoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create))
                {
                    foreach (var file in fileResult)
                    {
                        var fileContent = file as FileContentResult;
                        var zipFile = archive.CreateEntry(fileContent.FileDownloadName);

                        using (var zipEntryStream = zipFile.Open())
                        {
                            await new MemoryStream(fileContent.FileContents).CopyToAsync(zipEntryStream);
                        }
                    }
                }

                return File(zipMemoryStream.ToArray(),"application/zip","all.zip");
            }


            return fileCreateInvoker.CreateFile();
        }
    }
}

