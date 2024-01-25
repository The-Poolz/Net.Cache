using System.Text;

namespace Net.Cache.DynamoDb.ERC20.Cryptography;

public static class SHA256
{
    public static string ToSha256(this string str)
    {
        using var sha256Hash = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str));

        var builder = new StringBuilder();
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }
        return builder.ToString();
    }
}