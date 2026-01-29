using ClassLibraryDatabaseConnection;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AIRLINEMANAGEMENTSYSTEM.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly string _connectionString;

        public LoginRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<(bool isSuccess, int administratorId)>
            LoginAsync(string username, string password)
        {
            int administratorId = 0;
            bool isSuccess = false;

            using SqlConnection conn = ConnectionManager.OpenConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_AdminLogin", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();


            if (await reader.ReadAsync())
            {
                administratorId = reader.GetInt32(0);
                isSuccess = true;
            }

            return (isSuccess, administratorId);
        }
    }
}
