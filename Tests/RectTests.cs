using GrillMaster2022.GrillOptimizer.Types;

namespace GrillMaster2022.Tests
{
    internal class RectTests
    {
        const int MainRectWidth = 50;
        const int MainRectHeight = 100;
        private Rect mainRect;

        [SetUp]
        public void Setup()
        {
            mainRect = new Rect(0, 0, MainRectWidth, MainRectHeight);
        }

        [Test]
        public void EmptyRectTest()
        {
            Rect rect = new Rect(0, 0);
            Assert.That(rect.IsEmpty, Is.True);
        }

        [Test]
        public void PartialIntersectTests()
        {
            Rect topLeft = new Rect(-15, -15, 25, 25);
            Rect bottomRight = new Rect(MainRectWidth - 15, MainRectHeight - 15, 25, 25);
            Assert.Multiple(() => {
                Assert.That(mainRect.IntersectsWith(topLeft), Is.True);
                Assert.That(mainRect.IntersectsWith(bottomRight), Is.True);
            });
        }

        [Test]
        public void FullIntersectTests()
        {
            Rect rect = new Rect(5, 5, 25, 25);
            Assert.That(mainRect.IntersectsWith(rect), Is.True);
        }

        [Test]
        public void NotIntersectedTests()
        {
            Rect topLeft = new Rect(-35, -35, 25, 25);
            Rect bottomRight = new Rect(MainRectWidth + 15, MainRectHeight + 15, 25, 25);
            Assert.Multiple(() => {
                Assert.That(mainRect.IntersectsWith(topLeft), Is.False);
                Assert.That(mainRect.IntersectsWith(bottomRight), Is.False);
            });
        }

        [Test]
        public void AreaIsFitting()
        {
            Size area = new ((int)(MainRectWidth * 0.9), (int)(MainRectHeight * 0.9));
            bool result = mainRect.CanFit(area);
            Assert.That(result, Is.True);
        }

        [Test]
        public void AreaIsFittingRotated()
        {
            Size area = new ((int)(MainRectHeight * 0.9), (int)(MainRectWidth * 0.9));
            bool result = mainRect.CanFitRotated(area);
            Assert.That(result, Is.True);
        }

        [Test]
        public void AreaIsNotFitting()
        {
            Size area = new ((int)(MainRectWidth * 1.1), (int)(MainRectHeight * 0.9));
            bool result = mainRect.CanFit(area);
            Assert.That(result, Is.False);
        }

        [Test]
        public void AreaIsNotFittingRotated()
        {
            Size area = new ((int)(MainRectHeight * 1.1), (int)(MainRectWidth * 0.9));
            bool result = mainRect.CanFitRotated(area);
            Assert.That(result, Is.False);
        }

        [Test]
        public void SubtractFromTopLeftTest()
        {
            const int TestRectW = 25;
            const int TestRectH = 25;
            Rect testRect = new(0, 0, TestRectW, TestRectH);
            List<Rect> result = mainRect.Subtract(testRect);
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));

