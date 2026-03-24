using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tfs.Api.Domain.AggretateModels;
namespace Tfs.Api.Infrastructure.EntityConfigurations
{
	internal class ClientEntityConfiguration : IEntityTypeConfiguration<Client>
	{
		public void Configure(EntityTypeBuilder<Client> builder)
		{
			builder.ToTable("Clients");
			// Primary key
			builder.HasKey(c => c.Id).HasName("PK_Clients");
			// Unique Keys
			builder
				.HasIndex(c => new { c.Name}, "UK_Clients_Name")
				.IsUnique();
			// Foreign Keys
			// Columns
			builder.Property(c => c.Name).IsRequired().HasMaxLength(128);
			builder.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
		}
	}
}
