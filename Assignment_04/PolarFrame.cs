//Author: George Calderon
//gcald@csu.fullerton.edu
//The course number: CPSC223n
//Assignment 04
//The due date: November 13th at 2am of the morning
//The purpose of the program: This program will draw the curve of r = sin(8t/5)
//The file name: PolarFrame.cs

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;

public class PolarFrame : Form
{
	private const int form_width = 1920; //The width of the monitor's display resolution
	private const int form_height = 1080; //The width of the monitor's display resolution
	private const int control_height = 96; //Area in which the buttons are held.
	private const int graphic_height = form_height - control_height; //Used to limit graphic area to disable drawing into the control area.

	// Used in establishing the smoothness of the polar drawing.
	private const double linear_velocity = 335; //These configurations ensure for  | If crashing is a problem on your pc, change the
	private const double scale_factor = 400.0; // a smooth drawing of the function | refresh and draw rate. The resulting curve will
	private const double refresh_rate = 200;  // as well as minimizing crashing on | not be as solid, but will still show the function 
	private const double draw_rate = 160;    // the tested pc.                     | and still end in 30 seconds. I recommend using a 
	                                        //                                     | refresh rate of 100 and a draw rate of 80.

	private const double pixel_dist = linear_velocity/draw_rate;
	private const double math_dist = pixel_dist/scale_factor;

	private const int x_origin = form_width/2;
	private const int y_origin = graphic_height/2;

	private const int draw_width = 5;

	private double t = 0;
	private double x;
	private double y;

	private double scaled_x;
	private double scaled_y;

	//Declare buttons
	private Button go = new Button();
	private Button exit = new Button();
	private Point go_point = new Point(40, form_height-control_height+20);
	private Point exit_point = new Point(form_width-160, form_height - control_height+20);

	//Declare clocks
	private static System.Timers.Timer graphics_refresh_clock = new System.Timers.Timer();
	private static System.Timers.Timer draw_clock = new System.Timers.Timer();

	//Declare drawing tools
	private Pen bic = new Pen(Color.Black,1);
	Font myFont = new Font("Arial",16);
	Rectangle xStrRect = new Rectangle(500,form_height-control_height+20,140,25);
	Rectangle yStrRect = new Rectangle(800,form_height-control_height+20,140,25);

	//Declares bitmap to be drawn onto
	private System.Drawing.Graphics pointer_to_graphics;
	private System.Drawing.Bitmap bitmap =
		new Bitmap(form_width,form_height,System.Drawing.Imaging.PixelFormat.Format24bppRgb);

	//Declares a toolkit
	Polar_Algorithms algorithms;

	public PolarFrame()
	{
		//Sets window size, color, and title
		Size = new Size(form_width,form_height);
		CenterToScreen();
		Text = "Curve of r = sin(8*t/5) by George Calderon";
		BackColor = Color.White;

		//Instantiates the collection
		algorithms = new Polar_Algorithms();

		//Set initial values for the graph
		t = 0.0;
		x = System.Math.Sin(((8*t)/5)) * System.Math.Cos(t);
		y = System.Math.Sin(((8*t)/5)) * System.Math.Sin(t);

		//Configures the buttons
		configButton(go, go_point, "Go","Go",1);
		configButton(exit, exit_point, "Exit","Exit",2);

		//Initializes the Clocks
		graphics_refresh_clock.Enabled = false;
		graphics_refresh_clock.Elapsed += new ElapsedEventHandler(Update_graphics);
		draw_clock.Enabled = false;
		draw_clock.Elapsed += new ElapsedEventHandler(Update_position);

		//Allocate more memory for a smoother draw
		DoubleBuffered = true;

		//Allows bitmap to be drawn onto graphics area
		pointer_to_graphics = Graphics.FromImage(bitmap);
		init_bitmap();

		//Adds the buttons to the window
		Controls.Add(go);
		go.Click += new EventHandler(Start);
		Controls.Add(exit);
		exit.Click += new EventHandler(Close_window);
	}

