//Author: George Calderon
//gcald@csu.fullerton.edu
//The course number: CPSC223n
//Assignment 05
//The due date: November 27th at 2am of the morning
//The purpose of the program: This program will run an apple catching game.
//The file name: ApplesFrame.cs

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;
using Buttons;
using Logic;

public class ApplesFrame : Form
{
	//Declare constants
	private const int form_width = 1920;
	private const int form_height = 1080;
	private const int control_height = 192;
	private const int graphic_height = form_height - control_height;
	private const int basket_width = 42; //24 is waaay too small for such a screen. I recommend at least 36 for playability. Best case @ 42;
	private const int basket_height = 10;
	private const int refresh_rate = 30;
	private const int basket_speed = 30;
	private const int apple_radius = 10;

	//Declare variables
	private int appleX;
	private int appleY;
	private int apple_speed = 125;
	private double applesCount = 1;
	private double applesCaught = 0;
	private int applesMissed = 0;
	private int basketX;
	private int basketY;
	private int delta;
	private double success_rate = 0.00;

	enum keypos {up,down}

	private keypos left = keypos.up;
	private keypos right = keypos.up;

	//Declare buttons
	private NonFocusButton start = new NonFocusButton();
	private NonFocusButton pause = new NonFocusButton();
	private NonFocusButton clear = new NonFocusButton();
	private NonFocusButton exit = new NonFocusButton();

	//Declare locations
	private Point start_point = new Point(50, (form_height - control_height) + 25);
	private Point pause_point = new Point(250, (form_height - control_height) + 25);
	private Point clear_point = new Point(50, (form_height - control_height) + 100);
	private Point exit_point = new Point(250, (form_height - control_height) + 100);

	//Create a bitmap & Graphics to be drawed onto.
	private System.Drawing.Graphics pointer_to_graphics;
	private System.Drawing.Bitmap bitmap =
		new Bitmap(form_width,form_height,System.Drawing.Imaging.PixelFormat.Format24bppRgb);

	//Creates objects used for drawing
	private Font drawFont = new Font("Arial",16);
	private Pen bic = new Pen(Color.Black,3);
	private SolidBrush brush = new SolidBrush(Color.BlanchedAlmond);

	//Creates timers for apple and graphics
	private static System.Timers.Timer graphics_refresh_clock = new System.Timers.Timer();
	private static System.Timers.Timer draw_clock = new System.Timers.Timer();

	public ApplesFrame()
	{
		//Sets window size, color, and title.
		Size = new Size(form_width, form_height);
		CenterToScreen();
		Text = "Falling Apples by George Calderon";
		BackColor = Color.SkyBlue;

		//Configures the buttons
		configButton(start, start_point, "Start","Start",1);
		configButton(pause, pause_point, "Pause", "Pause",2);
		configButton(clear, clear_point, "Clear", "Clear", 3);
		configButton(exit, exit_point, "Exit","Exit",4);

		//Initializes the Clocks
		graphics_refresh_clock.Enabled = false;
		graphics_refresh_clock.Elapsed += new ElapsedEventHandler(Update_graphics);
		draw_clock.Enabled = false;
		draw_clock.Elapsed += new ElapsedEventHandler(Update_position);

		//Allocate more memory for a smoother draw
		DoubleBuffered = true;

		//Allows bitmap to be drawn onto graphics area
		pointer_to_graphics = Graphics.FromImage(bitmap);

		//Adds the buttons to the window
		Controls.Add(start);
		Controls.Add(pause);
		Controls.Add(clear);
		Controls.Add(exit);

		//Gives buttons their click function.
		start.Click += new EventHandler(Start);
		pause.Click += new EventHandler(Start);
		clear.Click += new EventHandler(Reset);
		exit.Click += new EventHandler(Close_window);

		//Initialized basket in the center the graphic area.
		basketX = (form_width - basket_width)/2;
		basketY = (graphic_height - basket_height) - 25;

		//Allows for keystokes to be recorded
		KeyPreview = true;
		KeyUp += new KeyEventHandler(OnKeyUp);

		//Draws Graphic Area
		brush.Color = Color.SkyBlue;
		Rectangle graphic_area = new Rectangle(0,0,form_width,graphic_height);
		pointer_to_graphics.FillRectangle(brush, graphic_area);

		//Draws Control Area
		brush.Color = Color.BlanchedAlmond;
		Rectangle control_area = new Rectangle(0, form_height - control_height, form_width, control_height);
		pointer_to_graphics.FillRectangle(brush, control_area);
		pointer_to_graphics.DrawRectangle(bic, control_area);
		Rectangle strCaught = new Rectangle (1500, (form_height - control_height) + 25, 150,75);
		pointer_to_graphics.DrawString("Apples Caught:                    " + applesCaught + "", drawFont, Brushes.Black, strCaught); //Spaces added for formatting
		Rectangle strMissed = new Rectangle (1700, (form_height - control_height) + 25, 150,75);
		pointer_to_graphics.DrawString("Apples Missed:                    " + applesMissed + "", drawFont, Brushes.Black, strMissed); //Spaces added for formatting
		Rectangle strSuccess = new Rectangle (1500, (form_height - control_height) + 75, 170,75);
		pointer_to_graphics.DrawString("Success Percent:                                      " + Math.Round(success_rate,2) + "", drawFont, Brushes.Black, strSuccess); //Spaces added for formatting

		//Creates an apple slightly off screen.
		createApple();

	}

