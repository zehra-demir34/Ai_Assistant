using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Document
{
    public interface IDocumentTextExtractor
    {
        Task<string> ExtractTextAsync(Stream fileStream, string fileName, CancellationToken cancellationToken);
    }
}
