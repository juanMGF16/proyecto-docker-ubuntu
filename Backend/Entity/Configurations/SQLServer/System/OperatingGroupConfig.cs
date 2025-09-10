using Entity.Models.System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entity.Configurations.SQLServer.System
{
    public class OperatingGroupConfig : IEntityTypeConfiguration<OperatingGroup>
    {
        public void Configure(EntityTypeBuilder<OperatingGroup> builder)
        {
            builder.ToTable("OperatingGroup", DatabaseSchemas.System);

            builder.HasKey(og => og.Id);
            builder.Property(og => og.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(og => og.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(og => og.DateStart)
                .IsRequired();

            builder.Property(og => og.Active).HasColumnType("bit").HasDefaultValue(1).IsRequired();

            builder.HasOne(og => og.User)
                .WithOne(u => u.OperationalGroup)
                .HasForeignKey<OperatingGroup>(og => og.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(og => og.Operatings)
                .WithOne(o => o.OperationalGroup)
                .HasForeignKey(o => o.OperationalGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.CreatedAt).HasColumnType("datetime2(3)").IsRequired();
            builder.Property(x => x.UpdatedAt).HasColumnType("datetime2(3)");
        }
    }
}