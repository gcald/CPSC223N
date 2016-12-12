
using System;
using System.Windows.Forms;
public class Template
{
	public static void Main()
	{
		TemplateFrame template = new TemplateFrame();
		template.FormBorderStyle = FormBorderStyle.FixedSingle;
		Application.Run(template);
	}
}