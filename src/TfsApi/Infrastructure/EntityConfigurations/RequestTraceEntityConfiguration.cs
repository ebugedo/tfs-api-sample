using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tfs.Api.Domain.AggretateModels;
namespace Tfs.Api.Infrastructure.EntityConfigurations
{
	internal class RequestTraceEntityConfiguration : IEntityTypeConfiguration<RequestTrace>
	{
		public void Configure(EntityTypeBuilder<RequestTrace> builder)
		{
			builder.ToTable("RequestTrace");
			// Primary key
			builder.HasKey(c => c.Id).HasName("PK_RequestTrace");
			// Unique Keys
			builder
				.HasIndex(c => new { c.IPAddress}, "UK_RequestTrace_IPAddress")
				.IsUnique();
			// Foreign Keys
			// Columns
			builder.Property(c => c.IPAddress).IsRequired().HasMaxLength(128);
			builder.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
		}
	}
}
