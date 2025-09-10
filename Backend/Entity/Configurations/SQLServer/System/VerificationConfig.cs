using Entity.Models.System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entity.Configurations.SQLServer.System
{
    public class VerificationConfig : IEntityTypeConfiguration<Verification>
    {
        public void Configure(EntityTypeBuilder<Verification> builder)
        {
            builder.ToTable("Verification", DatabaseSchemas.System);

            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(v => v.Result)
                .IsRequired();

            builder.Property(v => v.Date)
                .IsRequired();

            builder.Property(v => v.Observations)
                .HasMaxLength(500);

            builder.Property(v => v.Active).HasColumnType("bit").HasDefaultValue(1).IsRequired();

            builder.HasOne(v => v.Inventary)
                .WithMany(i => i.Verifications)
                .HasForeignKey(v => v.InventaryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.User)
                .WithMany(u => u.Verifications)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.CreatedAt).HasColumnType("datetime2(3)").IsRequired();
            builder.Property(x => x.UpdatedAt).HasColumnType("datetime2(3)");
        }
    }
}