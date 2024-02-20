using Shared.Models;

namespace ManagementApp.Services
{
    public interface ICompanyService
    {
        Company? CurrentCompany { get; }
        Task<Company?> ChangeCompany(int id);
    }
}
