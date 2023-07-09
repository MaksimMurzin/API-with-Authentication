using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.Data.SqlClient;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool isUniqueUser(string username)
        {
            string queryString = $"select * from Users where UserName = '{username}'";
            List<LocalUser> users = new List<LocalUser>();
            using SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand sqlCommand = new SqlCommand(queryString, connection);

            sqlCommand.Connection.Open();

            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var user = new LocalUser()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    UserName = (string)reader["Username"],
                    Name = (string)reader["Name"],
                    Password = (string)reader["Password"],
                    Role = (string)reader["Role"]
                };

                users.Add(user);
            }

            sqlCommand.Connection.Close();

            if((users.Count == 0) || (users.Count > 1) ) 
            { 
                return false; 
            }
            return true;

        }

        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequest)
        {
            var queryString = $"insert into Users (UserName, Password) values({registrationRequest.UserName}, {registrationRequest.Password})";

            var connection = new SqlConnection(_connectionString);
            SqlCommand sqlCommand = new SqlCommand(queryString, connection);
            sqlCommand.Connection.Open();
            await sqlCommand.ExecuteNonQueryAsync();
            sqlCommand.Connection.Close();

            // I am only doing this part to follow the tutorial, this is a strange idea generally
            var user = new LocalUser()
            {
                UserName = registrationRequest.UserName,
                Password = ""
            };

            return user;
        }
    }
}
