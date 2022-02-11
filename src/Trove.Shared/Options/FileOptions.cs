using System.Text.Json.Serialization;

namespace Trove.Shared.Options;

public class FileOptions
{
    public const string Key = "File";

    [JsonPropertyName("Upload")]
    public UploadOptions Upload { get; set; } = new UploadOptions();

    [JsonPropertyName("Storage")]
    public StorageOptions Storage { get; set; } = new StorageOptions();

    public class UploadOptions
    {
        public int MaxFileSizeBytes { get; set; } = 100 * 1024 * 1024; // 100 MB
    }

    public class StorageOptions
    {
        public string Path { get; set; } = string.Empty;
    }
}