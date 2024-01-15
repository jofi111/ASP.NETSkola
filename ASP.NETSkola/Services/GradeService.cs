using ASP.NETSkola.Models;
using ASP.NETSkola.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETSkola.Services
{
    public class GradeService
    {
        private ApplicationDbContext dbContext;

        public GradeService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<GradesDropdownsVM> GetDropdownValuesAsync()
        {
            return new GradesDropdownsVM()
            {
                Students = await dbContext.Students.OrderBy(x=>x.LastName).ToListAsync(),
                Subjects = await dbContext.Subjects.ToListAsync(),
            };

        }
        internal async Task CreateAsync(GradeVM gradeModel)
        {
            var gradeToInsert = new Grade()
            {
                Student = dbContext.Students.FirstOrDefault(st => st.Id == gradeModel.StudentId),
                Subject = dbContext.Subjects.FirstOrDefault(sub => sub.Id == gradeModel.SubjectId),
                Date = DateTime.Today,
                Topic = gradeModel.Topic,
                Result = gradeModel.Result,
            };
            if (gradeToInsert.Student!=null&&gradeToInsert.Subject!=null) { 
            await dbContext.Grades.AddAsync(gradeToInsert);
            await dbContext.SaveChangesAsync();
            }
        }
        internal async Task<IEnumerable<Grade>> GetAllGrades()
        {
            return await dbContext.Grades.Include(gr=>gr.Student).Include(gr=>gr.Subject).ToListAsync();
        }

        internal GradeVM GetById(int id)
        {
            var gradeToEdit = dbContext.Grades.FirstOrDefault(gr=>gr.Id == id);
            //GradeVM gradeVM = new GradeVM();
            if (gradeToEdit != null)
            {
                //gradeVM.StudentId= gradeToEdit.Student.Id;
                //gradeVM.SubjectId= gradeToEdit.Subject.Id;
                //gradeVM.Id= gradeToEdit.Id;
                //gradeVM.Result = gradeToEdit.Result;
                //gradeVM.Date = gradeToEdit.Date;
                //gradeVM.Topic = gradeToEdit.Topic;
                return new GradeVM() 
                { 
                    StudentId = gradeToEdit.Student.Id,
                    SubjectId = gradeToEdit.Subject.Id,
                    Id = gradeToEdit.Id,
                    Result = gradeToEdit.Result,
                    Date = gradeToEdit.Date,
                    Topic = gradeToEdit.Topic
                };
            }
            //return gradeVM;
            return null;
        }

        internal async Task UpdateAsync(GradeVM updatedGrade)
        {
            var gradeToUpdate = dbContext.Grades.FirstOrDefault(gr=>gr.Id==updatedGrade.Id);
            if (gradeToUpdate!=null)
            {
                gradeToUpdate.Subject = dbContext.Subjects.FirstOrDefault(sub => sub.Id == updatedGrade.SubjectId);
                gradeToUpdate.Student = dbContext.Students.FirstOrDefault(st => st.Id == updatedGrade.StudentId);
                gradeToUpdate.Topic=updatedGrade.Topic;
                gradeToUpdate.Result= updatedGrade.Result;
                //gradeToUpdate.Date = updatedGrade.Date;
            }
            dbContext.Update(gradeToUpdate);
            await dbContext.SaveChangesAsync();
        }
        internal async Task DeleteAsync(int id)
        {
            var gradeToDelete = dbContext.Grades.FirstOrDefault(gr=>gr.Id == id);
            if (gradeToDelete!=null)
            {
                dbContext.Grades.Remove(gradeToDelete);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
