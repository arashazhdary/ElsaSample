using DocumentManagement.Core.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.Core.Services;

public interface IDocumentService
{
    Task<Document> SaveDocumentAsync(string fileName, Stream data, string documentTypeId, CancellationToken cancellationToken = default);
}
