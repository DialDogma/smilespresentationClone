using System;
using System.Text;

public static class Base64UrlExtensions
{
    /// <summary>
    /// เข้ารหัส string เป็น Base64 แบบ URL-safe (ไม่มี =, +, /)
    /// </summary>
    public static string ToBase64Url(this string input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        byte[] bytes = Encoding.UTF8.GetBytes(input);
        string base64 = Convert.ToBase64String(bytes);

        return base64
            .TrimEnd('=')         // ตัด padding
            .Replace('+', '-')    // URL-safe
            .Replace('/', '_');
    }

    /// <summary>
    /// ถอดรหัส Base64 URL-safe string กลับเป็น string ปกติ
    /// </summary>
    public static string FromBase64Url(this string input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        string base64 = input
            .Replace('-', '+')
            .Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
            case 0: break; // ปกติ
            default:
                throw new FormatException("Invalid Base64 URL-safe string.");
        }

        byte[] bytes = Convert.FromBase64String(base64);
        return Encoding.UTF8.GetString(bytes);
    }
}