using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkBase_9
{
    public static class UserService
    {
        private static List<User> _users = new List<User>();
        private static int _nextId = 4;
        static UserService()
        {
            _users.Add(new User
            {
                Id = 1,
                Login = "admin",
                Password = "admin123",
                FullName = "Администратор Системы",
                Role = "Администратор",
                IsBlocked = false,
                FailedAttempts = 0
            });
            _users.Add(new User
            {
                Id = 2,
                Login = "user",
                Password = "user123",
                FullName = "Иванов Иван Иванович",
                Role = "Пользователь",
                IsBlocked = false,
                FailedAttempts = 0
            });
        }
        public static User Authenticate(string login, string password)
        {
            return _users.FirstOrDefault(u => u.Login == login && u.Password == password && !u.IsBlocked);
        }

        public static User GetUserByLogin(string login)
        {
            return _users.FirstOrDefault(u => u.Login == login);
        }

        public static void IncrementFailedAttempts(string login)
        {
            var user = GetUserByLogin(login);
            if (user != null)
            {
                user.FailedAttempts++;
                if (user.FailedAttempts >= 3)
                {
                    user.IsBlocked = true;
                }
            }
        }

        public static void ResetFailedAttempts(string login)
        {
            var user = GetUserByLogin(login);
            if (user != null)
            {
                user.FailedAttempts = 0;
                user.LastLoginDate = DateTime.Now;
            }
        }

        public static List<User> GetAllUsers()
        {
            return _users.OrderBy(u => u.Id).ToList();
        }

        public static bool AddUser(string login, string password, string fullName, string role)
        {
            if (_users.Any(u => u.Login == login))
                return false;

            _users.Add(new User
            {
                Id = _nextId++,
                Login = login,
                Password = password,
                FullName = fullName,
                Role = role,
                IsBlocked = false,
                FailedAttempts = 0
            });
            return true;
        }

        public static bool UpdateUser(int id, string login, string password, string fullName, string role, bool isBlocked)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;

            if (_users.Any(u => u.Login == login && u.Id != id))
                return false;

            user.Login = login;
            if (!string.IsNullOrWhiteSpace(password))
                user.Password = password;
            user.FullName = fullName;
            user.Role = role;
            user.IsBlocked = isBlocked;

            return true;
        }

        public static void UnblockUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.IsBlocked = false;
                user.FailedAttempts = 0;
            }
        }

        public static bool DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null && user.Login != "admin")
                return _users.Remove(user);
            return false;
        }
    }
}

