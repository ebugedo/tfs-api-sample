namespace Tfs.Api.Application.Dtos.RequestTrace
{
    public class RequestTraceGetDto : RequestTraceBaseDto
	{
        public DateTime DateAndTimeOfCreation { get; set; }
        public DateTime? DateAndTimeOfLastModification { get; set; }
		public int Id { get; set; }
		public int NumberOfRequests { get; set; }
	}
}
