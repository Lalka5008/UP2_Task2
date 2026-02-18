using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Data;
using WpfApp2.Models;




namespace WpfApp2.Data;

internal class AuthService
{
    public Role TryAuth(string login, string password)
    {
        using (var context = new Task4Context())
        {
            var user = context.Users
                .Include(u => u.Role) // ← явно подгружаем роль
                .FirstOrDefault(u => u.Login == login && u.Password == password);

            return user?.Role; // ← безопасно
        }
    }
}