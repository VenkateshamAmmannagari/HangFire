using Hangfire.Logging;
using HangFire.Demo.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HangFire.Demo.Services
{
    public class SeviceManagement : IserviceManagement
    {
        private readonly IConfiguration _configuration;
        public SeviceManagement(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail()
        {
            Console.WriteLine($"SendEmail :Sending email is in process..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public DataTable SyncData()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection con = new SqlConnection(connection);
            var query = "SELECT * FROM PILOT";
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            adapter.Fill(ds);

            Console.WriteLine($"SyncData :sync is going on..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            return ds.Tables[0];
        }

        public void InsertRecords(Pilot pilot)
        {
            try
            {
                string connection = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
                var query = "INSERT INTO PILOT (Name,FlyingExperience,Status,CreatedDate) VALUES(@Name,@FlyingExperience,@Status,@CreatedDate)";
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Name", pilot.Name);
                    cmd.Parameters.AddWithValue("FlyingExperience", pilot.FlyingExperience);
                    cmd.Parameters.AddWithValue("Status", pilot.Status);
                    cmd.Parameters.AddWithValue("CreatedDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
            }
            Console.WriteLine($"UpdatedDatabase :Updating the database is in process..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public List<Pilot> GetAllRecords()
        {
            DataTable pilots = SyncData();
            return (from DataRow dr in pilots.Rows
                    select new Pilot()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        FlyingExperience = Convert.ToInt32(dr["FlyingExperience"]),
                        Status = Convert.ToInt32(dr["Status"])
                    }).ToList();


        }
    }
}
