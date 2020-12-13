using System.Threading.Tasks;
using Cw5.Dto;

namespace Cw5.Services
{
    public interface IStudyDbService
    {
        Task<Study> GetByName(string modelStudies);
    }
}
