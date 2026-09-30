using Application.Document;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace Infrastructure.Document
{
    public class DocumentTextExtractor: IDocumentTextExtractor
    {
        public async Task<string> ExtractTextAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
        {
            var extension=Path.GetExtension(fileName).ToLowerInvariant();

            switch (extension)
            {
                case ".txt":
                    using (var reader = new StreamReader(fileStream))
                    {
                        return await reader.ReadToEndAsync(cancellationToken);
                    }

                case ".pdf":
                    using (var pdf = PdfDocument.Open(fileStream))
                    {
                        var builder = new StringBuilder();

                        foreach (var page in pdf.GetPages())
                        {
                            builder.AppendLine(page.Text);
                        }

                        return builder.ToString();
                    }

                case ".docx":
                    using (var document = WordprocessingDocument.Open(fileStream, false))
                    {
                        var body = document.MainDocumentPart?.Document?.Body;

                        return body?.InnerText ?? string.Empty;
                    }

                default:
                    throw new NotSupportedException($"Desteklenmeyen dosya türü: {extension}");
            }
        }
    }
}
