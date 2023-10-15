using DocumentManagement.Workflows.Activities;
using DocumentManagement.Workflows.Handlers;
using DocumentManagement.Workflows.Scripting.JavaScript;
using Elsa;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Server.Hangfire.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DocumentManagement.Workflows.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWorkflowServices(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDb)
    {
        return services.AddNotificationHandlersFrom<StartDocumentWorkflows>().AddElsa(configureDb);
    }

    private static IServiceCollection AddElsa(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDb)
    {

        // Register custom type definition provider for JS intellisense.
        services.AddJavaScriptTypeDefinitionProvider<CustomTypeDefinitionProvider>();
        services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();
        services
            .AddElsa(elsa => elsa
                // Use EF Core's SQLite provider to store workflow instances and bookmarks.
                .UseEntityFrameworkPersistence(configureDb)

                // Ue Console activities for testing & demo purposes.
                .AddConsoleActivities()
                // Use Hangfire to dispatch workflows from.
                .UseHangfireDispatchers()
                // Configure Email activities.
                .AddEmailActivities()
                // Configure HTTP activities.
                .AddHttpActivities()
                .AddActivitiesFrom<ArchiveDocument>()
                .AddActivitiesFrom<GetDocument>()
                .AddActivitiesFrom<UpdateBlockchain>()
                .AddActivitiesFrom<ZipFile>()
            );

        return services;
    }
}