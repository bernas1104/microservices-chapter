using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infra.Mappings
{
    public class UsersMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Email).IsUnique(true);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.FullName)
                .HasColumnName("fullName")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("createdAt")
                .HasColumnType("timestamp")
                .HasDefaultValue(DateTime.Now);

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updatedAt")
                .HasColumnType("timestamp")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValue(DateTime.Now);

            builder.Property(x => x.DeletedAt)
                .HasColumnName("deletedAt")
                .HasColumnType("timestamp")
                .IsRequired(false);
        }
    }
}
