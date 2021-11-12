using HomeTreatment.Business.BusinesObjects;
using HomeTreatment.Data;
using HomeTreatment.Data.Models;
using System.Linq;

namespace HomeTreatment.Business.Services
{
    public interface IUsersService
    {
        void SetMessageAsRead(string Id);

        void AddMessage(User user, string text);

        bool IsDoctor(string doctorId);
        IQueryable<User> GetUser();

        User GetPatient(string userId);

        LoggedInUserBO GetLogginUser(string userId);

        PatientDashboardBO BuildPatiantDashboardViewModel(Patient patient);

    }
}
