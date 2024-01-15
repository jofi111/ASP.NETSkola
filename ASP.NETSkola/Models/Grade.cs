namespace ASP.NETSkola.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public Subject Subject { get; set; }
        public int Result { get; set; }
        public string Topic { get; set; }
        public DateTime Date { get; set; }
        public Student Student { get; set; }
    }
}
