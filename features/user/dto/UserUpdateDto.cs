 
namespace backend_ont_2.features.user.dto
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Department { get; set; }
        public bool? IsActive { get; set; }
        public string? Position { get; set; }
        public string? ProfileImage { get; set; }
    }
}