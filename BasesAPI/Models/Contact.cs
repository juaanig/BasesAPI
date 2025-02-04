namespace BasesAPI.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public int UserId { get; set; } // Relación con el usuario
        public User User { get; set; } // Propiedad de navegación
    }

}
