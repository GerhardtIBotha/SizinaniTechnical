using System.Data;
using Microsoft.Data.SqlClient;
using SizinaniTechnical.Model;


namespace SizinaniTechnical.Data
{
    public class ContractorRepository
    {
        private readonly string _cs;

        public ContractorRepository(IConfiguration config)
        {
            _cs = config.GetConnectionString("Default")!;
        }

        public void SaveContractor(Contractor contractor)
        {
            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand("insert into dbo.Contractor values (@ContractorFirstName, @ContractorLastName, @ContractorEmail, @ContractorPhone)", conn);
            cmd.Parameters.AddWithValue("@ContractorFirstName", contractor.ContractorFirstName);
            cmd.Parameters.AddWithValue("@ContractorLastName", contractor.ContractorLastName);
            cmd.Parameters.AddWithValue("@ContractorEmail", contractor.ContractorEmail);
            cmd.Parameters.AddWithValue("@ContractorPhone", contractor.ContractorPhone);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
