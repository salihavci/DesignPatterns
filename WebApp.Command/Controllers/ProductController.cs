using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using WebApp.Command.Commands;
using WebApp.Command.Models;

namespace WebApp.Command.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public ProductController(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public async Task<IActionResult> CreateFile(int type)
        {
            var products = await _context.Products.ToListAsync();
            FileCreateInvoker invoker = new FileCreateInvoker();
            EFileType fileType = (EFileType)type;
            switch (fileType)
            {
                case EFileType.Excel:
                    ExcelFile<Product> excelFile = new ExcelFile<Product>(products);
                    invoker.SetCommand(new CreateExcelTableActionCommand<Product>(excelFile));
                    break;
                case EFileType.PDF:
                    PdfFile<Product> pdfFile = new PdfFile<Product>(products, HttpContext);
                    invoker.SetCommand(new CreatePdfTableActionCommand<Product>(pdfFile));
                    break;
                default:
                    break;
            }
            return invoker.CreateFile();
        }

        public async Task<IActionResult> CreateFiles()
        {
            var products = await _context.Products.ToListAsync();
            ExcelFile<Product> excel = new ExcelFile<Product>(products);
            PdfFile<Product> pdf = new PdfFile<Product>(products,HttpContext);
            FileCreateInvoker invoker = new FileCreateInvoker();
            invoker.AddCommand(new CreateExcelTableActionCommand<Product>(excel));
            invoker.AddCommand(new CreatePdfTableActionCommand<Product>(pdf));
            var filesResult = invoker.CreateFiles();
            using (var zipMemoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipMemoryStream,ZipArchiveMode.Create))
                {
                    foreach (var item in filesResult)
                    {
                        var fileContent = item as FileContentResult;
                        var zipFile = archive.CreateEntry(fileContent.FileDownloadName);
                        using (var zipEntryStream = zipFile.Open())
                        {
                            await new MemoryStream(fileContent.FileContents).CopyToAsync(zipEntryStream);
                        }
                    }
                }
                return File(zipMemoryStream.ToArray(), "application/zip", "all.zip");
            }
        }
    }
}