                Rect rect = result[0];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(TestRectW));
                Assert.That(rect.Top, Is.EqualTo(0));
                Assert.That(rect.Width, Is.EqualTo(MainRectWidth - TestRectW));
                Assert.That(rect.Height, Is.EqualTo(MainRectHeight));


                rect = result[1];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(0));
                Assert.That(rect.Top, Is.EqualTo(TestRectH));
                Assert.That(rect.Width, Is.EqualTo(MainRectWidth));
                Assert.That(rect.Height, Is.EqualTo(MainRectHeight - TestRectH));
            });
        }

        [Test]
        public void SubtractFromTopRightTest()
        {
            const int TestRectW = 25;
            const int TestRectH = 25;
            Rect testRect = new(MainRectWidth - TestRectW, 0, TestRectW, TestRectH);
            List<Rect> result = mainRect.Subtract(testRect);
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));

                Rect rect = result[0];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(0));
                Assert.That(rect.Top, Is.EqualTo(0));
                Assert.That(rect.Width, Is.EqualTo(MainRectWidth - TestRectW));
                Assert.That(rect.Height, Is.EqualTo(MainRectHeight));


                rect = result[1];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(0));
                Assert.That(rect.Top, Is.EqualTo(TestRectH));
                Assert.That(rect.Width, Is.EqualTo(MainRectWidth));
                Assert.That(rect.Height, Is.EqualTo(MainRectHeight - TestRectH));
            });
        }

        [Test]
        public void SubtractFromLeftBottomTest()
        {
            const int TestRectW = 25;
            const int TestRectH = 25;
            Rect testRect = new(0, MainRectHeight - TestRectH, TestRectW, TestRectH);
            List<Rect> result = mainRect.Subtract(testRect);
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));

                Rect rect = result[0];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(0));
                Assert.That(rect.Top, Is.EqualTo(0));
                Assert.That(rect.Width, Is.EqualTo(MainRectWidth));
                Assert.That(rect.Height, Is.EqualTo(MainRectHeight - TestRectH));


                rect = result[1];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(TestRectW));
                Assert.That(rect.Top, Is.EqualTo(0));
                Assert.That(rect.Width, Is.EqualTo(MainRectWidth - TestRectW));
                Assert.That(rect.Height, Is.EqualTo(MainRectHeight));
            });
        }

        [Test]
        public void SubtractFromRightBottomTest()
        {
            const int TestRectW = 25;
            const int TestRectH = 25;
            Rect testRect = new(MainRectWidth - TestRectW, MainRectHeight - TestRectH, TestRectW, TestRectH);
            List<Rect> result = mainRect.Subtract(testRect);
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));

                Rect rect = result[0];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(0));
                Assert.That(rect.Top, Is.EqualTo(0));
                Assert.That(rect.Width, Is.EqualTo(MainRectWidth));
                Assert.That(rect.Height, Is.EqualTo(MainRectHeight - TestRectH));


                rect = result[1];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(0));
                Assert.That(rect.Top, Is.EqualTo(0));
                Assert.That(rect.Width, Is.EqualTo(MainRectWidth - TestRectW));
                Assert.That(rect.Height, Is.EqualTo(MainRectHeight));
            });
        }

        [Test]
        public void SubtractFromMiddleTest()
        {
            const int TestRectW = 25;
            const int TestRectH = 25;
            const int TestRectLeft = (int)((MainRectWidth / 2) - (TestRectW / 2));
            const int TestRectTop = (int)((MainRectHeight / 2) - (TestRectH / 2));

            Rect testRect = new(TestRectLeft, TestRectTop, TestRectW, TestRectH);
            List<Rect> result = mainRect.Subtract(testRect);
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(4));

                //Top
                Rect rect = result[0];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(0));
                Assert.That(rect.Top, Is.EqualTo(0));
                Assert.That(rect.Width, Is.EqualTo(MainRectWidth));
                Assert.That(rect.Height, Is.EqualTo(testRect.Top));

                //Left
                rect = result[1];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(0));
                Assert.That(rect.Top, Is.EqualTo(0));
                Assert.That(rect.Width, Is.EqualTo(testRect.Left));
                Assert.That(rect.Height, Is.EqualTo(MainRectHeight));

                //Right
                rect = result[2];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(testRect.Right));
                Assert.That(rect.Top, Is.EqualTo(0));
                Assert.That(rect.Width, Is.EqualTo(MainRectWidth - testRect.Right));
                Assert.That(rect.Height, Is.EqualTo(MainRectHeight));

                //Bottom
                rect = result[3];
                Assert.That(rect.IsEmpty, Is.False);
                Assert.That(rect.Left, Is.EqualTo(0));
                Assert.That(rect.Top, Is.EqualTo(testRect.Bottom));
                Assert.That(rect.Width, Is.EqualTo(MainRectWidth));
                Assert.That(rect.Height, Is.EqualTo(MainRectHeight - testRect.Bottom));
            });
        }

        
    }
}
