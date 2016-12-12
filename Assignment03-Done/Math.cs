//Author: George Calderon
//gcald@csu.fullerton.edu
//The course number: CPSC223n
//Assignment 03
//The due date: October 22 at 2am of the morning
//The purpose of the program: This program will animate a ball, as well as ricochet it against the walls.
//The file name: Ricochet.cs

using System;

namespace Math
{
	public class Math
	{
		private static Random randGen = new System.Random();

		public static double setAngle() //Sets a random angle between -pi/2 & pi/2
		{
			double rand = randGen.NextDouble();
			rand = rand -.5;
			double angle = System.Math.PI * rand;
			return angle * (180 / System.Math.PI);
		}

		public static double getDist(int ballX, int ballY, int postX, int postY) // uses pyt. thm. to get distance.
		{
			double distance = System.Math.Sqrt(System.Math.Pow(ballX - postX,2) + System.Math.Pow(ballY - postY,2));
			return System.Math.Round(distance,1);
		}
	}
}

