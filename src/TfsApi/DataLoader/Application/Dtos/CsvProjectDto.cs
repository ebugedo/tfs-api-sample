namespace DataLoader.Application.Dtos
{
	public class CsvProjectDto
	{
		public string Client { get; set; }
		public string Description { get; set; }
        public int? Duration { get; set; }
        public string Name { get; set; }
		public string Sector { get; set; }
        public DateTime? StartDate { get; set; }
        public string Technologies { get; set; }

	}
}
