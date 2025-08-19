using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using LastDanceAPI.Entities;
using LastDanceAPI.DTO;

namespace LastDanceAPI.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly string _connectionString;

        public ClientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString")
                ?? throw new ArgumentNullException("Connection string is not configured.");
        }

        public async Task<IEnumerable<Clients>> GetAllClientsNotDeletedAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = "SELECT * FROM ld.tClients WHERE cltIsDeleted = 0";

            var clients = await connection.QueryAsync<Clients>(sql);

            return clients;
        }

        public async Task<Clients?> SaveAsync(Clients client)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();

            parameters.Add("@cltID",
                client.cltID == 0 ? null : client.cltID,
                DbType.Int32,
                ParameterDirection.InputOutput
            );
            parameters.Add("@cltName", client.cltName);
            parameters.Add("@cltSurname", client.cltSurname);
            parameters.Add("@cltEmail", client.cltEmail);
            parameters.Add("@cltPhoneNumber", client.cltPhoneNumber);
            parameters.Add("@cltGender", client.cltGender);
            parameters.Add("@cltActive", client.cltActive);
            parameters.Add("@cltStatus", client.cltStatus);

            await connection.ExecuteAsync(
                "ld.spClientSave",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            client.cltID = parameters.Get<int>("@cltID");

            return client;
        }


        public async Task<Clients?> UpdateAsync(Clients client)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();

            parameters.Add("@cltID", client.cltID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@cltName", client.cltName);
            parameters.Add("@cltSurname", client.cltSurname);
            parameters.Add("@cltEmail", client.cltEmail);
            parameters.Add("@cltPhoneNumber", client.cltPhoneNumber);
            parameters.Add("@cltGender", client.cltGender);
            parameters.Add("@cltActive", client.cltActive);
            parameters.Add("@cltStatus", client.cltStatus);

            await connection.ExecuteAsync(
                "ld.spClientSave",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            client.cltID = parameters.Get<int>("@cltID");

            return client;
        }

        public async Task<bool> SoftDeleteAsync(ClientDeleteDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@cltID", dto.cltID);

                var result = await connection.ExecuteAsync(
                    "ld.spClientDelete",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result > 0;
            }
        }
    }
}