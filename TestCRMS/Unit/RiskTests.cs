using CorporateRiskManagementSystemBack.API.Controllers;
using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Application.Services;
using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace TestCRMS.Unit
{
    public class RiskTests
    {
        private readonly Mock<IRiskService> _riskServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly RiskController _controller;

        public RiskTests()
        {
            _riskServiceMock = new Mock<IRiskService>();
            _userServiceMock = new Mock<IUserService>();
            _controller = new RiskController(_riskServiceMock.Object, _userServiceMock.Object);
        }

        [Fact]
        public void GetAllRisks_ReturnsAllRisks()
        {
            // Arrange
            var mockRisks = new List<Risk> { new Risk(), new Risk() };
            _riskServiceMock.Setup(x => x.GetAllRisks()).Returns(mockRisks);

            var identity = new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Role, "Auditor")
        }, "TestAuth");

            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            // Act
            var result = _controller.GetAllRisks();

            // Assert
            result.Value.Should().BeEquivalentTo(mockRisks);
        }

        [Fact]
        public async Task CreateRisk_WithValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new CreateRiskRequest
            {
                UsernameId = "testUser",
                Title = "Risk title",
                Description = "desc",
                Likelihood = "3",
                Severity = "2",
                DepartmentId = 1
            };

            _userServiceMock.Setup(x => x.GetUserIdByName("testUser")).Returns(1);
            _riskServiceMock.Setup(x => x.CreateRisk(It.IsAny<Risk>())).Returns(10);
            _riskServiceMock.Setup(x => x.LinkRiskToDepartment(10, 1)).Returns(1);

            // Act
            var result = await _controller.CreateRisk(request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(new { message = "Risk created successfully" });
        }

        [Fact]
        public async Task CreateRisk_NullRequest_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.CreateRisk(null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetRisksForDepartment_ReturnsRisks()
        {
            // Arrange
            var departmentId = 5;
            var risks = new List<Risk> { new Risk { Title = "R1" } };
            _riskServiceMock.Setup(x => x.GetRisksForDepartment(departmentId)).Returns(risks);

            // Act
            var result = await _controller.GetRisksForDepartment(departmentId);

            // Assert
            result.Value.Should().BeEquivalentTo(risks);
        }

        [Fact]
        public async Task GetAssessmentForRisk_ReturnsAssessment()
        {
            // Arrange
            var assessment = new RiskAssessment { AssessmentId = 1 };
            _riskServiceMock.Setup(x => x.GetAssessmentForRisk(2)).Returns(assessment);

            // Act
            var result = await _controller.GetAssessmentForRisk(2);

            // Assert
            result.Value.Should().BeEquivalentTo(assessment);
        }

        [Fact]
        public async Task AddAssessments_WithValidRequest_ReturnsSuccessMessage()
        {
            // Arrange
            var request = new RiskAssessmentRequest
            {
                UsernameId = "admin",
                RiskId = 7,
                AssessmentDate = DateTime.Today,
                ImpactScore = 4,
                ProbabilityScore = 3,
                Notes = "some"
            };

            _userServiceMock.Setup(x => x.GetUserIdByName("admin")).Returns(999);

            // Act
            var result = await _controller.AddAssessments(request);

            // Assert
            var ok = result as OkObjectResult;
            ok!.Value.Should().BeEquivalentTo(new { message = "ќценка успешно добавлена" });
        }

    }
}
