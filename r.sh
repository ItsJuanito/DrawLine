#!/bin/bash


#Author: FJuan Zaragoza
#Mail: zaragoza_9@csu.fullerton.edu

#Program name: Assignment #3
#Programming language: C Sharp
#Date development of program began: 2022-Sep-26
#Date of last update: 2022-Sep-26

#Purpose:  This program will demonstrate how to draw a line and have a ball move along that line

#Files in project: DrawLineMain.cs, DrawLineUI.cs, r.sh

#Execute this file by entering the command in a Bash shell: "sh r.sh" without the quotes.

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo "Compile the file DrawLineUI.cs:"
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:DrawLineUI.dll DrawLineUI.cs

echo "Compile and link DrawLineMain.cs:"
mcs -r:System -r:System.Windows.Forms -r:DrawLineUI.dll -out:go.exe DrawLineMain.cs

echo "Run the program Assignment #3"
./go.exe

echo "The bash script has terminated."