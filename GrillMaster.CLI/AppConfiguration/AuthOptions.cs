namespace GrillMaster.CLI.AppConfiguration
{
    public class AuthOptions
    {
        public string UserName { get => _username; set => SetUserName(value); }
        public string Password { get => _password; set => SetPassword(value); }

        private string _username = ConfigDefaults.UserOptions.UserName;
        private string _password = ConfigDefaults.UserOptions.Password;

        private void SetUserName(string value)
        {
            if (string.IsNullOrEmpty(value) || value == ConfigDefaults.Placeholders.UserName) 
                value = ConfigDefaults.UserOptions.UserName;

            _username = value;
        }

        private void SetPassword(string value)
        { 
            if (string.IsNullOrEmpty(value) || value == ConfigDefaults.Placeholders.Password) 
                value = ConfigDefaults.UserOptions.Password;

            _password = value;
        }
    }
}
