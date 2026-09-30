namespace CulinaryBlog.Application.Common.Exceptions;

/// <summary>
/// Dữ liệu đầu vào không hợp lệ. Errors map tên field → danh sách lỗi,
/// khớp với trường "errors" của RFC 7807 ValidationProblemDetails (HTTP 422).
/// </summary>
public sealed class ValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}
