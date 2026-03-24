
using Domain.AggregateModels;
using Domain.AggregateModels.Projects;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tfs.Api.Domain.AggretateModels;

namespace Tfs.Api.Infrastructure.DbContexts
{
    public class TfsApiDbContext : DbContext
	{
		public TfsApiDbContext(DbContextOptions<TfsApiDbContext> options) : base(options)
		{
		}

		public DbSet<Client> Client { get; set; }
		public DbSet<Project> Project { get; set; }
		public DbSet<ProjectTechnology> ProjectTechnology { get; set; }
		public DbSet<RequestTrace> RequestTrace { get; set; }
		public DbSet<Sector> Sector { get; set; }
		public DbSet<Technology> Technology { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlServer("TfsApiConnection");
		//}
	}
}