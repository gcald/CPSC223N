//Author: George Calderon
//gcald@csu.fullerton.edu
//The course number: CPSC223n
//Assignment 05
//The due date: November 27th at 2am of the morning
//The purpose of the program: This program will run an apple catching game.
//The file name: ApplesLogic.cs

using System;
using System.Windows.Forms;

namespace Logic
{
	public class Logic
	{
		private static Random randGen = new System.Random();

		public static int setX(int form_width)
		{
			int rand = randGen.Next(20, form_width - 20) ;//Changed from 10 to 20 to avoid "uncatchable apples".
			return rand;
		}
	}
}


namespace Buttons
{
	public class NonFocusButton : Button
	{
		public NonFocusButton()
		{
			SetStyle(ControlStyles.Selectable, false);
		}
	}
}