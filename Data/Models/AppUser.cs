using System.ComponentModel.DataAnnotations;
public class AppUser
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
    public string? ImagePath { get; set; }

    public string? AdminUserId { get; set; }

    public AdminUser? AdminUser { get; set; }
    public List<WebList>? WebLists { get; set; }
}