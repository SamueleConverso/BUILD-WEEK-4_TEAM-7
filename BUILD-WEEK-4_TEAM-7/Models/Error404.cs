namespace BUILD_WEEK_4_TEAM_7.Models
{
    public class Error404
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
