using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;

public class TemplateFrame : Form
{
	//Declare constants
	private const int form_width = 1920;
	private const int form_height = 1080;
	private const int control_height = 192;
	private const int graphic_height = form_height - control_height;

	public TemplateFrame()
	{
		//Sets window size, color, and title.
		Size = new Size(form_width, form_height);
		CenterToScreen();
		Text = "TemplateFrame by George Calderon";
		BackColor = Color.SkyBlue;
		//Frame init go here
	}
	//Functions go here
}