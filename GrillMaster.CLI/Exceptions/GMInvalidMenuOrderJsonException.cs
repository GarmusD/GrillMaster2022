namespace GrillMaster.CLI.Exceptions
{
    internal class GMInvalidMenuOrderJsonException : GMBaseException
    {
        public GMInvalidMenuOrderJsonException() : base("Unsupported MenuOrder json format.")
        {
            ExitCode = ExitCodes.InvalidMenuOrderJson;
        }
    }
}
