using Kreta.Shared.Models;
using Kreta.Shared.Responses;

namespace Kreata.Backend.Repos
{
    public interface IStudentRepo
    {
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(Guid id);
        Task<Response> UpdateStudentAsync(Student student);
        Task<Response> DeleteStudentAsync(Guid id);
        Task<Response> InsertStudentAsync(Student student);
    }
}
