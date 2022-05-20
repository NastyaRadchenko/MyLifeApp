using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLifeClient.Models
{
    public class MainUser
    {
        static Guid Id { get; set; }
        static string Name { get; set; }
        static bool Auth { get; set; }
        static UserRole Role { get; set; }
        public enum UserRole { User, Administrator, None }

        public static void NewMain(User user)
        {
            Id = user.Id;
            Auth = true;
            Name = user.Name;
            Role = GetRole().Result;
        }

        public static void Quit()
        {
            Auth = false;
            Role = UserRole.None;
            Name = "None";
        }

        static async Task<UserRole> GetRole()
        {
            var httpClient = new HttpClient();
            var responce = await httpClient.GetAsync($"https://localhost:5001/api/users/{Id}/role");
            if (!responce.IsSuccessStatusCode) return UserRole.None;
            if (responce.Content.ReadAsStringAsync().Result.Equals("Administrator")) return UserRole.Administrator;
            else return UserRole.User;
        }

        public static bool IsAuthorized()
        {
            if (Auth == false) return false;
            else return true;
        }

        public static bool IsInRole(UserRole role)
        {
            if (Role.Equals(role)) return true;
            else return false;
        }

        public static string GetName() { return Name; }
    }
}
