using API_Eventos.Models;
using API_Eventos.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace API_Eventos.Services
{
    public class UserService
    {
        private readonly ApiDataContext _context;

        public UserService(ApiDataContext context)
        {
            _context = context;
        }

        public async Task<Users> Authenticate(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null && VerifyPassword(password, user.Password))
            {
                // Autenticação bem-sucedida
                return user;
            }

            // Autenticação falhou
            return null;
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedInput = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hashedInput == hashedPassword;
            }
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hashedPassword;
            }
        }
    }
}
