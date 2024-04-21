using JWT_Authentication.Models;
using JWT_Authentication.Repositories;

namespace JWT_Authentication.Serives
{
    public class UserService
    {
        public User Get(UserLogin user)
        {
            User returnUser = UserRepository.users.FirstOrDefault(u => u.username.Equals(user.username, StringComparison.OrdinalIgnoreCase) && 
            u.password.Equals(user.password, StringComparison.OrdinalIgnoreCase));
            return returnUser;
        }
    }
}
