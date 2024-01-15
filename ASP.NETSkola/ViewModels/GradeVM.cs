using ASP.NETSkola.Models;

namespace ASP.NETSkola.ViewModels
{
    public class GradeVM
    {

        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int Result { get; set; }
        public string Topic { get; set; }
        public DateTime Date { get; set; }
        public int StudentId { get; set; }


    }
}
