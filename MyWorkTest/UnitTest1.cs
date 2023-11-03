using NUnit.Framework;
using System;
using MyWork;

namespace MyWork.Tests
{
    [TestFixture]
    public class CommandTests
    {
        private Form1 form;

        [SetUp]
        public void Setup()
        {
            form = new Form1();
        }

        [Test]
        public void TestValidMoveToCommand()
        {
            string command = "moveTo 100 100";
            Assert.DoesNotThrow(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestInvalidMoveToCommand()
        {
            string command = "moveTo 100";
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestValidDrawToCommand()
        {
            string command = "drawTo 200 200";
            Assert.DoesNotThrow(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestInvalidDrawToCommand()
        {
            string command = "drawTo 200";
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestValidClearCommand()
        {
            string command = "clear";
            Assert.DoesNotThrow(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestInvalidClearCommand()
        {
            string command = "clears";
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestValidResetCommand()
        {
            string command = "reset";
            Assert.DoesNotThrow(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestInvalidResetCommand()
        {
            string command = "resets";
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestValidRectangleCommand()
        {
            string command = "Rectangle";
            Assert.DoesNotThrow(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestInvalidRectangleCommand()
        {
            string command = "Rectangles";
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestValidCircleCommand()
        {
            string command = "Circle";
            Assert.DoesNotThrow(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestInvalidCircleCommand()
        {
            string command = "Circles";
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestValidTriangleCommand()
        {
            string command = "Triangle";
            Assert.DoesNotThrow(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestInvalidTriangleCommand()
        {
            string command = "Triangles";
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command));
        }

        [Test]
        public void TestValidColorCommand()
        {
            string command1 = "blue";
            string command2 = "green";
            string command3 = "red";
            Assert.DoesNotThrow(() => form.ExecuteCommand(command1));
            Assert.DoesNotThrow(() => form.ExecuteCommand(command2));
            Assert.DoesNotThrow(() => form.ExecuteCommand(command3));
        }

        [Test]
        public void TestInvalidColorCommand()
        {
            string command1 = "blue";
            string command2 = "green";
            string command3 = "red";
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command1));
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command2));
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command3));
        }


        [Test]
        public void TestValidFillShapesCommand()
        {
            string command1 = "fill on";
            string command2 = "fill off";
            Assert.DoesNotThrow(() => form.ExecuteCommand(command1));
            Assert.DoesNotThrow(() => form.ExecuteCommand(command2));
        }

        [Test]
        public void TestInvalidFillShapesCommand()
        {
            string command1 = "fill on";
            string command2 = "fill off";
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command1));
            Assert.Throws<ArgumentException>(() => form.ExecuteCommand(command2));
        }



        // Add more tests for other commands here...

        [TearDown]
        public void TearDown()
        {
            form.Dispose();
        }
    }
}
