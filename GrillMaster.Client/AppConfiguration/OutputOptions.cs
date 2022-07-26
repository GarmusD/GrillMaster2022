namespace GrillMaster.Client.AppConfiguration
{
    public class OutputOptions
    {
        public OutputTarget Output { get; set; } = OutputTarget.Console;
        public FileInfo? JsonOutputPath { get; set; } = null;
    }
}
