using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.AutoMapper;
using Airslip.Common.Services.AutoMapper.Extensions;
using Airslip.Common.Services.Consent.Extensions;
using Airslip.Common.Services.Consent.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Transactions;
using Xunit;

namespace Airslip.Common.Services.Consent.UnitTests
{
    public class MapperTests
    {
        [Fact]
        public void Can_map_from_transaction()
        {
            ConfigurationBuilder builder = new();
            IServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddTransactionMapperConfiguration();
                cfg.AddConsentMapperConfiguration();
            }, MapperUsageType.Service);

            ServiceProvider provider = services.BuildServiceProvider();

            using IServiceScope scope = provider.CreateScope();

            IModelMapper<IncomingTransactionModel> mapper = scope.ServiceProvider
                .GetService<IModelMapper<IncomingTransactionModel>>() ?? throw new NotImplementedException();


            IncomingTransactionModel model = new()
            {

            };


        }
    }
}