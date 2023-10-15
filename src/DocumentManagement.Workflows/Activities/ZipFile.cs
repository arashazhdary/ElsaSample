using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Providers.WorkflowStorage;
using Elsa.Services;
using Elsa.Services.Models;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace DocumentManagement.Workflows.Activities;

[Action(Category = "Documents", Description = "Zips the specified file.")]
public class ZipFile : Activity
{
    [ActivityInput(
        Hint = "The file stream to zip.",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript },
        DefaultWorkflowStorageProvider = TransientWorkflowStorageProvider.ProviderName,
        DisableWorkflowProviderSelection = true
    )]
    public Stream Stream { get; set; } = new MemoryStream();

    [ActivityInput(
        Hint = "The file name to use for the zip entry.",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript }
    )]
    public string FileName { get; set; } = default!;

    [ActivityOutput(
        Hint = "The zipped file stream.",
        DefaultWorkflowStorageProvider = TransientWorkflowStorageProvider.ProviderName,
        DisableWorkflowProviderSelection = true
    )]
    public Stream Output { get; set; } = default!;

    protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        var outputStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
        {
            var zipEntry = zipArchive.CreateEntry(FileName, CompressionLevel.Optimal);
            await using var zipStream = zipEntry.Open();
            await Stream.CopyToAsync(zipStream);
        }

        // Reset position.
        outputStream.Seek(0, SeekOrigin.Begin);
        Output = outputStream;
        return Done();
    }

    //private async Task rr()
    //{
    //    var zipFileMemoryStream = new MemoryStream();
    //    using (ZipArchive archive = new ZipArchive(zipFileMemoryStream, ZipArchiveMode.Create, leaveOpen: true))
    //    {
    //        string filePath = @"D:\Test\download.png";
    //        string fileName = @"download.png";
    //        var entry = archive.CreateEntry(fileName);
    //        using (var entryStream = entry.Open())
    //        {
    //            using (var fileStream = File.OpenRead(filePath))
    //            {
    //                await fileStream.CopyToAsync(entryStream);
    //            }
    //        }
    //    }
    //    zipFileMemoryStream.Seek(0, SeekOrigin.Begin);
    //    Output = zipFileMemoryStream;
    //}
}