namespace Airslip.Common.Repository.Exception
{
    public class RepositoryLifecycleException : System.Exception
    {
        public string ErrorCode { get; }
        public bool CanContinue { get; }

        public RepositoryLifecycleException(string ErrorCode, bool CanContinue)
        {
            this.ErrorCode = ErrorCode;
            this.CanContinue = CanContinue;
        }
    }
}