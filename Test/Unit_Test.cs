using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Tetris_WPF;

namespace Tetris_Test
{
    [TestClass]
    public class Unit_Test
    {
        Mock<IPersistence> mock;
        Model _model;

        int SIZE = 4;
        int LENGTH = 16;

        [TestMethod]
        public void TestShape()
        {
            _model = new Model(null, SIZE);
            _model.AddShape();

            foreach (Coord coord in _model.Shapes[^1].Coordinates)
            {
                Assert.IsFalse(coord.OutOfBounds(new Coord(SIZE, LENGTH)));
            }
        }

        [TestMethod]
        public void TestRotate()
        {
            _model = new Model(null, SIZE);
            _model.AddShape();

            Position p = _model.Shapes[^1].Position;
            _model.Shapes[^1].Rotate();

            Assert.IsFalse(_model.Shapes[^1].Coordinates.Any<Coord>(coord => coord.OutOfBounds(new Coord(SIZE, LENGTH))));

        }

        [TestMethod]
        public void TestMove()
        {
            _model = new Model(null, SIZE);
            _model.AddShape();

            int border = _model.Shapes[^1].Coordinates.ToArray().Max<Coord>(a => a.X); //
            int x = _model.Shapes[^1].Coordinates[0].X;
            _model.Shapes[^1].Move(Direction.RIGHT);

            if (border == SIZE - 1)
            {
                Assert.AreEqual(x, _model.Shapes[^1].Coordinates[0].X);
            }
            else
            {
                Assert.AreEqual(x + 1, _model.Shapes[^1].Coordinates[0].X);
            }
        }

        [TestMethod]
        public void TestDrop()
        {
            _model = new Model(null, SIZE);
            _model.AddShape();

            int y = _model.Shapes[^1].Coordinates[0].Y;
            _model.Shapes[^1].Move(Direction.DOWN);

            Assert.AreEqual(y + 1, _model.Shapes[^1].Coordinates[0].Y);

            for (int i = 0; i < LENGTH+1; i++)
            {
                if (_model.Shapes.Count == 0 || _model.Shapes[^1].Coordinates.Count < 4) return; //
                _model.Shapes[^1].Move(Direction.DOWN);
            }

            if (_model.Shapes.Count == 0) return;
            Assert.IsFalse(LENGTH <= _model.Shapes[^1].Coordinates[0].Y);
        }

        [TestMethod]
        public void TestGame()
        {
            mock = new Mock<IPersistence>();

            string[] s = { "4", "0 15 1 15 2 15", "2 14 3 14 3 13 3 15" };
            mock.Setup(obj => obj.ReadAsync("")).Returns(Task.FromResult<string[]>(s));

            _model = new Model(mock.Object, "");
            _model.LoadAsync();

            _model.Shapes[^1].Move(Direction.DOWN);

            Assert.IsTrue(_model.Shapes.Count < 2);
            Assert.IsTrue(_model.Shapes[^1].Coordinates.Count < 4);
        }

        [TestMethod]
        public void TestLost()
        {
            mock = new Mock<IPersistence>();

            string[] s = { "4", "0 0 1 0 2 0 3 0" }; //
            mock.Setup(obj => obj.ReadAsync("")).ReturnsAsync(s);

            _model = new Model(mock.Object, "");
            _model.LoadAsync();

            bool raisedEvent = false;

            _model.GameLost += _model_GameLost;

            _model.AddShape();

            void _model_GameLost(object sender, System.EventArgs e)
            {
                raisedEvent = true;
            }

            Assert.IsTrue(raisedEvent);
        }

    }
}
