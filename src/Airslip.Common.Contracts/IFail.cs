namespace Airslip.Common.Contracts
{
    public interface IFail : IResponse
    {
        public string ErrorCode { get; }
    }
}
