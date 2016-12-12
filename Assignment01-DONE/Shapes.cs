//Author: George Calderon
//gcald@csu.fullerton.edu
//The course number: CPSC223n
//Assignment 01
//The due date: September 11 at 2am of the morning
//The purpose of the program: This program will demonstrate drawing three different geometric shapes in any of 3 different colors.
//The file name: Shapes.cs
		
using System;
using System.Windows.Forms;
using Frame;

public class Shapes
{
	public static void Main()
	{
		Draw shapes_application = new Draw();
		Application.Run(shapes_application);
	}
}