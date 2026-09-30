namespace CulinaryBlog.Application.Common.Exceptions;

/// <summary>
/// Người dùng đã xác thực nhưng không có quyền trên tài nguyên (không phải Owner/Admin). Map sang HTTP 403.
/// </summary>
public sealed class ForbiddenException : Exception
{
    public ForbiddenException(string message = "You do not have permission to perform this action.")
        : base(message)
    {
    }
}
