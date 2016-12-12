
/*
	//Configures the buttons
	configButton(start, start_point, "Start","Start",1);
	configButton(pause, pause_point, "Pause", "Pause",2);
	configButton(clear, clear_point, "Clear", "Clear", 3);
	configButton(exit, exit_point, "Exit","Exit",4);
 *
	private void configButton (NonFocusButton button, Point loc, string Name, string Text, int TabIndex)
	{
		button.Location = loc;
		button.Name = Name;
		button.Size = new System.Drawing.Size(120,40);
		button.Text = Text;
		button.TabIndex = TabIndex;
		button.BackColor = Color.DarkGray;
	}
*
*
	protected override void OnPaint(PaintEventArgs ee)
	{
		Graphics graph = ee.Graphics; //Tells where to draw
		graph.DrawImage(bitmap,0,0,form_width,form_height);
		base.OnPaint(ee);
	}
*
*
	protected void Close_window(object sender, EventArgs e)
	{
		Close();	
	}
 */



