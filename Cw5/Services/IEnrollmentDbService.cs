using System.Threading.Tasks;
using Cw5.Dto;

namespace Cw5.Dal
{
    public interface IEnrollmentDbService
    {
        Task<Enrollment> GetBy(string studies, int semester);

        Task<bool> Exists(string studies, int semester);

        Task<Enrollment> EnrollStudent(EnrollStudent model, Study study);
        Task Promotions(Promotions promotions);
    }
}
