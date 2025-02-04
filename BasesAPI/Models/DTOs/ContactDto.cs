namespace BasesAPI.Models.DTOs
{
    public class ContactDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class ContactResponseDto : ContactDto
    {
        public int Id { get; set; }
    }

}
