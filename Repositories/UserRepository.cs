using JWT_Authentication.Models;

namespace JWT_Authentication.Repositories
{
    public class UserRepository
    {
        public static List<User> users = new()
        {
            new("yehor", "$2b$10$9l8bdt6m8b1l1xzqVUC4Hefd3vL0OFd8FnZILkj249xsFmE9jjCBu", "miggabit@gmail.com", "Yehor", "Malikov", "https://facebook.com/yehor", "admin"),
            new("tomek", "$2b$10$9l8bdt6m8b1l1xzqVUC4Hefd3vL0OFd8FnZILkj249xsFmE9jjCBu", "tomek@tomek.com", "Tomek", "Tomaszewicz", "https://facebook.com/yehor", "user")
        };

        // hashed password is "string"
    }
}
