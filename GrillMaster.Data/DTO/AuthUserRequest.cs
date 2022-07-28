namespace GrillMaster.Data.DTO
{
    public class AuthUserRequest
    {
        public string Username { get; init; }
        public string Password { get; init; }

        public AuthUserRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
