namespace AutoScales.Shared.Constants
{
    public class RoleConstants
    {
        public const string User = "User";
        public const string Admin = "Admin";
        private string[] roles;

        public RoleConstants()
        {
            roles = new string[] { User, Admin };
        }

        public string[] Roles => roles;
    }
}
