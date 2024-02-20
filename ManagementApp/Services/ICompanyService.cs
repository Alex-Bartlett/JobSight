using Shared.Models;

namespace ManagementApp.Services
{
    public interface ICompanyService
    {
        Company? GetCurrentCompany();
        Task<Company?> ChangeCompany(int id);
    }
}
