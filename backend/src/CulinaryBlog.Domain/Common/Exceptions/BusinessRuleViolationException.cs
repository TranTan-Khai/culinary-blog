namespace CulinaryBlog.Domain.Common.Exceptions;

/// <summary>
/// Thao tác làm entity rơi vào trạng thái không hợp lệ theo quy tắc nghiệp vụ
/// (ví dụ số lượng nguyên liệu âm). Map sang HTTP 422.
/// </summary>
public sealed class BusinessRuleViolationException : DomainException
{
    public BusinessRuleViolationException(string message)
        : base(message)
    {
    }
}
