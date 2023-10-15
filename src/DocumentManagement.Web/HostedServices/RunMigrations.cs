using DocumentManagement.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.Web.HostedServices
{
    /// <summary>
    /// Executes EF Core migrations.
    /// </summary>
    public class RunMigrations : IHostedService
    {
        private readonly IDbContextFactory<DocumentDbContext> _dbContextFactory;

        public RunMigrations(IDbContextFactory<DocumentDbContext> dbContextFactoryFactory)
        {
            _dbContextFactory = dbContextFactoryFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();
            await dbContext.Database.MigrateAsync(cancellationToken);
            await dbContext.DisposeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}