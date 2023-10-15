using DocumentManagement.Core.Models;
using DocumentManagement.Core.Services;
using DocumentManagement.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.Web.Services;

public class EFCoreDocumentTypeStore : IDocumentTypeStore
{
    private readonly IDbContextFactory<DocumentDbContext> _dbContextFactory;

    public EFCoreDocumentTypeStore(IDbContextFactory<DocumentDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IEnumerable<DocumentType>> ListAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.DocumentTypes.ToListAsync(cancellationToken);
    }

    public async Task<DocumentType?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.DocumentTypes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}