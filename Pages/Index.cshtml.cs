@page
@using ClinicResearchWebApp.Pages
@model IndexModel

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClinicResearchWebApp.Data;
using ClinicResearchWebApp.Models;


namespace ClinicResearchWebApp.Pages
{
    public class IndexModel : PageModel
    {    
      
      public void OnGet()
{
}
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<AuditLog> RecentLogs { get; set; } = new();
        public List<HealthCheckAlert> EmailAlerts { get; set; } = new();
        public List<ServiceOffer> Offers { get; set; } = new();
        public bool IsDatabaseHealthy { get; set; }
        public long DbLatencyMs { get; set; }
        public int TotalRecords { get; set; }

        public async Task OnGetAsync()
        {
            if (!await _db.ServiceOffers.AnyAsync())
            {
                _db.ServiceOffers.AddRange(
                    new ServiceOffer { Title = "At-Home Blood Pressure Monitoring", Description = "Remote telemetry setup for clinical participants.", BonusOffer = "Includes Free Telehealth Consultation" },
                    new ServiceOffer { Title = "In-Home Nursing Visit", Description = "Scheduled sample collection & vitals check.", BonusOffer = "Priority Lab Results Delivery" }
                );
                await _db.SaveChangesAsync();
            }

            var sw = Stopwatch.StartNew();
            IsDatabaseHealthy = await _db.Database.CanConnectAsync();
            sw.Stop();
            DbLatencyMs = sw.ElapsedMilliseconds;

            RecentLogs = await _db.AuditLogs.OrderByDescending(x => x.Timestamp).Take(5).ToListAsync();
            EmailAlerts = await _db.HealthCheckAlerts.OrderByDescending(x => x.SentAt).Take(4).ToListAsync();
            Offers = await _db.ServiceOffers.ToListAsync();
            TotalRecords = await _db.AuditLogs.CountAsync() + await _db.HealthCheckAlerts.CountAsync();
        }

        public async Task<IActionResult> OnPostRequestServiceAsync(string serviceTitle)
        {
            var user = User.Identity?.Name ?? "Guest User";

            _db.AuditLogs.Add(new AuditLog
            {
                UserEmail = user,
                Action = $"Requested '{serviceTitle}' + Auto-Emailed Offer Bundle",
                Timestamp = DateTime.UtcNow
            });

            _db.HealthCheckAlerts.Add(new HealthCheckAlert
            {
                Recipient = user,
                Status = $"Offer Email Sent: {serviceTitle}",
                SentAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}

