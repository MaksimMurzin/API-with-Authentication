using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private string secretKey;

        public UserRepository(string connectionString, IConfiguration configuration)
        {
            _connectionString = connectionString;
            secretKey = configuration.GetValue<string>("Jwt:Key");
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

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            var queryString = $"select * from Users where (Username = '{loginRequest.UserName}') and (Password = '{loginRequest.Password}')";
            var users = new List<LocalUser>();
            var connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(queryString,connection);
            command.Connection.Open();
            var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var user = new LocalUser()
                {
                    UserName = (string)reader["Username"],
                    Name = (string)reader["Name"],
                    Password = (string)reader["Password"],
                    Role = (string)reader["Role"]
                };

                users.Add(user);
            }
            command.Connection.Close();

            // if we have more than one user we can't login, if we don't find any users we can't login
            if((users.Count > 1) || (users.Count == 0) )  
            {
                throw new Exception("Invalid User");
            }

            var loginUser = users.First();
            //if the user was found generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
            };
        }


        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequest)
        {
            var queryString = $"insert into Users (UserName, Password, Name, Role) values({registrationRequest.UserName}, {registrationRequest.Password}, {registrationRequest.Name}, {registrationRequest.Role})";

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
