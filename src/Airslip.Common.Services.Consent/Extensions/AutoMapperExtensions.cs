using Airslip.Common.Matching.Data;
using Airslip.Common.Services.Consent.Entities;
using Airslip.Common.Services.Consent.Implementations;
using Airslip.Common.Types;
using AutoMapper;

namespace Airslip.Common.Services.Consent.Extensions
{
    public static class AutoMapperExtensions
    {
        public static void AddTransactionMapperConfiguration(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression
                .CreateMap<IncomingTransactionModel, Transaction>()
                .ForPath(d => d.Amount, c => c
                    .MapFrom(s => Currency.ConvertToUnit(s.Amount)))
                .ForPath(d => d.Description, c => c
                    .MapFrom(s => s.Description))
                .ForPath(d => d.CurrencyCode, c => c
                    .MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.MatchType, c => c
                    .MapFrom(src => MatchTypes.Yapily))
                .ForPath(d => d.UserId, c => c
                    .MapFrom(s => s.UserId))
                .ForPath(d => d.AuthorisedTimeStamp, c => c
                    .MapFrom(s => s.AuthorisedDate))
                .ForPath(d => d.CapturedTimeStamp, c => c
                    .MapFrom(s => s.CapturedDate))
                .ForPath(d => d.Id, c => c
                    .MapFrom(s => s.Id))
                .ForPath(d => d.DataSource, c => c
                    .MapFrom(s =>s.DataSource))
                .ForPath(d => d.Merchant, c => c.Ignore())
                .ForPath(d => d.Metadata, c => c.Ignore())
                .ReverseMap();

            mapperConfigurationExpression
                .CreateMap<IncomingTransactionModel, TransactionBank>()
                .ForPath(d => d.AccountId, c => c
                    .MapFrom(s => s.AccountId))
                .ForPath(d => d.BankIcon, c => c
                    .Ignore())
                .ForPath(d => d.BankName, c => c
                    .Ignore())
                .ForPath(d => d.BankTransactionId, c => c
                    .MapFrom(s => s.BankTransactionId))
                .ReverseMap();

            mapperConfigurationExpression
                .CreateMap<IncomingTransactionModel, TransactionMerchant>()
                .ForPath(d => d.EntityId, c => c
                    .MapFrom(s => s.EntityId))
                .ReverseMap();
        }
    }
}