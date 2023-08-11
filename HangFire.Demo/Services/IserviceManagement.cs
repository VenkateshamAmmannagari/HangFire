using HangFire.Demo.Models;
using System.Data;

namespace HangFire.Demo.Services
{
    public interface IserviceManagement
    {
        void SendEmail();
        void InsertRecords(Pilot pilot);

        DataTable SyncData();

        List<Pilot> GetAllRecords();
    }
}
