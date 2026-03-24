using Domain.AggregateModels.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Tfs.Api.Infrastructure.EntityConfigurations
{
    internal class ProjectTechnologyEntityConfiguration : IEntityTypeConfiguration<ProjectTechnology>
	{
		public void Configure(EntityTypeBuilder<ProjectTechnology> builder)
		{
			builder.ToTable("ProjectsTechnologies");
			// Primary key
			builder.HasKey(c => c.Id).HasName("PK_ProjectsTechnologies");
			// Índices
			builder
				.HasIndex(c => new { c.ProjectId, c.TechnologyId }, "UK_ProjectsTechnologies_ProjectId_TechnologyId")
				.IsUnique();
			// Foreign Keys
			builder
				.HasOne(c => c.Project)
				.WithMany(c => c.ProjectTechnologies)
				.HasForeignKey(c => c.ProjectId)
				.HasConstraintName("FK_ProjectsTechnologies_Projects_ProjectId")
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
			builder
				.HasOne(c => c.Technology)
				.WithMany()
				.HasForeignKey(c => c.TechnologyId)
				.HasConstraintName("FK_ProjectsTechnologies_Technology_TechnologyId")
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			// Columns
			builder.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
			builder.Property(c => c.ProjectId).IsRequired();
			builder.Property(c => c.TechnologyId).IsRequired();
		}
	}
}
