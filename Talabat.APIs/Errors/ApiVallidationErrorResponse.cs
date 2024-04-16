namespace Talabat.APIs.Errors
{
	public class ApiVallidationErrorResponse : ApiResponse
	{
        public IEnumerable<string> Errors { get; set; }

        public ApiVallidationErrorResponse()
            :base(400)
        {
            Errors = new List<string>();
        }
    }
}
