namespace CulinaryBlog.Application.Common.Exceptions;

/// <summary>
/// Xung đột với trạng thái hiện tại của tài nguyên (trùng slug, RowVersion lệch...). Map sang HTTP 409.
/// </summary>
public sealed class ConflictException : Exception
{
    public ConflictException(string message)
        : base(message)
    {
    }
}
