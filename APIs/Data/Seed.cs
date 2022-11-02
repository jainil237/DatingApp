using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using APIs.Entities;
using Microsoft.EntityFrameworkCore;


namespace APIs.Data
{
    public class Seed{
        public static async Task SeedUsers(DataContext context)
        {

            {
                if (await context.User.AnyAsync()) return;
            }
            var userData = await System.IO.File.ReadAllTextAsync( "Data/UserSeedData.json");
             var users = JsonSerializer.Deserialize<List<AppUser>>(userData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
            foreach(var user in users){
                user.UserName = user.UserName.ToLower();
                using var hmac = new HMACSHA512();
                user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes("Password"));
                user.PasswordSalt=hmac.Key;

                context.User.Add(user);
            }

            await context.SaveChangesAsync();
        
        }


}



}