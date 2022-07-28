using GrillMaster.CLI.AppConfiguration;
using GrillMaster.Data.DTO;
using System.Text;

namespace GrillMaster.CLI.Renderer.Console
{
    public class ConsoleRenderer : IRenderer
    {
        private const char BorderCharTopLeft = '╔';
        private const char BorderCharTopRight = '╗';
        private const char BorderCharBottomLeft = '╚';
        private const char BorderCharBottomRight = '╝';
        private const char BorderCharHorizontal = '═';
        private const char BorderCharVertical = '║';

        private int _grillWidth, _grillHeight;
        private readonly Random _random = new ();

        public void Render(OutputOptions outputOptions, OptimizedOrder optimizedOrder)
        {
            _ = outputOptions;
            if (optimizedOrder.GrilledPans == null) return;

            _grillWidth = optimizedOrder.GrillSize.Width;
            _grillHeight = optimizedOrder.GrillSize.Height;

            bool printSeparator = false;
            foreach (var optimizedPan in optimizedOrder.GrilledPans)
            {
                if (printSeparator) PrintSeparator();
                printSeparator = true;
                RenderGrilledPan(PrepareGrilledItems(optimizedPan));
            }
        }

        private List<OptimizedItemEx> PrepareGrilledItems(List<OptimizedItem> optItems)
        {
            return optItems
                .Select(item => 
                    new OptimizedItemEx() { Item = item, FillChar = GetNextFillChar() })
                .ToList();
        }

        private void RenderGrilledPan(List<OptimizedItemEx> optItems)
        {
            PrintLine(RenderHorizontalTopBorder(_grillWidth));

            foreach(var line in Enumerable.Range(0, _grillHeight)) 
                PrintLine(RenderScanLine(optItems, line));

            PrintLine(RenderHorizontalBottomBorder(_grillWidth));
        }

        private string RenderScanLine(List<OptimizedItemEx> optItems, int line)
        {
            StringBuilder builder = new(_grillWidth + 2);
            builder.Append(BorderCharVertical);

            foreach (var x in Enumerable.Range(0, _grillWidth))
            {
                OptimizedItemEx? item = optItems.FirstOrDefault(i => i.Item.Location.Contains(x, line));
                builder.Append(item is null ? ' ' : item.FillChar);
            }

            builder.Append(BorderCharVertical);
            return builder.ToString();
        }

        private static string RenderHorizontalTopBorder(int width)
        {
            return BorderCharTopLeft + "".PadLeft(width, BorderCharHorizontal) + BorderCharTopRight;
        }

        private static string RenderHorizontalBottomBorder(int width)
        {
            return BorderCharBottomLeft + "".PadLeft(width, BorderCharHorizontal) + BorderCharBottomRight;
        }

        private char GetNextFillChar()
        {
            const string CharList = "$%#*@!abcdefghijklmnopqrstuvwxyz1234567890?ABCDEFGHIJKLMNOPQRSTUVWXYZ&";
            return CharList[ _random.Next(0, CharList.Length-1) ];
        }

        private static void PrintSeparator()
        {
            PrintLine();
            PrintLine();
            PrintLine();
        }

        private static void PrintLine(string line = "")
        {
            System.Console.WriteLine(line);
        }
    }
}
