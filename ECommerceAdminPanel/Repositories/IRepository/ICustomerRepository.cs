using ECommerceAdminPanel.Models;

namespace ECommerceAdminPanel.Repositories.IRepository
{
    public interface ICustomerRepository
    {
        Task<int> CreateAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<List<Customer>> GetByTenantAsync(int tenantId, int pageNumber, int pageSize);

        // ICustomerRepository.cs mein add karo
        Task<Customer?> GetByEmailAsync(string email, int tenantId);

        Task<int> UpdateAsync(int id, Customer customer);
        Task<int> DeleteAsync(int id);
    }
}