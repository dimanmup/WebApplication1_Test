using Newtonsoft.Json;

#nullable disable

namespace DAL
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        [JsonIgnore]
        public List<Role> Roles { get; set; } = new List<Role>();

        /// <summary>
        /// Время регистрации в UTC.
        /// </summary>
        public DateTime RegistrationDateTime { get; set; }
    }
}
