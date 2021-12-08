using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.AutoMapper;
using Airslip.Common.Services.AutoMapper.Extensions;
using Airslip.Common.Services.Consent.Entities;
using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Services.Consent.Extensions;
using Airslip.Common.Services.Consent.Models;
using Airslip.Common.Types.Enums;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace Airslip.Common.Services.Consent.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Account_model_maps_as_expected()
        {
            ConfigurationBuilder builder = new();
            IConfigurationRoot configuration= builder.Build();
            IServiceCollection services = new ServiceCollection();
            services.AddConsentAuthorisation(configuration);
            
            services.AddAutoMapper(cfg =>
            {
                cfg.AddTransactionMapperConfiguration();
                cfg.AddConsentMapperConfiguration<TestMerchant>();
            }, MapperUsageType.Service);

            ServiceProvider provider = services.BuildServiceProvider();


            using IServiceScope scope = provider.CreateScope();
            
            IModelMapper<AccountModel> mappers = scope
                .ServiceProvider
                .GetService<IModelMapper<AccountModel>>() ?? throw new NotImplementedException();

            AccountModel model = new()
            {
                Id = "SomeId",
                AccountId = "AccountId",
                AccountNumber = "AccountNumber",
                AccountStatus = AccountStatus.Active,
                AccountType = "AccountType",
                CurrencyCode = "CurrencyCode",
                DataSource = DataSources.Yapily,
                EntityId = "EntityId",
                EntityStatus = EntityStatus.Active,
                InstitutionId = "InstitutionId",
                SortCode = "SortCode",
                TimeStamp = 1,
                UsageType = "UsageType",
                UserId = "UserId",
                AirslipUserType = AirslipUserType.Merchant,
                CreatedTimeStamp = 2,
                LastCardDigits = "LastCardDigits"
            };

            Account entity = mappers
                .Create<Account>(model);

            entity.Id.Should().Be(model.Id);
            entity.AccountId.Should().Be(model.AccountId);
            entity.AccountNumber.Should().Be(model.AccountNumber);
            entity.AccountStatus.Should().Be(model.AccountStatus);
            entity.AccountType.Should().Be(model.AccountType);
            entity.CurrencyCode.Should().Be(model.CurrencyCode);
            entity.DataSource.Should().Be(model.DataSource);
            entity.EntityId.Should().Be(model.EntityId);
            entity.EntityStatus.Should().Be(model.EntityStatus);
            entity.InstitutionId.Should().Be(model.InstitutionId);
            entity.SortCode.Should().Be(model.SortCode);
            entity.TimeStamp.Should().Be(model.TimeStamp);
            entity.UsageType.Should().Be(model.UsageType);
            entity.UserId.Should().Be(model.UserId);
            entity.AirslipUserType.Should().Be(model.AirslipUserType);
            // entity.CreatedTimeStamp = 2,
            entity.LastCardDigits.Should().Be(model.LastCardDigits);
            
            
        }
    }
}