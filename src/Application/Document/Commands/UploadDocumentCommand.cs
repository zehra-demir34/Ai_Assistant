using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Document.Commands
{
    public record UploadDocumentCommand(Guid SessionId, IFormFile File, Guid UserId): IRequest<UploadDocumentResponse>;
    public record UploadDocumentResponse(Guid? DocumentId, string Message);

}
