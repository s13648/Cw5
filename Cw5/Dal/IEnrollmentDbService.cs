using System.Threading.Tasks;
using Cw5.Dto;

namespace Cw5.Dal
{
    public interface IEnrollmentDbService
    {
        Task<Enrollment> EnrollStudent(EnrollStudent model, Study study);
    }
}
