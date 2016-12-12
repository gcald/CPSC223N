//Author: George Calderon
//gcald@csu.fullerton.edu
//The course number: CPSC223n
//Assignment 04
//The due date: November 13th at 2am of the morning
//The purpose of the program: This program will draw the curve of r = sin(8t/5)
//The file name: PolarAlgorithm.cs

public class Polar_Algorithms
{
	//r = sin(8*t/5)
	public void get_next_coords(double tic_distance,
								ref double t,
								out double new_x,
								out double new_y)
	{
		t += tic_distance; //adds the tic distance to the t variable to go through a domain.
		new_x = System.Math.Sin(((8*t)/5)) * System.Math.Cos(t); //Gives the x value for the function at position t.
		new_y = System.Math.Sin(((8*t)/5)) * System.Math.Sin(t); //Gives the Y value for the function at position t.
	}
}