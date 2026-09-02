using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace TgiControl.Engines;

public interface IDocumentEngine
{
    Task<DocumentRecord> UploadAsync(
        Stream s,
        string name,
        string type,
        string entity,
        string id,
        UserContext user,
        CancellationToken ct);
}

public sealed class DocumentEngine : IDocumentEngine
{
    private readonly IConfiguration _cfg;
    private readonly IWebHostEnvironment _env;

    public DocumentEngine(IConfiguration cfg, IWebHostEnvironment env)
    {
        _cfg = cfg;
        _env = env;
    }

    public async Task<DocumentRecord> UploadAsync(
        Stream s,
        string name,
        string type,
        string entity,
        string id,
        UserContext u,
        CancellationToken ct)
    {
        var safe = Path.GetFileName(name);
        var key = $"{u.Center}/{entity}/{id}/{Guid.NewGuid():N}-{safe}";
        string uri;

        if ((_cfg["Documents:Provider"] ?? "Local") == "Blob")
        {
            var account = _cfg["Documents:BlobAccount"]
                ?? throw new InvalidOperationException("Cuenta Blob no configurada");
            var cc = new BlobContainerClient(
                new Uri($"https://{account}.blob.core.windows.net/{_cfg["Documents:Container"] ?? "operational-documents"}"),
                new DefaultAzureCredential());
            await cc.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

            var blob = cc.GetBlobClient(key);
            await blob.UploadAsync(s, new BlobHttpHeaders { ContentType = type }, ct);
            uri = blob.Uri.ToString();
        }
        else
        {
            var path = Path.Combine(_env.ContentRootPath, "App_Data", "documents",
                key.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            await using var f = File.Create(path);
            await s.CopyToAsync(f, ct);
            uri = path;
        }

        return new(
            Guid.NewGuid(),
            entity,
            id,
            safe,
            type,
            uri,
            u.Email,
            DateTime.UtcNow,
            u.Center
        );
    }
}