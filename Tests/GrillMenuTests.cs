using GrillMaster.Data.DTO;
using GrillMaster.Data.Primitives;
using System.Text.Json;

namespace GrillMaster2022.Tests
{
    public class GrillMenuTests
    {
        const string JsonGrillMenu = @"{""grillSize"":{""width"":86,""height"":111},""menuItems"":[{""dimensions"":{""width"":3,""height"":4},""name"":""Tomato Slice"",""count"":11},{""dimensions"":{""width"":4,""height"":4},""name"":""Bread Slice"",""count"":12},{""dimensions"":{""width"":1,""height"":6},""name"":""Chickpea Sausage with Cheese"",""count"":15},{""dimensions"":{""width"":15,""height"":14},""name"":""Smoked Beef Steak"",""count"":13},{""dimensions"":{""width"":14,""height"":11},""name"":""Spicy Salmon Steak"",""count"":11},{""dimensions"":{""width"":1,""height"":14},""name"":""Smoked Spider Sausage with Cheese"",""count"":7},{""dimensions"":{""width"":1,""height"":15},""name"":""Mutton Sausage with Cheese"",""count"":9},{""dimensions"":{""width"":1,""height"":12},""name"":""Walnut Sausage with Lard Pieces"",""count"":10},{""dimensions"":{""width"":15,""height"":19},""name"":""Savory Pork Steak"",""count"":13},{""dimensions"":{""width"":1,""height"":8},""name"":""Savory Walnut Sausage with Paprika"",""count"":6},{""dimensions"":{""width"":1,""height"":5},""name"":""Mutton Sausage"",""count"":12},{""dimensions"":{""width"":5,""height"":6},""name"":""Potato Slice"",""count"":14},{""dimensions"":{""width"":17,""height"":20},""name"":""Savory Beef Steak"",""count"":10},{""dimensions"":{""width"":4,""height"":5},""name"":""Onion Slice"",""count"":10}]}";
        const string JsonSize = @"{""width"":15,""height"":25}";
        GrillOrder? _grillOrder = null;

        [SetUp]
        public void Setup()
        {

        }

        [Test, Order(0)]
        public void ParseJsonGrillOrderTest()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            Size size = JsonSerializer.Deserialize<Size>(JsonSize, options);
            Assert.That(size.IsEmpty, Is.False);
            _grillOrder = JsonSerializer.Deserialize<GrillOrder>(JsonGrillMenu, options);
            Assert.That(_grillOrder, Is.Not.Null);
        }

        [Test]
        public void GrillSizeIsNotEmptyTest()
        {
            Assert.That(_grillOrder?.GrillSize.IsEmpty, Is.False);
        }

        [Test]
        public void MenuItemsNotZeroTest()
        {
            Assert.That(_grillOrder?.MenuItems.Count, Is.GreaterThan(0));
        }

        [Test]
        public void MenuItemSizeNotEmpty()
        {
            Assert.That(_grillOrder?.MenuItems[0].Dimensions.IsEmpty, Is.False);
        }
    }
}