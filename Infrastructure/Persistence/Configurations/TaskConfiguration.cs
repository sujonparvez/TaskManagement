using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.Task>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(m => m.Title).IsRequired().HasMaxLength(500);

            builder.HasOne(m => m.AssignedUser).WithMany(m => m.AssignedTasks).HasForeignKey(m => m.AssignedUserId).IsRequired(false);

            builder.HasOne(m => m.CreatedByUser).WithMany(m => m.CretaedTasks).HasForeignKey(m => m.CreatedByUserId).IsRequired(true);

            builder.HasOne(m => m.Team).WithMany(m => m.Tasks).HasForeignKey(m => m.TeamId).IsRequired(true);
        }
    }
}
