using Microsoft.AspNetCore.Identity;
public class AdminUser: IdentityUser
    {
        public string? Name { get; set;}
        
        public ICollection<AppUser>? AppUsers { get; set; }
        
    }