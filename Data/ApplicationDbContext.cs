using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClinicalResearchWebApp.Models;

namespace ClinicResearchWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<HealthCheckAlert> HealthCheckAlerts { get; set; }
        public DbSet<ServiceOffer> ServiceOffers { get; set; }
    }
}

