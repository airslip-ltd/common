namespace Airslip.Common.Contracts
{
    public class Success : ISuccess, IResponse
    {
        public static Success Instance = new Success();

        private Success()
        {
        }
    }
}
