using Shared.Models;

namespace ManagementApp.Services
{
    public interface ICompanyService
    {
        Task<Company?> GetCurrentCompanyAsync();
        Task<Company?> UpdateCurrentCompanyAsync(int id);
        Task<IEnumerable<Company>> GetAllAsync();
    }
}
