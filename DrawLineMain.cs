//Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========0=========1=========2=========3**
//Author: Juan Zaragoza
//Mail: zaragoza_9@csu.fullerton.edu

//Program name: Assignment #3
//Programming language: C Sharp
//Date development of program began: 2022-Sep-23
//Date of last update: 2022-Sep-17

//Purpose:  This programs demonstrate how to use user input tp draw a line and make a ball stay on it while still maintain constant speed.

//Files in project: DrawLineMain.cs, DrawLineUI.cs, r.sh

//This file's name: DrawLineUI.cs
//This file purpose: This module initiates the creation of the UI.
//Date last modified: 2022-Sep-26

//Known issues: Functionally the program meets the requirements, namely: create a UI using panels and employ the
//the correct mathematics to make a straight line.  The source code in places could use some cosmetic clean up
//for better understandability.

//To compile straight-line-travel-user-interface.cs:
//      mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:DrawLineUI.dll DrawLineUI.cs
//To compile  and link straight-line-travel-main.cs:
//      mcs -r:System -r:System.Windows.Forms -r:DrawLineUI.dll -out:go.exe DrawLineMain.cs
//To execute this program:
//     ./go.exe
//





using System;
using System.Windows.Forms;            //Needed for "Application" near the end of Main function.
public class Straight_line_main
{  public static void Main()
   {  System.Console.WriteLine("The Ninety Degree Turning program has begun.");
      DrawLine turn = new DrawLine();
      Application.Run(turn);
      System.Console.WriteLine("The Ninety Degree Turning program has ended.  Bye.");
   }//End of Main function
}//End of Straight_line_main class






//Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========0=========1=========2=========3**
