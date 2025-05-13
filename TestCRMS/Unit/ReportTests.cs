using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using CorporateRiskManagementSystemBack.API.Controllers;
using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack.Infrastructure.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using CorporateRiskManagementSystemBack.Application.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace TestCRMS.Unit
{
    public class ReportTests
    {
        private readonly Mock<IRiskService> _riskServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly ReportController _controller;

        public ReportTests()
        {
            _riskServiceMock = new Mock<IRiskService>();
            _userServiceMock = new Mock<IUserService>();
            var dbMock = new Mock<RiskDbContext>();
            _controller = new ReportController(dbMock.Object, _riskServiceMock.Object, _userServiceMock.Object);
        }
         
        [Fact]
        public async Task CreateReport_ReturnsBadRequest_WhenUserNotFound()
        {
            var request = new CreateReportRequest
            {
                Username = "ghost",
                DepartmentId = 1,
                Content = "Контент"
            };

            _riskServiceMock.Setup(x => x.GetRisksForDepartment(1)).Returns(new List<Risk>
            {
                new Risk { RiskAssessments = new List<RiskAssessment> { new RiskAssessment() } }
            });

            _userServiceMock.Setup(x => x.GetUserIdByName("ghost")).Returns(0);

            var result = await _controller.CreateReport(request);

            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequest = result as BadRequestObjectResult;
            badRequest!.Value.Should().Be("Пользователь с авторизоавнным юзернейном не найден");
        }

        [Fact]
        public async Task CreateReport_ReturnsBadRequest_WhenContentEmpty()
        {
            var request = new CreateReportRequest
            {
                Username = "admin",
                DepartmentId = 1,
                Content = string.Empty
            };

            _riskServiceMock.Setup(x => x.GetRisksForDepartment(1)).Returns(new List<Risk>
            {
                new Risk { RiskAssessments = new List<RiskAssessment> { new RiskAssessment() } }
            });

            _userServiceMock.Setup(x => x.GetUserIdByName("admin")).Returns(42);

            var result = await _controller.CreateReport(request);

            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequest = result as BadRequestObjectResult;
            badRequest!.Value.Should().Be("Content cannot be empty.");
        }

        [Fact]
        public async Task CanReportBuild_ReturnsTrue_WhenAllRisksHaveAssessment()
        {
            _riskServiceMock.Setup(x => x.GetRisksForDepartment(1)).Returns(new List<Risk>
            {
                new Risk { RiskAssessments = new List<RiskAssessment> { new RiskAssessment() } },
                new Risk { RiskAssessments = new List<RiskAssessment> { new RiskAssessment() } }
            });

            var result = await _controller.CanReportBuild(1);

            result.Value.Should().Be(true);
        }

        [Fact]
        public async Task CanReportBuild_ReturnsFalse_WhenAnyRiskHasNoAssessment()
        {
            _riskServiceMock.Setup(x => x.GetRisksForDepartment(1)).Returns(new List<Risk>
            {
                new Risk { RiskAssessments = new List<RiskAssessment>() },
                new Risk { RiskAssessments = new List<RiskAssessment> { new RiskAssessment() } }
            });

            var result = await _controller.CanReportBuild(1);

            result.Value.Should().Be(false);
        }
    }
}
