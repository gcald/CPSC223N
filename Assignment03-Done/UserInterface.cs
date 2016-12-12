//Author: George Calderon
//gcald@csu.fullerton.edu
//The course number: CPSC223n
//Assignment 03
//The due date: October 22 at 2am of the morning
//The purpose of the program: This program will animate a ball, as well as ricochet it against the walls.
//The file name: UserInterface.cs

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using Math;

namespace Frame
{
	public class Draw : Form
	{
		private const int form_width = 1289; //offset due to bordering
		private const int form_height = 900;
		private const int graphic_width = 1280;
		private const int graphic_height = 720;
		private const double refresh_rate = 30.0;
		private const double ball_speed = 150.0;
		private int ballX = graphic_width/2 - 12;
		private int ballY = graphic_height/2 - 12;
		private static int postRadius = Convert.ToInt32(graphic_height * .05);
		private int postX = graphic_width/4 - postRadius;
		private int postY = graphic_height/2 - postRadius;
		private double angle = 0.00, stringAngle = 0.00, rcount = 0, dist = 0;
		private bool destroyBall = false;
		//Creates the button objects
		private Button exit = new Button();
		private Button go = new Button();
		//Creates points to set location of buttons later
		private Point go_point = new System.Drawing.Point(900, form_height-120);
		private Point exit_point = new System.Drawing.Point(1100,form_height-120);
		//Create a bitmap & Graphics to be drawed onto.
		private System.Drawing.Graphics pointer_to_graphics;
		private System.Drawing.Bitmap bitmap =
			new Bitmap(form_width,form_height,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
		//Creates objects used for drawing
		private Font drawFont = new Font("Arial",16);
		private Pen bic = new Pen(Color.Black,3);
		private SolidBrush brush = new SolidBrush(Color.BlanchedAlmond);
		//Creates timers for ball and graphics
		private static System.Timers.Timer graphics_refresh_clock = new System.Timers.Timer();
		private static System.Timers.Timer ball_timer = new System.Timers.Timer();



		public Draw()
		{
			//Sets up window size, title,and centers screen.
			Text = "Ricochet Action by George Calderon";
			Size = new Size(form_width,form_height);
			CenterToScreen();
			//Configures and draws the buttons
			configButton (go, go_point, "Go", "Start", 1);
			configButton (exit, exit_point, "Exit", "Exit", 2);
			//Handles the button's click behavior
			go.Click += new EventHandler(Start);
			exit.Click += new EventHandler(Close_window);
			pointer_to_graphics = Graphics.FromImage(bitmap);
			//Sets default bitmap to a white background
			pointer_to_graphics.Clear(System.Drawing.Color.White);
			//Configures the button area
			//Draws the button area
			Rectangle buttonArea = new Rectangle(0, 722, graphic_width, 150);
			pointer_to_graphics.FillRectangle(brush, buttonArea);
			pointer_to_graphics.DrawRectangle(bic, buttonArea);
			//Draws text onto button area
			Rectangle aRect = new Rectangle(110,750,150,0);
			Rectangle cRect = new Rectangle(265,750,150,0);
			Rectangle dRect = new Rectangle(505,730,125,0);
			pointer_to_graphics.DrawString("Angle", drawFont, Brushes.Black, aRect);
			pointer_to_graphics.DrawString("Ricochet Count", drawFont, Brushes.Black, cRect);
			pointer_to_graphics.DrawString("  Distance Ball to Post", drawFont, Brushes.Black, dRect);
			//deals with clocks
			graphics_refresh_clock.Enabled = false;
			graphics_refresh_clock.Elapsed += new ElapsedEventHandler(Update_Display);
			ball_timer.Enabled = false;
			ball_timer.Elapsed += new ElapsedEventHandler(Update_Ball);
			drawNumbers();
			Invalidate();

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
			
		protected override void OnPaint(PaintEventArgs ee)
		{
			Graphics graph = ee.Graphics; //Tells where to draw
			graph.DrawImage(bitmap,0,0,form_width,form_height);
			base.OnPaint(ee);
		}

		public void EnableDoubleBuffering() //Removes flicker
		{
			this.SetStyle(ControlStyles.DoubleBuffer | 
				ControlStyles.UserPaint | 
				ControlStyles.AllPaintingInWmPaint,
				true);
			this.UpdateStyles();
		}

		private void Start(object sender, EventArgs e)
		{
			Control ctrl = (Control) sender;
			if (ctrl.Text == "Start")
			{
				go.Text = "Pause";
				getAngle();
				start_gClock(refresh_rate);
				start_bClock(refresh_rate);
			}
			else if (ctrl.Text == "Go")
			{
				go.Text = "Pause";
				start_gClock(refresh_rate);
				start_bClock(refresh_rate);
			}
			else if (ctrl.Text == "Pause")
			{
				go.Text = "Go";
				graphics_refresh_clock.Enabled = false;
				ball_timer.Enabled = false;
			}
		}

		private void getAngle()
		{
			angle = Math.Math.setAngle();
		}

		private void getDist()
		{
			dist = Math.Math.getDist(ballX + 12,ballY + 12,postX + postRadius,postY + postRadius);
			if(dist <= 50) //50 is the distance in which the ball touches the post
				destroyBall = true;
		}

		private void drawNumbers()
		{
			//Draws the blue area
			brush.Color = Color.SkyBlue;
			Rectangle blueArea = new Rectangle(0,0,graphic_width,graphic_height);
			pointer_to_graphics.FillRectangle(brush, blueArea);
			pointer_to_graphics.DrawRectangle(bic, blueArea);
			//Draw the post
			Rectangle post = new Rectangle(postX, postY, postRadius * 2, postRadius * 2);
			pointer_to_graphics.FillEllipse(Brushes.Purple, post);
			//Draw rectangular area which will hold the numbers.
			brush.Color = Color.White;
			Rectangle aIntRect = new Rectangle(100,780,75,75);
			pointer_to_graphics.FillRectangle(brush, aIntRect);
			pointer_to_graphics.DrawRectangle(bic, aIntRect);
			Rectangle cIntRect = new Rectangle(300,780,75,75);
			pointer_to_graphics.FillRectangle(brush, cIntRect);
			pointer_to_graphics.DrawRectangle(bic, cIntRect);
			Rectangle dIntRect = new Rectangle(525,780,75,75);
			pointer_to_graphics.FillRectangle(brush, dIntRect);
			pointer_to_graphics.DrawRectangle(bic, dIntRect);
			Rectangle aStrRect = new Rectangle(105,807,70,25);
			Rectangle cStrRect = new Rectangle(310,807,70,25);
			Rectangle dStrRect = new Rectangle(535,807,70,25);
			if(angle != 0) //Used to limit angle to 0 to 360 (For readability)
				stringAngle = 360 - angle;
			if(stringAngle > 360)
				stringAngle -= 360;
			else if(stringAngle < 0)
				stringAngle += 360;
			pointer_to_graphics.DrawString(System.Math.Round(stringAngle,2).ToString(), drawFont, Brushes.Black, aStrRect);
			pointer_to_graphics.DrawString("" + rcount + "", drawFont, Brushes.Black, cStrRect);
			pointer_to_graphics.DrawString("" + dist + "", drawFont, Brushes.Black, dStrRect);
			//Draws ball
			if(destroyBall == false)
			{
				Rectangle ball = new Rectangle(ballX,ballY,24,24);
				pointer_to_graphics.FillEllipse(Brushes.Orange, ball);
			}
			else
			{
				ball_timer.Enabled = false;
				graphics_refresh_clock.Enabled = false;
				go.Dispose();
			}
		}

		protected void start_gClock(double refresh)
		{
			double time_tics;
			if(refresh < 1.0)
				refresh = 1.0;
			time_tics = 1000.0/refresh;
			graphics_refresh_clock.Interval = (int)System.Math.Round(time_tics);
			graphics_refresh_clock.Enabled = true;
		}

		protected void start_bClock(double update)
		{
			double time_tics;
			if(update < 1.0)
				update = 1.0;
			time_tics = 1000.0/update;
			ball_timer.Interval = (int)System.Math.Round(time_tics);
			ball_timer.Enabled = true;
		}

		protected void Update_Display(object sender, EventArgs e)
		{
			getDist();
			drawNumbers();
			Invalidate();
		}

		protected void Update_Ball(object sender, EventArgs e)
		{
			ballX += (int)System.Math.Round(ball_speed/refresh_rate * System.Math.Cos(angle * System.Math.PI/180));
			ballY += (int)System.Math.Round(ball_speed/refresh_rate * System.Math.Sin(angle * System.Math.PI/180));
			if(ballX < 0 || ballX > graphic_width - 25)
			{
				angle = 180 - angle;
				rcount += 1;
			}
			if(ballY < 0 || ballY > graphic_height - 25)
			{
				angle = 360 - angle;
				rcount += 1;
			}
		}

		protected void Close_window(object sender, EventArgs e)
		{
			Close();	
		}
	}
}
