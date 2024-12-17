using Kreata.Backend.Context;
using Kreta.Shared.Models;
using Kreta.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Kreata.Backend.Repos
{
    public class StudentRepo : IStudentRepo
    {
        private readonly KretaInMemoryContext _dbContext;

        public StudentRepo(KretaInMemoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<Response> UpdateStudentAsync(Student student)
        {
            Response response = new Response();
            _dbContext.ChangeTracker.Clear();
            _dbContext.Entry(student).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.AppendNewError(e.Message);
                response.AppendNewError($"{nameof(StudentRepo)} osztály, {nameof(UpdateStudentAsync)} metódusban hiba keletkezett");
                response.AppendNewError($"{student} frissítése nem sikerült!");
            }
            return response;
        }

        public async Task<Response> DeleteStudentAsync(Guid id)
        {
            Response response = new Response();
            Student? studentToDelete = await GetByIdAsync(id);
            if (studentToDelete == null || studentToDelete == default)
            {
                response.AppendNewError($"{id} idével rendelkező diák nem található!");
                response.AppendNewError("A diák törlése nem sikerült!");
            }
            else
            {
                _dbContext.ChangeTracker.Clear();
                _dbContext.Entry(studentToDelete).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
            }
            return response;
        }

        public async Task<Response> InsertStudentAsync(Student student)
        {
            if (student.HasId)
            {
                return await UpdateStudentAsync(student);
            }
            else
            {
                return await InsertNewItemAsync(student);
            }
        }

        private async Task<Response> InsertNewItemAsync(Student student)
        {
            Response response = new Response();
            try
            {
                _dbContext.Students.Add(student);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.AppendNewError(e.Message);
                response.AppendNewError($"{nameof(StudentRepo)} osztály, {nameof(InsertNewItemAsync)} metódusban hiba keletkezett");
                response.AppendNewError($"{student} osztály hozzáadása az adatbázishoz nem sikerült!");
            }
            return response;
        }
    }
}
