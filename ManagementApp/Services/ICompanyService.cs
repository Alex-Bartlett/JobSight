using Shared.Models;

namespace ManagementApp.Services
{
    public interface ICompanyService
    {
        Company? Company { get; }
        Task<Company?> SetCompany(int id);
    }
}
