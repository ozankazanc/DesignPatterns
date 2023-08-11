using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;

namespace WebApp.Command.Commands
{
    public class PDFFile<T>
    {
        public readonly List<T> _list;
        public string FileName => $"{typeof(T).Name}.pdf";
        public string FileType => $"application/octet-stream";
        
        private readonly HttpContext _context;

        public PDFFile(List<T> list, HttpContext context)
        {
            _list = list;
            _context = context;
        }

        public MemoryStream Create()
        {
            var type = typeof(T);

            var sb = new StringBuilder();
            sb.Append($@"<html>
                                <head></head>
                                <body>
                                    <div class='text-center'><h1>{type.Name}</h1></div>
                                    <table class='table table-striped' align='center'>");

            sb.Append("<tr>");
            type.GetProperties().ToList().ForEach(p =>
            {
                sb.Append($"<th>{p.Name}</th>");
            });
            sb.Append("</tr>");

            _list.ForEach(p =>
            {
                var values = type.GetProperties().Select(propertyInfo => propertyInfo.GetValue(p, null)).ToList();
                sb.Append("<tr>");

                values.ForEach(value =>
                {
                    sb.Append($"<td>{value}</td>");
                });

                sb.Append("</tr>");
            });
            sb.Append("</table></body></html>");


            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = sb.ToString(),
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8",
                            UserStyleSheet=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/lib/bootstrap/dist/css/bootstrap.css")
                        },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                    }
                }
            };

            var converter = _context.RequestServices.GetRequiredService<IConverter>();

            var pdfMemory =new MemoryStream(converter.Convert(doc));

            return pdfMemory;
        }


    }
}
