using Application.Chat;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainDocument=Domain.Entities.Document;

namespace Application.Document.Commands
{
    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDocumentTextExtractor _documentTextExtractor;

        public UploadDocumentCommandHandler(IApplicationDbContext context, IDocumentTextExtractor documentTextExtractor)
        {
            _context = context;
            _documentTextExtractor = documentTextExtractor;
        }

        public async Task<Guid> Handle(UploadDocumentCommand request,CancellationToken cancellationToken)
        {
            var session = await _context.ChatSessions
                .FirstOrDefaultAsync(x => x.SessionId == request.SessionId && x.UserId == request.UserId,cancellationToken);

            if (session is null)
            {
                throw new UnauthorizedAccessException("Bu session'a erişim yetkiniz yok.");
            }

            using var stream = request.File.OpenReadStream();

            var content = await _documentTextExtractor.ExtractTextAsync(stream, request.File.FileName, cancellationToken);

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new InvalidOperationException("Dosyadan metin çıkarılamadı.");
            }

            var document = new DomainDocument
            {
                DocumentId = Guid.NewGuid(),
                UserId = request.UserId,
                SessionId = request.SessionId,
                FileName = request.File.FileName,
                FileType = Path.GetExtension(request.File.FileName).ToLowerInvariant(),
                Content = content,
                UploadedAt = DateTime.UtcNow
            };

            _context.Documents.Add(document);

            await _context.SaveChangesAsync(cancellationToken);

            return document.DocumentId;
        }
    }
}
