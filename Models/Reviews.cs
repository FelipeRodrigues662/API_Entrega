namespace API_Eventos.Models
{
    public class Reviews
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public Users Users { get; set; }

        public int Event_Id { get; set; }
        public Events Events { get; set; }

        public int Score { get; set; }
        public string Comment { get; set; }
    }
}
