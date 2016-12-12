//Author: George Calderon
//gcald@csu.fullerton.edu
//The course number: CPSC223n
//Assignment 01
//The due date: September 11 at 2am of the morning
//The purpose of the program: This program will demonstrate drawing three different geometric shapes in any of 3 different colors.
//The file name: UserInterface.cs

using System;
using System.Windows.Forms;
using System.Drawing;

namespace Frame
{
	public class Draw : Form
	{
		private const int form_width = 1600;
		private const int form_height = 900;
		//Creates the button objects
		private Button draw_circle =  new Button();
		private Button draw_rectangle = new Button();
		private Button draw_triangle = new Button();
		private Button color_red = new Button();
		private Button color_green = new Button ();
		private Button color_blue = new Button ();
		private Button draw_shape = new Button ();
		private Button clear_shape = new Button ();
		private Button exit = new Button();
		//Creates points to set location of buttons later
		private Point draw_circle_point = new System.Drawing.Point(20,form_height-75);
		private Point draw_rectangle_point = new System.Drawing.Point(160,form_height-75);
		private Point draw_triangle_point = new System.Drawing.Point(300,form_height-75);
		private Point color_red_point = new System.Drawing.Point(600, form_height-75);
		private Point color_green_point = new System.Drawing.Point(740, form_height-75);
		private Point color_blue_point = new System.Drawing.Point(880, form_height-75);
		private Point draw_shape_point = new System.Drawing.Point(1180,form_height-75);
		private Point clear_shape_point =  new System.Drawing.Point(1320,form_height-75);
		private Point exit_point = new System.Drawing.Point(1460, form_height-75);
		//Sets points for an array later used to create a triangle
		private Point point1 = new System.Drawing.Point(form_width/2, 200);
		private Point point2 = new System.Drawing.Point(form_width/2 - 200, form_height - 300);
		private Point point3 = new System.Drawing.Point(form_width/2 + 200, form_height - 300);
		//Create a bitmap & Graphics to be drawed onto.
		private System.Drawing.Graphics pointer_to_graphics;
		private System.Drawing.Bitmap bitmap =
			new Bitmap(form_width,form_height,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
		Color myColor = Color.Black;
		string myShape;

		public Draw()
		{
			//Sets up window size, title,and centers screen.
			Text = "Shapes by George Calderon";
			Size = new Size(form_width,form_height);
			CenterToScreen();
			//Configures and draws the buttons
			configButton (draw_circle, draw_circle_point, "Draw Circle", "Circle", 1);
			configButton (draw_rectangle, draw_rectangle_point, "Draw Rectangle", "Rectangle", 2);
			configButton (draw_triangle, draw_triangle_point, "Draw Triangle", "Triangle", 3);
			configButton (color_red, color_red_point, "Red", "Red", 4);
			configButton (color_green, color_green_point, "Green", "Green", 5);
			configButton (color_blue, color_blue_point, "Blue", "Blue", 6);
			configButton (draw_shape, draw_shape_point, "Draw Shape", "Draw Shape", 7);
			configButton (clear_shape, clear_shape_point, "Clear Shape", "Clear Shape", 8);
			configButton (exit, exit_point, "Exit", "Exit", 9);
			//Handles the button's click behavior
			draw_circle.Click += new EventHandler(setShape);
			draw_rectangle.Click += new EventHandler(setShape);
			draw_triangle.Click += new EventHandler(setShape);
			color_red.Click += new EventHandler(setColor);
			color_green.Click += new EventHandler(setColor);
			color_blue.Click += new EventHandler(setColor);
			draw_shape.Click += new EventHandler(drawShape);
			clear_shape.Click += new EventHandler(clearShape);
			exit.Click += new EventHandler(Close_window);
			pointer_to_graphics = Graphics.FromImage(bitmap);
			//Sets default bitmap to a white background
			pointer_to_graphics.Clear(System.Drawing.Color.White);

		}

		public void configButton (Button button, Point loc, string Name, string Text, int TabIndex)
		{
			button.Location = loc;
			button.Name = Name;
			button.Size = new System.Drawing.Size(120,40);
			button.Text = Text;
			button.TabIndex = TabIndex;
			button.BackColor = Color.DarkGray;
			Controls.Add (button); //Adds buttons to window
		}
			
		private void setColor(object sender, EventArgs e) //Used to set myColor
		{
			Control ctrl = (Control) sender;
			if (ctrl.Name == "Red")
				myColor = Color.Red;
			else if (ctrl.Name == "Green")
				myColor = Color.Green;
			else if (ctrl.Name == "Blue")
				myColor = Color.Blue;
			else
				myColor = Color.Black;
		}

		private void setShape(object sender, EventArgs e) //Used to set myShape
		{
			Control ctrl = (Control) sender;
			myShape = ctrl.Text;
		}

		protected override void OnPaint(PaintEventArgs ee)
		{
			Graphics graph = ee.Graphics; //Tells where to draw
			graph.DrawImage(bitmap,0,0,form_width,form_height);
			base.OnPaint(ee);
		}

		private void drawShape(object sender, EventArgs e)
		{
			Pen bic = new Pen(myColor,3);
			if (myShape == "Circle"){
				Rectangle circle = new Rectangle(form_width/2 - 200, 200, 400, 400);
				pointer_to_graphics.DrawEllipse(bic, circle); //Draws a circle onto bitmap
			}
			if (myShape == "Rectangle"){
				Rectangle rect = new Rectangle(form_width/2 - 400, 200, 800, 400);
				pointer_to_graphics.DrawRectangle(bic, rect); //Draws a rectangle onto bitmap

			}
			if (myShape == "Triangle"){
				Point[] triPoints = {point1, point2, point3};
				pointer_to_graphics.DrawPolygon(bic, triPoints); //Draws a triangle onto bitmap
			}
			Invalidate(); //Forces an update on Graphics
		}

		private void clearShape(object sender, EventArgs e)
		{
			pointer_to_graphics.Clear(System.Drawing.Color.White);
			Invalidate(); //Forces an update on Graphics
		}

		protected void Close_window(object sender, EventArgs e)
		{
			Close();	
		}
	}
}
