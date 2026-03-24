using Domain.AggregateModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Tfs.Api.Infrastructure.EntityConfigurations
{
	internal class TechnologyEntityConfiguration : IEntityTypeConfiguration<Technology>
	{
		public void Configure(EntityTypeBuilder<Technology> builder)
		{
			builder.ToTable("Technologies");
			// Primary key
			builder.HasKey(c => c.Id).HasName("PK_Technologies");
			// Índices
			builder
				.HasIndex(c => new { c.Name }, "UK_Technologies_Name")
				.IsUnique();
			// Foreign Keys
			// Columns
			builder.Property(c => c.Description).IsRequired(false);
			builder.Property(c => c.Name).IsRequired().HasMaxLength(128);
			builder.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
		}
	}
}
