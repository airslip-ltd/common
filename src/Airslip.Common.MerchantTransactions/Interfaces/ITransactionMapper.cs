using Airslip.Common.MerchantTransactions.Generated;

namespace Airslip.Common.MerchantTransactions.Interfaces;

public interface ITransactionMapper<in TSource> 
    where TSource : class
{
    TransactionDetails Create(TSource transaction);
}