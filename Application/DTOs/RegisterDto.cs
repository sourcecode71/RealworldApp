namespace Application.DTOs
{
    public class RegisterDto
    {
        public RegisterDto()
        {
            this.Name = string.Format("{0} {1}", this.FirstName, this.LastName);
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
    }
}