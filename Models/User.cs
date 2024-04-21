namespace JWT_Authentication.Models
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string facebook { get; set; }
        public string role { get; set; }

        public User(string username, string password, string email, string firstName, string lastName, string facebook, string role)
        {
            this.username = username;
            this.password = password;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.facebook = facebook;
            this.role = role;
        }
    }
}