	public void configButton (Button button, Point loc, string Name, string Text, int TabIndex)
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
		if (ctrl.Text == "Go")
		{
			go.Text = "Pause";
			start_gClock(refresh_rate);
			start_dClock(refresh_rate);
		}
		else if (ctrl.Text == "Pause")
		{
			go.Text = "Resume";
			graphics_refresh_clock.Enabled = false;
			draw_clock.Enabled = false;
		}
		else if (ctrl.Text == "Resume")
		{
			go.Text = "Pause";
			start_gClock(refresh_rate);
			start_dClock(refresh_rate);
		}
	}

	protected void init_bitmap()
	{
		pointer_to_graphics.Clear(System.Drawing.Color.SkyBlue); //Creates a default blue screen
		//Draws the axes
		bic.Width = 1;
		bic.DashStyle = DashStyle.Dot;
		pointer_to_graphics.DrawLine(bic, 0, graphic_height/2, form_width, graphic_height/2);
		pointer_to_graphics.DrawLine(bic, form_width/2, 0, form_width/2, graphic_height);
		bic.DashStyle = DashStyle.Solid;
		//Labels the x axis.
		Rectangle zero_label = new Rectangle(x_origin-15,y_origin,35,25);
		pointer_to_graphics.DrawString("0.0", myFont, Brushes.Black, zero_label);
		Rectangle one_label = new Rectangle((x_origin-15)+(int)System.Math.Round(scale_factor), y_origin,35,25);
		pointer_to_graphics.DrawString("+1.0", myFont, Brushes.Black, one_label);
		Rectangle negative_one_label = new Rectangle((x_origin-15)-(int)System.Math.Round(scale_factor), y_origin,35,25);
		pointer_to_graphics.DrawString("-1.0", myFont, Brushes.Black, negative_one_label);
		Rectangle half_label = new Rectangle((x_origin-15)+(int)System.Math.Round(scale_factor * .5), y_origin,35,25);
		pointer_to_graphics.DrawString("+.5", myFont, Brushes.Black, half_label);
		Rectangle negative_half_label = new Rectangle((x_origin-15)+(int)System.Math.Round(scale_factor * -.5), y_origin,35,25);
		pointer_to_graphics.DrawString("+.5", myFont, Brushes.Black, negative_half_label);
		//Draws control area
		pointer_to_graphics.FillRectangle(Brushes.BlanchedAlmond, 0, form_height-control_height, form_width, control_height);
	}

	protected void Update_graphics(object sender, EventArgs e)
	{
		int int_scaled_x = (int)System.Math.Round(scaled_x);
		int int_scaled_y = (int)System.Math.Round(scaled_y);
		pointer_to_graphics.FillEllipse(Brushes.Red, x_origin + int_scaled_x-draw_width/2, 
													 y_origin + int_scaled_y-draw_width/2,
													 draw_width, draw_width);
		pointer_to_graphics.FillRectangle(Brushes.BlanchedAlmond,xStrRect);
		pointer_to_graphics.FillRectangle(Brushes.BlanchedAlmond, yStrRect);
		pointer_to_graphics.DrawString("x = " + (int_scaled_x) + "", myFont, Brushes.Black, xStrRect);
		pointer_to_graphics.DrawString("y = " + (int_scaled_y) + "", myFont, Brushes.Black, yStrRect);
		Invalidate();
	}

	protected void Update_position(object sender, EventArgs e)
	{
		//Gets the next coordinates and proceeds to scale them by the scale factor.
		algorithms.get_next_coords(math_dist, ref t, out x, out y);
		scaled_x = scale_factor * x;
		scaled_y = scale_factor * y;
	}

	protected override void OnPaint(PaintEventArgs ee)
	{
		Graphics graph = ee.Graphics; //Tells where to draw
		graph.DrawImage(bitmap,0,0,form_width,form_height);
		base.OnPaint(ee);
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

	protected void Close_window(object sender, EventArgs e)
	{
		Close();	
	}
}