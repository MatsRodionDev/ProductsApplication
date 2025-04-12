﻿namespace UserService.DAL.Outbox
{
    public sealed class OutboxMessageEntity
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime OccuredOnUtc { get; set; }
        public DateTime? ProcessedOnUtc { get; set; }
        public string? Error { get; set; }
    }
}
