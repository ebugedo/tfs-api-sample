using Domain.AggregateModels.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tfs.Api.Infrastructure.EntityConfigurations
{
    internal class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
	{
		public void Configure(EntityTypeBuilder<Project> builder)
		{
			builder.ToTable("Projects");
			// Primary key
			builder.HasKey(c => c.Id).HasName("PK_Projects");
			// Índices			
			builder
				.HasIndex(c => new { c.Name }, "UK_Projects_Name")
				.IsUnique();
			// Foreign Keys
			builder
				.HasOne(c => c.Client)
				.WithMany()
				.HasForeignKey(c => c.ClientId)
				.HasConstraintName("FK_Projects_Clients_ClientId")
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
			builder
				.HasOne(c => c.Sector)
				.WithMany()
				.HasForeignKey(c => c.SectorId)
				.HasConstraintName("FK_Projects_Sectors_SectorId")
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
			//builder
			//	.HasMany(c => c.Technologies)
			//	.WithOne(c=>c.Project)
			//	.HasForeignKey(c => c.Id)
			//	//.HasConstraintName("FK_Projects_Technology_TechnologyId")
			//	.IsRequired()
			//	.OnDelete(DeleteBehavior.Restrict);

			// Columns
			builder.Property(c => c.ClientId).IsRequired();
			builder.Property(c => c.Name).IsRequired().HasMaxLength(128);
			builder.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
		}
	}
}
