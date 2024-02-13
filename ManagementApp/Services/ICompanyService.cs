using Shared.Models;

namespace ManagementApp.Services
{
    public interface ICompanyService
    {
        Company Company { get; }
        Company Set();
    }
}
