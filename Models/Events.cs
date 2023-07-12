namespace API_Eventos.Models

{
    public class Events
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date {get; set; }
        public string Location { get; set; }

        public int Cartegory_Id { get; set; }
        public Categories Categories { get; set; }

        public double Price { get; set; }
        public string Images { get; set; }
    }
}