	private void configButton (NonFocusButton button, Point loc, string Name, string Text, int TabIndex)
	{
		button.Location = loc;
		button.Name = Name;
		button.Size = new System.Drawing.Size(120,40);
		button.Text = Text;
		button.TabIndex = TabIndex;
		button.BackColor = Color.DarkGray;
	}

	private void Start(object sender, EventArgs e)
	{
		Control ctrl = (Control) sender;
		if (ctrl.Text == "Start")
		{
			start_gClock(refresh_rate);
			start_dClock(refresh_rate);
		}
		else if (ctrl.Text == "Pause")
		{
			graphics_refresh_clock.Enabled = false;
			draw_clock.Enabled = false;
		}
	}

	private void Reset(object sender, EventArgs e)
	{
		//Stops Clocks
		graphics_refresh_clock.Enabled = false;
		draw_clock.Enabled = false;
		//Reset variables to their initial values
		apple_speed = 125;
		applesCount = 1;
		applesCaught = 0;
		applesMissed = 0;
		//Initialized basket in the center the graphic area.
		basketX = (form_width - basket_width)/2;
		basketY = (graphic_height - basket_height) - 25;
		//Draws Graphic Area
		brush.Color = Color.SkyBlue;
		Rectangle graphic_area = new Rectangle(0,0,form_width,graphic_height);
		pointer_to_graphics.FillRectangle(brush, graphic_area);

		//Draws Control Area
		brush.Color = Color.BlanchedAlmond;
		Rectangle control_area = new Rectangle(0, form_height - control_height, form_width, control_height);
		pointer_to_graphics.FillRectangle(brush, control_area);
		pointer_to_graphics.DrawRectangle(bic, control_area);
		Rectangle strCaught = new Rectangle (1500, (form_height - control_height) + 25, 150,75);
		pointer_to_graphics.DrawString("Apples Caught:                    " + applesCaught + "", drawFont, Brushes.Black, strCaught); //Spaces added for formatting
		Rectangle strMissed = new Rectangle (1700, (form_height - control_height) + 25, 150,75);
		pointer_to_graphics.DrawString("Apples Missed:                    " + applesMissed + "", drawFont, Brushes.Black, strMissed); //Spaces added for formatting
		Rectangle strSuccess = new Rectangle (1500, (form_height - control_height) + 75, 170,75);
		pointer_to_graphics.DrawString("Success Percent:                                      " + Math.Round(success_rate,2) + "", drawFont, Brushes.Black, strSuccess); //Spaces added for formatting

		//Creates an apple slightly off screen.
		createApple();
		Invalidate();
	}

	private void createApple()
	{
		appleX = Logic.Logic.setX(form_width); //Sets random x-value from (10,form_width-10)
		appleY = -40; //Sets apple just offscreen
		apple_speed += 25; 
		success_rate = applesCaught/applesCount;
		Rectangle control_area = new Rectangle(0, form_height - control_height, form_width, control_height); //Redraws control area with new values
		pointer_to_graphics.FillRectangle(Brushes.BlanchedAlmond, control_area);
		pointer_to_graphics.DrawRectangle(bic, control_area);
		Rectangle strCaught = new Rectangle (1500, (form_height - control_height) + 25, 150,75);
		pointer_to_graphics.DrawString("Apples Caught:                    " + applesCaught + "", drawFont, Brushes.Black, strCaught);
		Rectangle strMissed = new Rectangle (1700, (form_height - control_height) + 25, 150,75);
		pointer_to_graphics.DrawString("Apples Missed:                    " + applesMissed + "", drawFont, Brushes.Black, strMissed);
		Rectangle strSuccess = new Rectangle (1500, (form_height - control_height) + 75, 170,75);
		pointer_to_graphics.DrawString("Success Percent:                                      " + Math.Round(success_rate,2) + "", drawFont, Brushes.Black, strSuccess);
		Invalidate();
	}

