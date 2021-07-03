namespace Airslip.Common.Contracts
{
    public class Success : ISuccess
    {
        public static readonly Success Instance = new();

        private Success()
        {
        }
    }
}
