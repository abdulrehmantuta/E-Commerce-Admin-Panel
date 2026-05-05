using Dapper;
using ECommerceAdminPanel.Models;
using ECommerceAdminPanel.Repositories.IRepository;
using System.Data;

namespace ECommerceAdminPanel.Repositories.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection _db;

        public CustomerRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> CreateAsync(Customer customer)
        {
            var sql = @"EXEC sp_Customer_Create 
                        @TenantId,
                        @FirstName,
                        @LastName,
                        @Email,
                        @Password,
                        @Status";

            return await _db.QuerySingleAsync<int>(sql, new
            {
                customer.TenantId,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.Password,
                customer.Status
            });
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Customers WHERE CustomerId = @id";
            return await _db.QueryFirstOrDefaultAsync<Customer>(sql, new { id });
        }

        //new

        public async Task<Customer?> GetByEmailAsync(string email, int tenantId)
        {
            var sql = @"SELECT * FROM Customers 
                WHERE Email = @Email 
                AND TenantId = @TenantId";
            return await _db.QueryFirstOrDefaultAsync<Customer>(sql, new { Email = email, TenantId = tenantId });
        }

        public async Task<List<Customer>> GetByTenantAsync(int tenantId, int pageNumber, int pageSize)
        {
            var sql = @"SELECT * FROM Customers 
                        WHERE TenantId = @tenantId
                        ORDER BY CustomerId DESC
                        OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

            return (await _db.QueryAsync<Customer>(sql, new
            {
                tenantId,
                offset = (pageNumber - 1) * pageSize,
                pageSize
            })).ToList();
        }

        public async Task<int> UpdateAsync(int id, Customer customer)
        {
            var sql = @"UPDATE Customers 
                        SET FirstName = @FirstName,
                            LastName = @LastName,
                            Email = @Email,
                            Password = @Password,
                            Status = @Status
                        WHERE CustomerId = @CustomerId";

            return await _db.ExecuteAsync(sql, new
            {
                CustomerId = id,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.Password,
                customer.Status
            });
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Customers WHERE CustomerId = @id";
            return await _db.ExecuteAsync(sql, new { id });
        }
    }
}