public class UserDetails
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; }
}