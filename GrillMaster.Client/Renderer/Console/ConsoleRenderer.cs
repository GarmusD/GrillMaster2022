using GrillMaster.Client.AppConfiguration;
using GrillMaster.Client.DTO;
using GrillMaster.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace GrillMaster.Client.Renderer.Console
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

        public void Render(OutputOptions outputOptions, string optimizedGrillJson)
        {
            _ = outputOptions;
            var optimizedOrder = ParseJson(optimizedGrillJson);
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

        private static OptimizedOrder ParseJson(string optimizedJson)
        {
            try
            {
                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                return JsonSerializer.Deserialize<OptimizedOrder>(optimizedJson, options)!;
            }
            catch (Exception ex) when (ex is JsonException or NotSupportedException)
            {
                throw new GrillMasterInvalidJsonException();
            }
        }

        private List<OptimizedItemEx> PrepareGrilledItems(List<OptimizedItem> optItems)
        {
            List<OptimizedItemEx> newItems = new ();
            foreach (var item in optItems)
            {
                newItems.Add( new OptimizedItemEx() { Item = item, FillChar = GetNextFillChar() } );
            }
            return newItems;
        }

        private void RenderGrilledPan(List<OptimizedItemEx> optItems)
        {
            PrintLine(RenderHorizontalTopBorder(_grillWidth));

            for (var line = 0; line < _grillHeight; line++)
                PrintLine(RenderScanLine(optItems, line));

            PrintLine(RenderHorizontalBottomBorder(_grillWidth));
        }

        private string RenderScanLine(List<OptimizedItemEx> optItems, int line)
        {
            StringBuilder builder = new(_grillWidth + 2);
            builder.Append(BorderCharVertical);
            for (var x = 0; x < _grillWidth; x++)
            {
                OptimizedItemEx? item = optItems.FirstOrDefault(i => i.Item.Location.Contains(x, line));
                if (item is null) builder.Append(' ');
                else builder.Append(item.FillChar);
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
