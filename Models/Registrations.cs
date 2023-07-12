using System.Reflection.Metadata.Ecma335;

namespace API_Eventos.Models
{
    public class Registrations
    {
        public int Id { get; set; }

        public int User_Id { get; set; }
        public Users Users { get; set; }

        public int Event_Id { get; set; }
        public Events Events { get; set; }

        public bool Payment_Status { get; set; } = false;
    }
}
