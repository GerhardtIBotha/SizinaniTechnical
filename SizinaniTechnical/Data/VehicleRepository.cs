using System.Data;
using Microsoft.Data.SqlClient;
using SizinaniTechnical.Model;


namespace SizinaniTechnical.Data
{
    public class VehicleRepository
    {
        private readonly string _cs;

        public VehicleRepository(IConfiguration config)
        {
            _cs = config.GetConnectionString("Default")!;
        }

        public void SaveRegisteredVehicle(RegisteredVehicle vehicle)
        {
            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand("insert into dbo.RegisteredVehicle values (@Type, @RegistrationNr, @Modal, @Weight)", conn);
            cmd.Parameters.AddWithValue("@Type", vehicle.Type);
            cmd.Parameters.AddWithValue("@RegistrationNr", vehicle.RegistrationNr);
            cmd.Parameters.AddWithValue("@Modal", vehicle.Model);
            cmd.Parameters.AddWithValue("@Weight", vehicle.Weight);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public List<Contractor> GetContractors()
        {
            var list = new List<Contractor>();
            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand("dbo.usp_GetContractors", conn)
            { CommandType = CommandType.StoredProcedure };
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                list.Add(new Contractor
                {
                    ContractorID = rdr.GetInt32(0),
                    ContractorFirstName = rdr.GetString(1)
                });
            return list;
        }

        public List<RegisteredVehicle> GetVehiclesByContractor(int contractorId)
        {
            var list = new List<RegisteredVehicle>();
            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand("dbo.usp_GetVehiclesByContractor", conn)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add("@ContractorId", SqlDbType.Int).Value = contractorId;
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                list.Add(new RegisteredVehicle
                {
                    Type = rdr.GetString(rdr.GetOrdinal("Type")),
                    RegistrationNr = rdr.GetString(rdr.GetOrdinal("RegistrationNr")),
                    Model = rdr.GetString(rdr.GetOrdinal("Modal")),
                    Weight = rdr.GetDecimal(rdr.GetOrdinal("Weight"))
                });
            return list;
        }
    }
}
