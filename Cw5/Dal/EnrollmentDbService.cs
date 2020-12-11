namespace Cw5.Dal
{
    public class EnrollmentDbService : IEnrollmentDbService
    {
        private readonly IConfig _config;

        public EnrollmentDbService(IConfig config)
        {
            _config = config;
        }

    }
}
