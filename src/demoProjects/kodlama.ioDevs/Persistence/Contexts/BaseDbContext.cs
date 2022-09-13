using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Security.Entities;


namespace Persistence.Contexts
{
    public class BaseDbContext : DbContext
    {
        protected IConfiguration Configuration { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<EmailAuthenticator> EmailAuthenticators { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<OtpAuthenticator> OtpAuthenticators { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }

        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //    base.OnConfiguring(
            //        optionsBuilder.UseSqlServer(Configuration.
            //           GetConnectionString("KodlamaioDevsConnectionString")));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>(a =>
            {
                a.ToTable("Languages").HasKey(k => k.Id);
                a.Property(t => t.Id).HasColumnName("Id");
                a.Property(t => t.Name).HasColumnName("Name");
                a.HasMany(l => l.Technologies);
            });

            modelBuilder.Entity<Technology>(a =>
            {
                a.ToTable("Technologies").HasKey(k => k.Id);
                a.Property(t => t.Id).HasColumnName("Id");
                a.Property(t => t.Name).HasColumnName("Name");
                a.Property(t => t.LanguageId).HasColumnName("LanguageId");
                a.HasOne(t => t.Language);
            });

            modelBuilder.Entity<User>(a =>
            {
                a.ToTable("Users").HasKey(k => k.Id);
                a.Property(u => u.Id).HasColumnName("Id");
                a.Property(u => u.Email).HasColumnName("Email");
                a.Property(u => u.FirstName).HasColumnName("FirstName");
                a.Property(u => u.LastName).HasColumnName("LastName");
                a.Property(u => u.PasswordHash).HasColumnName("PasswordHash");
                a.Property(u => u.PasswordSalt).HasColumnName("PasswordSalt");
                a.Property(u => u.AuthenticatorType).HasColumnName("AuthenticatorType");
                a.Property(u => u.Status).HasColumnName("Status");
                a.HasMany(u => u.UserOperationClaims);
                a.HasMany(u => u.RefreshTokens);
            });

            modelBuilder.Entity<UserOperationClaim>(a =>
            {
                a.ToTable("UserOperationClaims").HasKey(k => k.Id);
                a.Property(u => u.Id).HasColumnName("Id");
                a.Property(u => u.UserId).HasColumnName("UserId");
                a.Property(u => u.OperationClaimId).HasColumnName("OperationClaimId");
                a.HasOne(u => u.User);
                a.HasOne(u => u.OperationClaim);
            });

            modelBuilder.Entity<OperationClaim>(a =>
            {
                a.ToTable("OperationClaims").HasKey(k => k.Id);
                a.Property(o => o.Id).HasColumnName("Id");
                a.Property(o => o.Name).HasColumnName("Name");

            });
            
            modelBuilder.Entity<OtpAuthenticator>(a =>
            {
                a.ToTable("OtpAuthenticators").HasKey(k => k.Id);
                a.Property(o => o.Id).HasColumnName("Id");
                a.Property(r => r.UserId).HasColumnName("UserId");
                a.Property(o => o.SecretKey).HasColumnName("SecretKey");
                a.Property(o => o.IsVerified).HasColumnName("IsVerified");
                a.HasOne(o => o.User);
            });
            
            modelBuilder.Entity<EmailAuthenticator>(a =>
            {
                a.ToTable("EmailAuthenticators").HasKey(k => k.Id);
                a.Property(o => o.Id).HasColumnName("Id");
                a.Property(r => r.UserId).HasColumnName("UserId");
                a.Property(o => o.ActivationKey).HasColumnName("ActivationKey");
                a.Property(o => o.IsVerified).HasColumnName("IsVerified");
                a.HasOne(o => o.User);
            });
            
            modelBuilder.Entity<RefreshToken>(a =>
            {
                a.ToTable("OperationClaims").HasKey(k => k.Id);
                a.Property(r => r.Id).HasColumnName("Id");
                a.Property(r => r.UserId).HasColumnName("UserId");
                a.Property(r => r.Token).HasColumnName("Token");
                a.Property(r => r.Expires).HasColumnName("Expires");
                a.Property(r => r.Created).HasColumnName("Created");
                a.Property(r => r.CreatedByIp).HasColumnName("CreatedByIp");
                a.Property(r => r.Revoked).HasColumnName("Revoked");
                a.Property(r => r.RevokedByIp).HasColumnName("RevokedByIp");
                a.Property(r => r.ReplacedByToken).HasColumnName("ReplacedByToken");
                a.Property(r => r.ReasonRevoked).HasColumnName("ReasonRevoked");
                a.HasOne(r => r.User);
            });


            Language[] languageEntitySeeds = { new(1, "Java"), new(2, "JavaScript") };
            modelBuilder.Entity<Language>().HasData(languageEntitySeeds);

            Technology[] technologyEntitySeeds = {new(1, 1, "Spring"), new(2, 2, "React")};
            modelBuilder.Entity<Technology>().HasData(technologyEntitySeeds);
        }

    }
}
