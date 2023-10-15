using DocumentManagement.Core.Models;
using System.IO;

namespace DocumentManagement.Workflows.Activities;

public record DocumentFile(Document Document, Stream FileStream);