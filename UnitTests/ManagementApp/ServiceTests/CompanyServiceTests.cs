using ManagementApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Models;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace UnitTests.ManagementApp.ServiceTests
{
    public class CompanyServiceTests
    {
        private readonly CompanyService _sut;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock = new();
        private readonly Mock<ILogger<CompanyService>> _loggerMock = new();

        public CompanyServiceTests()
        {
            _sut = new CompanyService(_companyRepositoryMock.Object, _loggerMock.Object);
        }
    }
}
