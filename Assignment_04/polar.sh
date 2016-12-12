#!/bin/bash
#In the official documemtation the line above always has to be the first line of any script file.  But, students have 
#told me that script files work correctly without that first line.

#Author: George Calderon
#Course: CPSC223n
#Semester: Fall 2016
#Assignment: 4
#Due:November 13, 2016.

#This is a bash shell script to be used for compiling, linking, and executing the C sharp files of this assignment.
#Execute this file by navigating the terminal window to the folder where this file resides, and then enter the command: ./build.sh

#System requirements: 
#  A Linux system with BASH shell (in a terminal window).
#  The mono compiler must be installed.  If not installed run the command "sudo apt-get install mono-complete" without quotes.
#  The three source files and this script file must be in the same folder.
#  This file, build.sh, must have execute permission.  Go to the properties window of build.sh and put a check in the 
#  permission to execute box.


echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile PolarAlgorithm.cs to create the file: PolarAlgorithm.dll
mcs -t:library PolarAlgorithm.cs -out:PolarAlgorithm.dll

echo Compile PolarFrame.cs to create the file: PolarFrame.dll
mcs -t:library PolarFrame.cs -r:System.Drawing -r:System.Windows.Forms.dll -r:PolarAlgorithm.dll -out:PolarFrame.dll

echo Compile PolarMain.cs and link the previously created dll file to create an executable file. 
mcs -t:exe PolarMain.cs -r:System.Windows.Forms.dll -r:PolarFrame.dll -out:PolarMain.exe

echo View the list of files in the current folder
ls -l

echo Run the Assignment 1 program.
./PolarMain.exe

echo The script has terminated.
