using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using LastDanceAPI.Entities;
using LastDanceAPI.DTO;

namespace LastDanceAPI.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly string _connectionString;

        public OrdersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString")
                ?? throw new ArgumentNullException("Connection string is not configured.");
        }


        public async Task<IEnumerable<Orders>> GetAllOrdersNotDeletedAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = "SELECT * FROM ld.tOrders WHERE ordIsDeleted = 0";

            var orders = await connection.QueryAsync<Orders>(sql);

            return orders;
        }

        public async Task<Orders?> SaveAsync(Orders orders)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();

            parameters.Add("@ordID",
                orders.ordID == 0 ? null : orders.ordID,
                DbType.Int32,
                ParameterDirection.InputOutput
            );
            parameters.Add("@ordClientID", orders.ordClientID);
            parameters.Add("@ordName", orders.ordName);
            parameters.Add("@ordStatus", orders.ordStatus);
            parameters.Add("@ordTotalAmount", orders.ordTotalAmount);
            parameters.Add("@ordDescription", orders.ordDescription);
            parameters.Add("@ordDelivered", orders.ordDelivered);
            

            await connection.ExecuteAsync(
                "ld.spOrderSave",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            orders.ordID = parameters.Get<int?>("@ordID") ?? 0;
            return orders;
        }

        public async Task<bool> SoftDeleteAsync(OrdersDeleteDto dto)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@ordID", dto.ordID);
            var result = await connection.ExecuteAsync(
                "ld.spOrderDelete",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return result > 0;

        }

        public async Task<Orders?> UpdateAsync(Orders order)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();

            parameters.Add("@ordID", order.ordID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@ordClientID", order.ordClientID);
            parameters.Add("@ordName", order.ordName);
            parameters.Add("@ordStatus", order.ordStatus);
            parameters.Add("@ordTotalAmount", order.ordTotalAmount);
            parameters.Add("@ordDescription", order.ordDescription);
            parameters.Add("@ordDelivered", order.ordDelivered);

            await connection.ExecuteAsync(
                "ld.spOrderSave",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            order.ordID = parameters.Get<int>("@ordID");
            return order;
        }
    }
}
