using System;
namespace Accounting.Application;

public static class Shared
{
    public static string? GetAttachmentUrl(string attachment)
    {
        if (attachment is null) return null;

        return $"http://127.0.0.1:10000/devstoreaccount1/attachments/{attachment}";
    }
}