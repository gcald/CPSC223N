//Author: George Calderon
//gcald@csu.fullerton.edu
//The course number: CPSC223n
//Assignment 05
//The due date: November 27th at 2am of the morning
//The purpose of the program: This program will run an apple catching game.
//The file name: ApplesMain.cs

using System;
using System.Windows.Forms;
public class Apples
{
	public static void Main()
	{
		System.Console.WriteLine("The program will now start the game \"Falling Apples\".");
		ApplesFrame apple = new ApplesFrame();
		apple.FormBorderStyle = FormBorderStyle.FixedSingle;
		Application.Run(apple);
		System.Console.WriteLine("The program has ended.");
	}
}