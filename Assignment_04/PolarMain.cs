//Author: George Calderon
//gcald@csu.fullerton.edu
//The course number: CPSC223n
//Assignment 04
//The due date: November 13th at 2am of the morning
//The purpose of the program: This program will draw the curve of r = sin(8t/5)
//The file name: PolarMain.cs

using System;
using System.Windows.Forms;
public class Polar
{
	public static void Main()
	{
		System.Console.WriteLine("The program will now draw a coil of wires.");
		PolarFrame coils = new PolarFrame();
		coils.FormBorderStyle = FormBorderStyle.FixedSingle;
		Application.Run(coils);
		System.Console.WriteLine("The program has ended.");
	}
}