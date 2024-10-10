using Domain.Courses;
using Domain.CoursesUsers;
using Domain.Faculties;
using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CourseUserConfiguration : IEntityTypeConfiguration<CourseUser>
{
    public void Configure(EntityTypeBuilder<CourseUser> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new CourseUserId(x))
            .IsRequired();

        builder.Property(x => x.CourseId)
            .HasConversion(x => x.Value, x => new CourseId(x))
            .IsRequired();
        builder.HasOne(x => x.Course)
            .WithMany()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.Course).AutoInclude();
        
        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new UserId(x))
            .IsRequired();
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Raiting)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.JoinAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired();

        builder.Property(x => x.EndAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
    }
}