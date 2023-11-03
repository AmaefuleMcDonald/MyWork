using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using static MyWork.Form1;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Text;
using System.CodeDom;

namespace MyWork
{
   




    public partial class Form1 : Form
    {
        public Action buttonClickAction;
        private PaintEventHandler pictureBox1_Paint;
        private SyntaxChecker syntaxChecker; // Add a syntax checker
        private MenuStrip menuStrip1;
        private Pen pen; //declare the pen
        private Pen blackPen; // set pen color to black
        private Bitmap drawingArea; //bitmap to store the drawing
        private Point penPosition; //Variable to store the current position
        private ToolStripMenuItem LoadToolStripMenuItem;
        private ToolStripMenuItem SaveToolStripMenuItem;

        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private bool fillShapes;
        //private Graphics graphics;
        //private bool errorDisplayed;

        private void LoadFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(' ');
                        if (parts[0] == "penPosition" && parts.Length == 3 &&
                            int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
                        {
                            penPosition = new Point(x, y);
                        }
                        else if (parts[0] == "penColor" && parts.Length == 2)
                        {
                            pen.Color = Color.FromName(parts[1]);
                        }
                        else if (parts[0] == "FillShapes" && parts.Length == 2 && bool.TryParse(parts[1], out bool fill))
                        {
                            fillShapes = fill;
                        }
                    }
                }

            }


        }
        private void SaveToFile(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                //save current state of program
                sw.WriteLine($"PenPosition {penPosition.X} {penPosition.Y}");
                sw.WriteLine($"PenColor {pen.Color.Name}");
                sw.WriteLine($"FillShapes {fillShapes}");
            }
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFromFile(openFileDialog.FileName);
                // Refresh the PictureBox
                pictureBox1.Refresh();
            }
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveToFile(saveFileDialog.FileName);
            }
        }


        //public class CommandExecutor
        //{
           // private SyntaxChecker syntaxChecker;

            //public CommandExecutor()
           // {
           //     syntaxChecker = new SyntaxChecker();
            //}

           // public object buttonClickAction { get; private set; }

            public void ExecuteCommand(string command)
            {
                try
                {
                    syntaxChecker.CheckSyntax(command); //check the syntax of the command
                    string[] input = command.Split(' ');
                    buttonClickAction?.Invoke();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //throw ex;
                }
            }
        //}
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string command = textBox1.Text;
                ExecuteCommand(command);
                button1.PerformClick();
                e.Handled = true;
            }
        }

       // private void ExecuteCommand(string command)
       // {
        //    throw new NotImplementedException();
       // }

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            blackPen = new Pen(Color.Black);
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox1_Paint);
            button1.Click += new System.EventHandler(this.button1_Click);
            penPosition = new Point(10, 10); //the initial position of the pen
            pen = blackPen;// the initial color of the pen
            textBox1.KeyPress += new KeyPressEventHandler(textBox1_KeyPress);
            loadToolStripMenuItem = new ToolStripMenuItem("Load");
            saveToolStripMenuItem = new ToolStripMenuItem("Save");
            loadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
            saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            menuStrip1 = new MenuStrip();
            this.MainMenuStrip = menuStrip1;
            this.Controls.Add(menuStrip1);
            menuStrip1.Items.Add(loadToolStripMenuItem);
            menuStrip1.Items.Add(saveToolStripMenuItem);
            this.Controls.Add(this.pictureBox1);
          //  graphics = Graphics.FromImage(drawingArea);
            drawingArea = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = pictureBox1.CreateGraphics();
            syntaxChecker = new SyntaxChecker();
            Syntax.Click += new System.EventHandler(this.Syntax_Click);
            button1.Click += new System.EventHandler(this.button1_Click);
        }
        private void Syntax_Click(object sender, EventArgs e)
        {
            try
            {
                string command = textBox1.Text;
                syntaxChecker.CheckSyntax(command);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public class SyntaxChecker
        {
            private HashSet<string> validCommands; //list of valid commands
            public SyntaxChecker()
            {
                validCommands = new HashSet<string>
            {
                "moveTo",
                "drawTo",
                "Rectangle",
                "Circle",
                "clear",
                "Triangle",
                "green",
                "reset",
                "red",
                "blue",
                "fill"
            };
            }
            public void CheckSyntax(string command)
            {
                string[] input = command.Split(' ');
                if (input.Length == 0)
                {
                    throw new ArgumentException("Empty command. Please enter a valid command.");
                }
                if (!validCommands.Contains(input[0]))
                {
                    throw new ArgumentException("Invalid command. Please enter a valid command.");
                }
                switch (input[0])
                {
                    case "drawTo":
                        if (input.Length != 3 || !int.TryParse(input[1], out _) || !int.TryParse(input[2], out _))
                        {
                            throw new ArgumentException("Invalid syntax for drawTo command.");
                        }
                        break;

                    case "moveTo":
                        if (input.Length != 3 || !int.TryParse(input[1], out _) || !int.TryParse(input[2], out _))
                        {
                            throw new ArgumentException("Invalid syntax for moveTo command.");
                        }
                        break;
                }
            }
        }
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //A small square at the top left corder of the picturebox
            e.Graphics.FillRectangle(Brushes.Black, penPosition.X, penPosition.Y, 5, 5);
        }
        //private bool fillShapes = false;
        private Graphics graphics;

        private bool errorDisplayed = false;
        private EventHandler Form1_Load;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string command = textBox1.Text;
                syntaxChecker.CheckSyntax(command);
                if (pictureBox1.Image == null)
                {
                    pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                }

                graphics = Graphics.FromImage(pictureBox1.Image);

                string[] input = textBox1.Text.Split(' ');
                if (input[0] == "Circle" && input.Length == 2 && int.TryParse(input[1], out int radius))
                {
                    if (fillShapes)
                    {
                        graphics.FillEllipse(pen.Brush, 10, 10, radius * 2, radius * 2);
                    }
                    else
                    {
                        graphics.DrawEllipse(pen, 10, 10, radius * 2, radius * 2);
                    }
                }
                else if (input[0] == "Rectangle" && input.Length == 3 && int.TryParse(input[1], out int width) && int.TryParse(input[2], out int height))
                {
                    if (fillShapes)
                    {
                        graphics.FillRectangle(pen.Brush, 10, 10, width, height);

                    }
                    else
                    {
                        graphics.DrawRectangle(pen, 10, 10, width, height);
                    }
                }

                else if (input[0] == "Triangle" && input.Length == 3 && int.TryParse(input[1], out int side1) && int.TryParse(input[2], out int side2))
                {
                    if (fillShapes)
                    {
                        Point point1 = new Point(10, 10 + side2);
                        Point point2 = new Point(10 + side1, 10 + side2);
                        Point point3 = new Point(10 + side1 / 2, 10);

                        Point[] trianglePoints = { point1, point2, point3 };

                        graphics.FillPolygon(pen.Brush, trianglePoints);
                    }
                    else
                    {
                        Point point1 = new Point(10, 10 + side2);
                        Point point2 = new Point(10 + side1, 10 + side2);
                        Point point3 = new Point(10 + side1 / 2, 10);

                        Point[] trianglePoints = { point1, point2, point3 };

                        graphics.DrawPolygon(pen, trianglePoints);
                    }
                }
                else if (input[0] == "clear")
                {
                    //Clear the graphics area
                    graphics.Clear(Color.Transparent);
                }
                else if (input[0] == "reset")
                {
                    penPosition = new Point(10, 10);
                }
                else if (input[0].StartsWith("moveTo") && input.Length == 3 && int.TryParse(input[1], out int x) && int.TryParse(input[2], out int y))
                {
                    penPosition = new Point(x, y);
                }
                else if (input[0].StartsWith("drawTo") && input.Length == 3 && int.TryParse(input[1], out int x2) && int.TryParse(input[2], out int y2))
                {
                    Point endPoint = new Point(x2, y2);
                    graphics.DrawLine(pen, penPosition, endPoint);
                    penPosition = endPoint; // update the pen position
                }
                else if (input[0] == "red")
                {
                    pen.Color = Color.Red;
                }
                else if (input[0] == "green")
                {
                    pen.Color = Color.Green;
                }
                else if (input[0] == "blue")
                {
                    pen.Color = Color.Blue;
                }
                else if (input[0] == "fill" && input.Length == 2)
                {
                    if (input[1] == "on")
                    {
                        fillShapes = true;
                    }
                    else if (input[1] == "off")
                    {
                        fillShapes = false;
                    }
                }
                pictureBox1.Refresh();
                errorDisplayed = false;
            }

            catch (ArgumentException ex)
            {
                if (!errorDisplayed)
                {
                    MessageBox.Show(ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorDisplayed = true;
                }
            }

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}


