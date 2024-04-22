using BCrypt.Net;
using JWT_Authentication.Models;
using JWT_Authentication.Repositories;
using System.Text.RegularExpressions;

namespace JWT_Authentication.Serives
{
    public class UserService
    {
        // POST endpoint
        public User Post(User user)
        {
            // check whether this user already exists
            User checkUser = UserRepository.users.FirstOrDefault(u => u.username.Equals(user.username, StringComparison.OrdinalIgnoreCase));
            if (checkUser != null) throw new Exception("The user with the same username already exists");

            // encrypting user's password
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);

            // if user's facebook has incorrect format, throw an eception
            if (!CheckFacebook(user.facebook)) throw new Exception("Write correct facebook URI");

            // the same with email
            if (!CheckEmail(user.email)) throw new Exception("Write correct email");

            // prewriting standart role
            user.role = "user";

            UserRepository.users.Add(user);
            return user;
        }
        public User Get(UserLogin user)
        {
            // Validate the UserLogin object
            if (user == null || string.IsNullOrEmpty(user.username) || string.IsNullOrEmpty(user.password))
            {
                throw new ArgumentException("Invalid username or password");
            }

            // Find the user in the repository
            User returnUser = UserRepository.users.FirstOrDefault(u =>
                u.username.Equals(user.username, StringComparison.OrdinalIgnoreCase));

            if (returnUser == null)
            {
                throw new Exception("This user doesn't exist");
            }

            // Verify the password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.password, returnUser.password);

            // If password is not valid throw an exception
            if (!isPasswordValid)
            {
                throw new Exception("Password is not correct");
            }

            return returnUser;
        }

        // Regular expressions for fb and email
        public bool CheckFacebook(string uri)
        {
            string pattern = @"^(https?://)?(www\.)?(facebook\.com|fb\.com)/[a-zA-Z0-9.]+/?$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(uri);
        }

        public bool CheckEmail(string email)
        {
            string pattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
