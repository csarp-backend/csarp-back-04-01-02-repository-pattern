using Kreata.Backend.Repos;
using Kreta.Shared.Dtos;
using Kreta.Shared.Extensions;
using Kreta.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Kreata.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private IStudentRepo _studentRepo;

        public StudentController(IStudentRepo? studentRepo)
        {
            _studentRepo = studentRepo ?? throw new ArgumentNullException(nameof(studentRepo));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBy(Guid id)
        {
            var entity = (await _studentRepo.FindByConditionAsync(s => s.Id == id)).FirstOrDefault();
            if (entity != null)
                return Ok(entity.ToStudentDto());
            else
                return NotFound();
            
        }

        [HttpGet]
        public async Task<IActionResult> SelectAllRecordToListAsync()
        {
            var users = await _studentRepo.GetAllAsync();
            return Ok(users.Select(student => student.ToStudentDto()));
        }

        [HttpPut()]
        public async Task<ActionResult> UpdateStudentAsync(StudentDto entity)
        {
            Response response = new();
            response = await _studentRepo.UpdateAsync(entity.ToStudent());
            if (response.HasError)
            {
                Console.WriteLine(response.Error);
                response.ClearAndAddError("A diák adatainak módosítása nem sikerült!");
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudendAsync(Guid id)
        {
            Response response = new();
            response = await _studentRepo.DeleteAsync(id);

            if (response.HasError)
            {
                Console.WriteLine(response.Error);
                response.ClearAndAddError("A diák adatainak törlése nem sikerült!");
                return BadRequest(response);
            }
            return Ok(response);           
        }

        [HttpPost()]
        public async Task<IActionResult> CreateStudentAsync(StudentDto student)
        {
            Response response = new();
            response = await _studentRepo.CreateAsync(student.ToStudent());
            if (response.HasError)
            {
                Console.WriteLine(response.Error);
                response.ClearAndAddError("Az új adatok mentése nem lehetséges!");
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
