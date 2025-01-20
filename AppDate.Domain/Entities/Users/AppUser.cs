namespace AppDate.Domain.Entities.Users
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}
