using JWT_Authentication.Models;

namespace JWT_Authentication.Repositories
{
    public class UserRepository
    {
        public static List<User> users = new()
        {
            new("yehor", "123", "miggabit@gmail.com", "Yehor", "Malikov", "https://facebook.com/yehor", "admin"),
            new("tomek", "haslo", "tomek@tomek.com", "Tomek", "Tomaszewicz", "https://facebook.com/yehor", "user")
        };
    }
}
