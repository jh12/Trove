using System.Diagnostics;

namespace Trove.Shared;

[DebuggerDisplay("{Value}")]
public struct SGuid
{
    public static readonly SGuid Empty = new (Guid.Empty);
    public static SGuid NewGuid() => new (Guid.NewGuid());


    private readonly Guid _underlyingGuid;
    private readonly string _encodedString;

    public Guid Guid => _underlyingGuid;
    public string Value => _encodedString;

    public SGuid(string value)
    {
        _encodedString = value;
        _underlyingGuid = Decode(value);
    }

    public SGuid(Guid guid)
    {
        _encodedString = Encode(guid);
        _underlyingGuid = guid;
    }

    public override string ToString() => _encodedString;

    public override bool Equals(object? obj)
    {
        if (obj is SGuid shortGuid)
        {
            return _underlyingGuid.Equals(shortGuid._underlyingGuid);
        }

        if (obj is Guid guid)
        {
            return _underlyingGuid.Equals(guid);
        }

        if (obj is string str)
        {
            // Try a ShortGuid string.
            if (TryDecode(str, out guid))
                return _underlyingGuid.Equals(guid);

            // Try a guid string.
            if (Guid.TryParse(str, out guid))
                return _underlyingGuid.Equals(guid);
        }

        return false;
    }

    public override int GetHashCode() => _underlyingGuid.GetHashCode();

    public static string Encode(string value)
    {
        var guid = new Guid(value);
        return Encode(guid);
    }

    public static string Encode(Guid guid)
    {
        string encoded = Convert.ToBase64String(guid.ToByteArray());

        encoded = encoded
            .Replace("/", "_")
            .Replace("+", "-");

        return encoded[..22];
    }

    public static Guid Decode(string value)
    {
        // avoid parsing larger strings/blobs
        if (value?.Length != 22)
        {
            throw new ArgumentException(
                $"A ShortGuid must be exactly 22 characters long. Received a {value?.Length ?? 0} character string.",
                paramName: nameof(value)
            );
        }

        string base64 = value
            .Replace("_", "/")
            .Replace("-", "+") + "==";

        byte[] blob = Convert.FromBase64String(base64);
        var guid = new Guid(blob);

        var sanityCheck = Encode(guid);
        if (sanityCheck != value)
        {
            throw new FormatException(
                $"Invalid strict ShortGuid encoded string. The string '{value}' is valid URL-safe Base64, " +
                $"but failed a round-trip test expecting '{sanityCheck}'."
            );
        }

        return guid;
    }

    public static bool TryDecode(string value, out Guid guid)
    {
        try
        {
            guid = Decode(value);
            return true;
        }
        catch
        {
            guid = Guid.Empty;
            return false;
        }
    }

    public static bool TryParse(string value, out SGuid shortGuid)
    {
        // Try a ShortGuid string.
        if (SGuid.TryDecode(value, out var guid))
        {
            shortGuid = new SGuid(guid);
            return true;
        }

        // Try a Guid string.
        if (Guid.TryParse(value, out guid))
        {
            shortGuid = new SGuid(guid);
            return true;
        }

        shortGuid = SGuid.Empty;
        return false;
    }
}