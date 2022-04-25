using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace DAL
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("ROLES");

            builder.Property(b => b.Id)
                .HasColumnName("ID");

            builder.Property(b => b.Name)
                .HasColumnName("NAME")
                .IsRequired(true)
                .HasMaxLength(20);

            builder.Property(b => b.Description)
                .HasColumnName("DESCRIPTION")
                .IsRequired(false)
                .HasMaxLength(1000);

            builder.HasIndex(b => b.Name)
                .IsUnique(true)
                .HasDatabaseName("IX__NAME");

            builder.HasMany(b => b.Users)
                .WithMany(b => b.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "USER_ROLE",
                    j => j.HasOne<User>()
                        .WithMany()
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasForeignKey("USER_ID"),
                    j => j.HasOne<Role>()
                        .WithMany()
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasForeignKey("ROLE_ID"),
                    j => j.HasKey("USER_ID", "ROLE_ID"));
        }
    }
}
