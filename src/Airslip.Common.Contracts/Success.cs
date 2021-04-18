namespace Airslip.Common.Contracts
{
    public class Success : ISuccess
    {
        public static Success Instance = new();

        private Success()
        {
        }
    }
}