	private void OnKeyUp(object sender, KeyEventArgs e)
	{
		if(e.KeyCode == Keys.Right)
		{
			right = keypos.up;
			if(left == keypos.down) //Prevents moving if both keys are pressed.
				delta = -basket_speed;
			else
				delta = 0;
		}
		else if(e.KeyCode ==Keys.Left)
		{
			left = keypos.up;
			if(right == keypos.down) //Prevents moving if both keys are pressed.
				delta = basket_speed;
			else
				delta = 0;
		}
		basketX += delta;
		Invalidate();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys KeyCode) //Used to process key commands
	{
		if(KeyCode == Keys.Right)
		{
			right = keypos.down;
			if(left == keypos.up)
				delta = basket_speed;
		}
		else if (KeyCode == Keys.Left)
		{
			left = keypos.down;
			if(right == keypos.up)
				delta = -basket_speed;
		}
		basketX += delta;
		Invalidate();
		return base.ProcessCmdKey(ref msg, KeyCode);
	}

	protected override void OnPaint(PaintEventArgs ee)
	{
		Graphics graph = ee.Graphics; //Tells where to draw
		graph.DrawImage(bitmap,0,0,form_width,form_height);
		graph.FillRectangle(Brushes.SaddleBrown, basketX,basketY,basket_width,basket_height); //Draws basket on top of the bitmap.
		base.OnPaint(ee);
	}

	protected void Update_graphics(object sender, EventArgs e)
	{
		Rectangle graphic_area = new Rectangle(0,0,form_width,graphic_height);
		pointer_to_graphics.FillRectangle(Brushes.SkyBlue, graphic_area);
		Rectangle apple = new Rectangle(appleX,appleY,apple_radius * 2,apple_radius * 2);
		pointer_to_graphics.FillEllipse(Brushes.Red, apple);
		if(appleY >= graphic_height - (25+basket_height)) //Prevents unnecessary redraws of control area
		{
			Rectangle control_area = new Rectangle(0, form_height - control_height, form_width, control_height);
			pointer_to_graphics.FillRectangle(Brushes.BlanchedAlmond, control_area);
			pointer_to_graphics.DrawRectangle(bic, control_area);
			Rectangle strCaught = new Rectangle (1500, (form_height - control_height) + 25, 150,75);
			pointer_to_graphics.DrawString("Apples Caught:                    " + applesCaught + "", drawFont, Brushes.Black, strCaught);
			Rectangle strMissed = new Rectangle (1700, (form_height - control_height) + 25, 150,75);
			pointer_to_graphics.DrawString("Apples Missed:                    " + applesMissed + "", drawFont, Brushes.Black, strMissed);
			Rectangle strSuccess = new Rectangle (1500, (form_height - control_height) + 75, 170,75);
			pointer_to_graphics.DrawString("Success Percent:                                      " + Math.Round(success_rate,2) + "", drawFont, Brushes.Black, strSuccess);
			checkCollision(); //Used to check if the apple has been caught
		}
		Invalidate();
	}

	private void checkCollision()
	{
			if(appleX >= basketX && appleX + apple_radius < basketX + basket_width)  //Makes sure more than half of the apple is within the basket.
			{
				applesCaught += 1;
				createApple();
				applesCount += 1;
			}
		else if (appleY >= basketY + basket_height)
			{
				applesMissed += 1;
				createApple();
				applesCount += 1;
			}
	}

	protected void Update_position(object sender, EventArgs e)
	{
		if(applesCount <= 15)
			appleY += apple_speed/refresh_rate; //Ensures smooth graphics update.
		if(applesCount > 15)
			endGame();
	}

	protected void endGame()
	{
		//Ends game with a big "Game Over" screen.
		graphics_refresh_clock.Enabled = false;
		draw_clock.Enabled = false;
		Rectangle end = new Rectangle(form_width/3 + 50,graphic_height/4, form_width, 175);
		pointer_to_graphics.DrawString("GAME OVER",new Font("Arial",72),Brushes.Black, end);
		Rectangle end2 = new Rectangle(form_width/3 - 20,graphic_height/4 + 100, form_width, 175);
		pointer_to_graphics.DrawString("Press \"Clear\" to try again!", new Font("Arial",48), Brushes.Black, end2);
		Invalidate();

	}

	protected void start_gClock(double refresh) //The clock used for refreshing the screen
	{
		double time_tics;
		if(refresh < 1.0)
			refresh = 1.0;
		time_tics = 1000.0/refresh;
		graphics_refresh_clock.Interval = (int)System.Math.Round(time_tics);
		graphics_refresh_clock.Enabled = true;
	}

	protected void start_dClock(double update) //The clock used for drawing (updating position)
	{
		double time_tics;
		if(update < 1.0)
			update = 1.0;
		time_tics = 1000.0/update;
		draw_clock.Interval = (int)System.Math.Round(time_tics);
		draw_clock.Enabled = true;
	}

	private void Close_window(object sender, EventArgs e)
	{
		Close();
	}
}