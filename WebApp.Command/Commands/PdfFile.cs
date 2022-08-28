using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebApp.Command.Commands
{
    public class PdfFile<T>
    {
        public readonly List<T> list;
        public readonly HttpContext context;
        public string FileName => $"{typeof(T).Name}.pdf";
        public string FileType => "application/octet-stream";

        public PdfFile(List<T> list, HttpContext context)
        {
            this.list = list;
            this.context = context;
        }


        public MemoryStream Create()
        {
            var type = typeof(T);
            var sb = new StringBuilder();
            sb.Append($@"<html>
                                <head></head>
                                <body>
                                    <div class='text-center'><h1>{type.Name} Tablosu</h1></div>
                                    <table class='table table-striped' align='center'>");
            sb.Append("<tr>");
            type.GetProperties().ToList().ForEach(x =>
            {
                sb.Append($"<th>{x.Name}</th>");
            });
            sb.Append("</tr>");

            this.list.ForEach(x =>
            {
                var values = type.GetProperties().Select(m => m.GetValue(x)).ToList();
                sb.Append("<tr>");
                values.ForEach(value =>
                {
                    sb.Append($@"<td>{value}</td>");
                });
                sb.Append("</tr>");
            });
            sb.Append($@"</table>
                            </body>
                        </html>");
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = sb.ToString(),
                        WebSettings = {DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/lib/bootstrap/dist/css/bootstrap.css")},
                        HeaderSettings = {FontSize = 9, Right = "Sayfa [page] - [toPage]", Line=true,Spacing = 2.812}
                    }
                }
            };

            var converter = context.RequestServices.GetRequiredService<IConverter>();
            var result = converter.Convert(doc);
            MemoryStream pdfMemoryStream = new MemoryStream(result);
            return pdfMemoryStream;
        }
    }
}
