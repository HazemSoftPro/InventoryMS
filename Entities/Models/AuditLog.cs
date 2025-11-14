using System;

namespace Entities.Models
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public Guid EntityId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? ModifiedFields { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserRoles { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? TableName { get; set; }
        public string? PrimaryKey { get; set; }
        public AuditType AuditType { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? ErrorMessage { get; set; }
        public string? RequestId { get; set; }
        public string? SessionId { get; set; }
        public string? CompanyId { get; set; }
        public string? OfficeId { get; set; }
    }

    public enum AuditType
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Login = 4,
        Logout = 5,
        Access = 6,
        Export = 7,
        Import = 8,
        Print = 9,
        Email = 10,
        Backup = 11,
        Restore = 12,
        Other = 99
    }
}