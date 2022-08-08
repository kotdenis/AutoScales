namespace AutoScales.Shared.Models
{
    public class CurrentUser
    {
        public bool IsAuthenticated { get; init; }
        public string UserName { get; init; } = string.Empty;
        public Dictionary<string, string> UserClaims { get; init; } = new Dictionary<string, string>();
    }
}
