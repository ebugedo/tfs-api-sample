using Domain.Common;
using System.Net;
namespace Tfs.Api.Domain.AggretateModels
{
    public class RequestTrace : EntityBase<int>
    {
        public DateTime DateAndTimeOfCreation { get; private set; }
        public DateTime? DateAndTimeOfLastModification { get; private set; }
        public string IPAddress { get; private set; }
		public int NumberOfRequests { get; private set; }
		public RequestTrace() { }
		public RequestTrace(
			string ipAddress
            ) 
		{
			DateAndTimeOfCreation = DateTime.UtcNow;
            IPAddress = string.IsNullOrWhiteSpace(ipAddress) ? throw new ArgumentNullException(nameof(ipAddress)) : ipAddress;
			NumberOfRequests = 1;
		}
		public RequestTrace(
            string ipAddress,
			int numberOfRequest
            ) 
		{
            DateAndTimeOfLastModification = DateTime.UtcNow;
            IPAddress = string.IsNullOrWhiteSpace(ipAddress) ? throw new ArgumentNullException(nameof(ipAddress)) : ipAddress;
			NumberOfRequests = numberOfRequest;
		}


        public void Update(string ipAddress,int numberOfRequest)
        {
            DateAndTimeOfLastModification = DateTime.UtcNow;
            IPAddress = string.IsNullOrWhiteSpace(ipAddress) ? throw new ArgumentNullException(nameof(ipAddress)) : ipAddress;
            NumberOfRequests = numberOfRequest;
        }
    }
}