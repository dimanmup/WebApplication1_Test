using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USERS");

            builder.Property(b => b.Id)
                .HasColumnName("ID");

            builder.Property(b => b.Email)
                .HasColumnName("EMAIL")
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(b => b.EmailConfirmed)
                .HasColumnName("EMAIL_CONFIRMED")
                .HasDefaultValue(false)
                .IsRequired(true);

            builder.Property(b => b.DisplayName)
                .HasColumnName("DISPLAY_NAME")
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(b => b.RegistrationDateTime)
                .HasColumnName("REG_UTC_DT")
                .IsRequired(true);

            builder.Property(b => b.Password)
                .HasColumnName("PASSWORD")
                .IsRequired(false)
                .HasMaxLength(1000);

            builder.HasIndex(b => b.Email)
                .IsUnique(true)
                .HasDatabaseName("IX__EMAIL");

            builder.HasIndex(b => b.DisplayName)
                .IsUnique(true)
                .HasDatabaseName("IX__DISPLAY_NAME");
        }
    }
}
