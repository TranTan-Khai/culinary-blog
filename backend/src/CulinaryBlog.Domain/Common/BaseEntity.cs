namespace CulinaryBlog.Domain.Common;

/// <summary>
/// Lớp cơ sở cho tất cả các Entity trong hệ thống.
/// Cung cấp: khóa chính UUID, audit fields (CreatedAt/UpdatedAt),
/// Soft Delete flag (IsDeleted) và Optimistic Concurrency Token (RowVersion).
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    /// <summary>
    /// Concurrency token. EF Core map sang cột "bytea" (timestamp) trên PostgreSQL
    /// thông qua .IsRowVersion() trong Configuration, KHÔNG dùng [Timestamp] attribute
    /// (attribute đó chỉ hoạt động chuẩn trên SQL Server).
    /// </summary>
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
