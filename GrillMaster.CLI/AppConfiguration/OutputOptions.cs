namespace GrillMaster.CLI.AppConfiguration
{
    public class OutputOptions
    {
        public OutputTarget Output { get; set; } = OutputTarget.Console;
        public FileInfo? JsonOutputPath { get; set; } = null;
    }
}
