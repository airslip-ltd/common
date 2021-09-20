using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Repository.Entities;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Repository.UnitTests
{
    public class RepositoryTests
    {
        [Fact]
        public async Task Error_is_not_thrown_when_unknown_resource_is_deleted()
        {
            Mock<IContext> mockContext = new();
            Mock<IModelValidator<TModel>> mockModelValidator = new();
            Mock<IModelMapper<TModel>> mockModelMapper = new();
            Mock< ITokenDecodeService<UserToken>> mockTokenDecodeService = new();

           Repository<TEntity, TModel> repo = new(
                mockContext.Object,
                mockModelValidator.Object,
                mockModelMapper.Object,
                mockTokenDecodeService.Object);

          RepositoryActionResultModel<TModel> delete = await repo.Delete("unknown-id");

          delete.Should().BeOfType<RepositoryActionResultModel<TModel>>();
          delete.ResultType.Should().Be(ResultType.NotFound);
        }
    }

    public class TEntity : IEntity
    {
        public string Id { get; set; } = string.Empty;
        public BasicAuditInformation? AuditInformation { get; set; }
        public EntityStatus EntityStatus { get; set; }
    }


    public class TTokenType : IDecodeToken
    {
        public void SetCustomClaims(List<Claim> tokenClaims)
        {
            throw new NotImplementedException();
        }

        public string TokenType { get; init; } = string.Empty;
        public bool? IsAuthenticated { get; init; }
        public string CorrelationId { get; init; }= string.Empty;
        public string IpAddress { get; init; }= string.Empty;
        public string BearerToken { get; init; }= string.Empty;
        public string UserAgent { get; init; }= string.Empty;
        public string EntityId { get; init; }= string.Empty;
        public AirslipUserType AirslipUserType { get; init; }
        public string Environment { get; init; }= string.Empty;
    }

    public class TModel : IModel
    {
        public string? Id { get; set; }
        public EntityStatus EntityStatus { get; set; }
    }
}