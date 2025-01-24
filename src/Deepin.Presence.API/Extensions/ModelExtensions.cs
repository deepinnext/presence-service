using Deepin.Storage.API.Application.Models;
using Deepin.Storage.API.Infrastructure.Entitites;

namespace Deepin.Storage.API;

public static class ModelExtensions
{

    public static FileModel ToModel(this FileObject file) => new FileModel
    {
        Provider = file.Provider,
        Checksum = file.Checksum,
        Hash = file.Hash,
        MimeType = file.MimeType,
        Id = file.Id,
        Path = file.Path,
        Name = file.Name,
        Format = file.Format,
        Length = file.Length,
        CreatedBy = file.CreatedBy,
        CreatedAt = file.CreatedAt,
        UpdatedAt = file.UpdatedAt
    };
}
