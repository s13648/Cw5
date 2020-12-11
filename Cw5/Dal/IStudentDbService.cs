using System.Collections.Generic;
using System.Threading.Tasks;
using Cw5.Models;

namespace Cw5.Dal
{
    public interface IStudentDbService
    {
        public Task<IEnumerable<Student>> GetStudents();
    }
}
