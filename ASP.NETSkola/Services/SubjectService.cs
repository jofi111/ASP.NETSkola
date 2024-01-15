using ASP.NETSkola.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETSkola.Services
{
    public class SubjectService
    {
        private ApplicationDbContext dbContext;

        public SubjectService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Subject>> GetAllAsync() { 
        return await dbContext.Subjects.ToListAsync();
        }
        public async Task CreateSubjectAsync(Subject subject)
        {
            await dbContext.Subjects.AddAsync(subject);
            await dbContext.SaveChangesAsync();
        }

        internal async Task<Subject> GetById(int id)
        {
            return await dbContext.Subjects.FirstOrDefaultAsync(st => st.Id == id);
        }

        internal async Task UpdateAsync(Subject subject)
        {
            dbContext.Subjects.Update(subject);
            await dbContext.SaveChangesAsync();
        }

        internal async Task DeleteSubjectAsync(Subject subjectToDelete)
        {
            dbContext.Subjects.Remove(subjectToDelete);
            await dbContext.SaveChangesAsync();
        }
    }
}
