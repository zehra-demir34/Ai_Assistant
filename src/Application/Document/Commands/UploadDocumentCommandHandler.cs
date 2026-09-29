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
    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, UploadDocumentResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDocumentTextExtractor _documentTextExtractor;

        public UploadDocumentCommandHandler(IApplicationDbContext context, IDocumentTextExtractor documentTextExtractor)
        {
            _context = context;
            _documentTextExtractor = documentTextExtractor;
        }

        public async Task<UploadDocumentResponse> Handle(UploadDocumentCommand request,CancellationToken cancellationToken)
        {
            var session = await _context.ChatSessions
                .FirstOrDefaultAsync(x => x.SessionId == request.SessionId && x.UserId == request.UserId,cancellationToken);

            if (session is null)
            {
                return new UploadDocumentResponse(null,"Bu session'a erişim yetkiniz yok.");
            }

            const int maxDocumentCount = 3;
            const long maxFileSize = 3 * 1024 * 1024;

            var documentCount=await _context.Documents
                .CountAsync(x => x.SessionId == request.SessionId && x.UserId == request.UserId, cancellationToken);

            if (documentCount >= maxDocumentCount)
            {
                return new UploadDocumentResponse(null, "Bir session için en fazla 3 dosya yüklenebilir.");
            }

            if(request.File is null||request.File.Length == 0)
            {
                return new UploadDocumentResponse(null,"Lütfen bir dosya seçiniz.");
            }

            if(request.File.Length > maxFileSize)
            {
                return new UploadDocumentResponse(null, "Dosya boyutu 3 MB'dan büyük olamaz.");
            }

            using var stream = request.File.OpenReadStream();

            var content = await _documentTextExtractor.ExtractTextAsync(stream, request.File.FileName, cancellationToken);

            if (string.IsNullOrWhiteSpace(content))
            {
                return new UploadDocumentResponse(null, "Dosyadan metin çıkarılamadı.");
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

            return new UploadDocumentResponse(document.DocumentId,"Dosya başarıyla yüklendi.");
        }
    }
}
