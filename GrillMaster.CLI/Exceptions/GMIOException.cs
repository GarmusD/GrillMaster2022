namespace GrillMaster.CLI.Exceptions
{
    internal class GMIOException : GMBaseException
    {
        public GMIOException(string message) : base(message)
        {
            ExitCode = ExitCodes.IOException;
        }
    }
}
