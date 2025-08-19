using Dapper;
using LastDanceAPI.DTO;
using Microsoft.Data.SqlClient;

namespace LastDanceAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private readonly string _keyColumn;

        public GenericRepository(IConfiguration configuration, string tableName, string keyColumn)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString")
                ?? throw new ArgumentNullException("Connection string is not configured.");
            _tableName = tableName;
            _keyColumn = keyColumn;
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            var query = $"SELECT * FROM {_tableName}";

            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<T>(query);
        }

        public async Task<T?> GetByID(int id)
        {
            var query = $"SELECT * FROM {_tableName} WHERE {_keyColumn} = @Id";

            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
        }

        public async Task<T?> Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var properties = typeof(T).GetProperties();

            var columns = string.Join(", ", properties.Select(p => p.Name));

            var parameters = string.Join(", ", properties.Select(p => "@" + p.Name));

            var query = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters}); SELECT SCOPE_IDENTITY();";

            using var connection = new SqlConnection(_connectionString);

            var id = await connection.ExecuteScalarAsync<int>(query, item);

            return await GetByID(id);
        }
    }
}