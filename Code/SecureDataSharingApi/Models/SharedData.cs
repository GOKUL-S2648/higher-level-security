namespace SecureDataSharingApi.Models
{
    public class SharedData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string AccessLevel { get; set; } // "Admin" or "User"
    }
}