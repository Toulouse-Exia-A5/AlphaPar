using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaParWcfServiceLibrary.Models
{
    class UserToken
    {
        private static Random random = new Random();

        public string username { get; set; }
        public string token { get; set; }

        public UserToken(string username)
        {
            this.username = username;
            token = GenerateToken();
        }

        private string GenerateToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 36).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
