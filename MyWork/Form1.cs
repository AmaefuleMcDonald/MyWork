using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MyWork
{
    public partial class Form1 : Form
    {
        private readonly PaintEventHandler pictureBox1_Paint;
        private Point penPosition; // variable to store the current position.
        private Bitmap drawingArea; // bitmap to store the drawing
        private Pen blackPen; // set pen color
        private Pen pen; //declare the pen object



        public Form1()
        {
            InitializeComponent();
            penPosition = new Point(10, 10); // Adjust the position as needed
            blackPen = new Pen(Color.Black);
            pen = blackPen; //InitializeComponent pen object
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox1_Paint);
            // Rest of your code
        }
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Draw a small square at the top left corner of the PictureBox
            e.Graphics.FillRectangle(Brushes.Black, penPosition.X, penPosition.Y, 5, 5);
        }

        private bool fillShapes = false; // to store the state of fill shapes

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            }

            var graphics = Graphics.FromImage(pictureBox1.Image);

            // Clear the graphics area
            graphics.Clear(Color.Yellow);

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
                    Point point1 = new Point(10, 10);
                    Point point2 = new Point(10 + side1, 10);
                    Point point3 = new Point(10 + side1 / 2, 10 + side2);

                    Point[] trianglePoints = { point1, point2, point3 };

                    graphics.FillPolygon(pen.Brush, trianglePoints);
                }
                else
                {
                    Point point1 = new Point(10, 10);
                    Point point2 = new Point(10 + side1, 10);
                    Point point3 = new Point(10 + side1 / 2, 10 + side2);

                    Point[] trianglePoints = { point1, point2, point3 };

                    graphics.DrawPolygon(pen, trianglePoints);
                }
               
            }
            else if (input[0] == "clear")
            {
                graphics.Clear(Color.Yellow);
            }
            else if (input[0]  == "reset")
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
        }
    }
}