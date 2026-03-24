using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tfs.Api.Domain.AggretateModels;
namespace Tfs.Api.Infrastructure.EntityConfigurations
{
	internal class SectorEntityConfiguration : IEntityTypeConfiguration<Sector>
	{
		public void Configure(EntityTypeBuilder<Sector> builder)
		{
			builder.ToTable("Sectors");
			// Primary key
			builder.HasKey(c => c.Id).HasName("PK_Sectors");
			// Keys
			builder
				.HasIndex(c => new { c.Name }, "UK_Sectors_Name")
				.IsUnique();
			// Foreign Keys
			// Columns
			builder.Property(c => c.Name).IsRequired().HasMaxLength(128);
			builder.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
		}
	}
}
