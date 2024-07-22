using System.ComponentModel.DataAnnotations;

public class WebList
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Password { get; set; }

    public string? Note { get; set; }

    public Guid UserId { get; set; }
    public AppUser? AppUser { get; set; }
}