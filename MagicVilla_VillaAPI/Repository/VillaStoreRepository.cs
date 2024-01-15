using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.Data.SqlClient;

namespace MagicVilla_VillaAPI.Repository
{

    public class VillaStoreRepository : IVillaStoreRepository
    {
        private readonly string _connectionString;
        public VillaStoreRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<Villa> GetVilla(int id)
        {
            var queryString = $"select * from Villas where Id = {id}";
            var villa = new Villa();
            using SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(queryString, connection);

            await command.Connection.OpenAsync();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                villa.Id = Convert.ToInt32(reader["Id"]);
                villa.Name = reader["Name"].ToString();
            }

            return villa;
        }

        public async Task<IEnumerable<Villa>> GetAllVillas()
        {
            var villas = new List<Villa>();
            var queryString = "select * from Villas";
            using SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            await command.Connection.OpenAsync();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var villa = new Villa()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                };

                villas.Add(villa);

            }

            return villas;
        }
    }
}
