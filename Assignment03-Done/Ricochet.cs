//Author: George Calderon
//gcald@csu.fullerton.edu
//The course number: CPSC223n
//Assignment 03
//The due date: October 22 at 2am of the morning
//The purpose of the program: This program will animate a ball, as well as ricochet it against the walls.
//The file name: Ricochet.cs

using System;
using System.Windows.Forms;
using Frame;

public class Ricochet {
	public static void Main() {
		Draw ricochet_application = new Draw();
		ricochet_application.FormBorderStyle = FormBorderStyle.FixedSingle;
		Application.Run(ricochet_application);
	}
}