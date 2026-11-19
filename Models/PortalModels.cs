using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicResearchWebApp.Models
{
    // Extreme Top-Left: Security & Activity History (Audit Trail)
    public class AuditLog
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = "System";
        public string Action { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    // Extreme Top-Right: Monthly Health Check Auto-Emails
    public class HealthCheckAlert
    {
        public int Id { get; set; }
        public string Recipient { get; set; } = string.Empty;
        public string Status { get; set; } = "Scheduled";
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }

    // Extreme Bottom-Center: Home Services & Offers
    public class ServiceOffer
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string BonusOffer { get; set; } = string.Empty;
    }
}
