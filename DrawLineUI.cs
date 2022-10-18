//****************************************************************************************************************************
//Program name: "Ninety Degree Turn 3.0".  This programs accepts the coordinates of two points from the user, draws a        *
//straight line segment connecting them, and ball travels from the beginning end point to the terminal end point.            *
//Copyright (C) 2022  Floyd Holliday                                                                                         *
//                                                                                                                           *
//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License  *
//version 3 as published by the Free Software Foundation.                                                                    *
//This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied         *
//warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.     *
//A copy of the GNU General Public License v3 is available here:  <https://www.gnu.org/licenses/>.                           *
//****************************************************************************************************************************



﻿//Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========0=========1=========2=========3**
//Author: Juan Zaragoza
//Mail: zaragoza_9@csu.fullerton.edu

//Program name: Assignment #3
//Programming language: C Sharp
//Date development of program began: 2022-Sep-26
//Date of last update: 2022-Sep-26

//Purpose:  This programs demonstrate how an animated ball down a line using user input and the ball maintains constant speed.

//Files in project: DrawLineMain.cs, DrawLineUI.cs, r.sh

//This file's name: DrawLineUI.cs
//This file purpose: This module (file) defines the layout of the user interface
//Date last modified: 2022-Sep-26

//To compile straight-line-travel-user-interface.cs:
//      mcs -r:System -r:System.Windows.Forms -r:DrawLineUI.dll -out:go.exe DrawLineMain.cs

//Suggestion to user: Feel free to change the values in the first five parameters and discover the effects of those changes.
//For instance, modify the values in refresh_clock_rate, ball_linear_speed, motion_clock_rate.


using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class DrawLine : Form
{  //Declare constants ordered by the specification document
   private const int ui_width = 1000;
   private const int ui_height = 800;
   private const double refresh_clock_rate = 24.8;  //Hertz = tics per second
   private const double motion_clock_rate = 57.3;   //Hertz = tics per second
   private const double ball_linear_speed = 98.3;   //pixel per second
   private Size maxframesize = new Size(ui_width,ui_height);
   private Size minframesize = new Size(ui_width,ui_height);

   // Text input lables
   private Label startPoint = new Label();
   private Label endPoint = new Label();

   //Location
   private Label location = new Label();
   private Label curr_location = new Label();

   // Text inputs
   private TextBox v1_x = new TextBox();
   private TextBox v1_y = new TextBox();
   private TextBox v2_x = new TextBox();
   private TextBox v2_y = new TextBox();

   //Boolean
   private bool flag = true;

   //Create line Height
   private double lineHeight;

   //Declare the top panel and related attributes
   private Panel header = new Panel();
   private int header_height = 70;
   private Label ninety_degree_label = new Label();

   //Declare the graphic panel and related attributes.
   private Graphicpanel graph_panel = new Graphicpanel();

   //Declare the controller panel
   private Panel controler = new Panel();

   //Declare objects (controls) to be placed on the controller panel.
   private Button go_and_pause = new Button();
   private Button start = new Button();
   private Label elapsed_time_legend = new Label();
   private Label elapsed_time_value = new Label();
   private Button quit = new Button();

   // Declare updated points
   private int v1_X = 0;
   private int v1_Y = 0;
   private int v2_X = 0;
   private int v2_Y = 0;

   //Declare vertices of the polygonal line where the ball will travel.
   private static Point v1;
   private static Point v2;

   //Declare the properties of the moving ball
   enum Compass {Up,Down};
   Compass current_direction = Compass.Down;
   private static double ball_radius = 8.65;
   private static double ball_center_x;
   private static double ball_center_y;
   private static double ball_corner_x;
   private static double ball_corner_y;
   private static double Δx;
   private static double Δy;
   private static double ball_speed_pixel_per_tic;

   //Declare properties of the clock controlling the ball.
   private int ball_clock_interval = (int)System.Math.Round(1000.0/motion_clock_rate);
   private static System.Timers.Timer ball_clock = new System.Timers.Timer();

   //Declare the refresh clock.
   private int refresh_clock_interval = (int)System.Math.Round(1000.0/refresh_clock_rate);
   private static System.Timers.Timer user_interface_refresh_clock = new System.Timers.Timer();

   //Auxiliary data
   private bool both_clocks_are_stopped = true;
   private int tic_counter = 0;

   //Begin constructor of this class
   public DrawLine()      //<====================
   {//Set properties of the form
    Width = ui_width;
    Height = ui_height;
    MaximumSize = maxframesize;
    MinimumSize = minframesize;
    BackColor = Color.Navy;
    Text = "Assignment #3";
    System.Console.WriteLine("form height = {0}, form width = {1}.", Height, Width);

    //Text label inputs
    startPoint.Text = "Start";
    startPoint.Size = new Size(45,30);
    startPoint.Location = new Point(75, 15);
    startPoint.Font = new Font("Arial",12,FontStyle.Bold);

    endPoint.Text = "Finish";
    endPoint.Size = new Size(45,30);
    endPoint.Location = new Point(175, 15);
    endPoint.Font = new Font("Arial",12,FontStyle.Bold);

    //Location label status
    location.Text = "Location";
    location.Size = new Size(70,30);
    location.Location = new Point(640, 15);
    location.Font = new Font("Arial",12,FontStyle.Bold);

    curr_location.Text = "(0, 0)";
    curr_location.Size = new Size(140,40);
    curr_location.Location = new Point(600, 45);
    curr_location.Font = new Font("Arial",12,FontStyle.Bold);
    curr_location.TextAlign = ContentAlignment.MiddleCenter;
    curr_location.BackColor = Color.Navy;

    //Text input sizes
    v1_x.Size = new Size(40,40);
    v1_x.Text="";
    v1_y.Size = new Size(40,40);
    v1_y.Text="";
    v2_x.Size = new Size(40,40);
    v2_x.Text="";
    v2_y.Size = new Size(40,40);
    v2_y.Text="";

    //Set up the top panel known as the header panel.
    header.Size = new Size(Width,header_height);
    header.Text = "Assignment 3";
    header.BackColor = Color.Navy;
    header.Location = new Point(0,0);
    System.Console.WriteLine("header panel height = {0}, header panel width = {1}.", header.Height, header.Width);

    //Set up the label to be placed in the top panel.
    ninety_degree_label.Font = new Font("Arial",15,FontStyle.Bold);
    ninety_degree_label.Size = new Size(300,25);
    ninety_degree_label.Text = "Assigment 3 by Juan";
    ninety_degree_label.TextAlign = ContentAlignment.MiddleCenter;
    //Next statement: find a point that centers the placement of ninety_degree_label in the center of header_panel.
    ninety_degree_label.Location = new Point((header.Width-ninety_degree_label.Width)/2,(header.Height-ninety_degree_label.Height)/2);
    header.Controls.Add(ninety_degree_label);

    //Set up the graphically enabled central panel
    graph_panel.Location = new Point(0,header.Height);
    graph_panel.BackColor = Color.LightGray;
    graph_panel.Width = Width;   //The graph panel has width equal to the width of the form
    graph_panel.Height = 550;
    System.Console.WriteLine("graph panel height = {0}, graph panel width = {1}.", graph_panel.Height, graph_panel.Width);

    //Set up the polygonal path in which the ball travels
    v1 = new Point(0,0);
    v2 = new Point(0,0);

    //Set up the control panel containing buttons and other display devices.
    controler.Width = Width;
    controler.Height = Height-header.Height-graph_panel.Height;
    controler.BackColor = Color.Orange;
    controler.Location = new Point(0,header.Height+graph_panel.Height);
    System.Console.WriteLine("control panel height = {0}, control panel width = {1}.", controler.Height, controler.Width);

    //Set up the go_and_pause button and attach it to the controller panel
    start.Size = new Size(90,40);
    start.Text = "Start";
//    go_and_pause_button.Location = new Point(control_panel.Width/15,control_panel.Height/4);
    start.Location = new Point(350,45);
    start.ForeColor = ForeColor = Color.White;
    start.BackColor = Color.Navy;
    start.Click += new EventHandler(Start);
    start.Enabled = true;
    start.TextAlign = ContentAlignment.MiddleCenter;
    controler.Controls.Add(start);

    //Set up the go_and_pause button and attach it to the controller panel
    go_and_pause.Size = new Size(90,40);
    go_and_pause.Text = "Go";
//    go_and_pause_button.Location = new Point(control_panel.Width/15,control_panel.Height/4);
    go_and_pause.Location = new Point(350,45);
    go_and_pause.ForeColor = ForeColor = Color.White;
    go_and_pause.BackColor = Color.Navy;
    go_and_pause.Click += new EventHandler(Go_pause);
    //go_and_pause.Enabled = true;
    go_and_pause.TextAlign = ContentAlignment.MiddleCenter;
    //controler.Controls.Add(go_and_pause);

    //Text input labels
    controler.Controls.Add(startPoint);
    controler.Controls.Add(endPoint);

    //Location labels
    controler.Controls.Add(location);
    controler.Controls.Add(curr_location);

    //Text inputs locations
    v1_x.Location = new Point(50,60);
    controler.Controls.Add(v1_x);

    v1_y.Location = new Point(100,60);
    controler.Controls.Add(v1_y);

    v2_x.Location = new Point(150,60);
    controler.Controls.Add(v2_x);

    v2_y.Location = new Point(200,60);
    controler.Controls.Add(v2_y);

    //Set up the quit button and attach it to the controller panel
    quit.Size = new Size(90,40);
    quit.Text = "Quit";
    //quit.Location = new Point(control_panel.Width*9/10,control_panel.Height/4);
    quit.Location = new Point(850,45);
    quit.BackColor = Color.Red;
    quit.ForeColor = ForeColor = Color.White;
    quit.TextAlign = ContentAlignment.MiddleCenter;
    quit.Click += new EventHandler(Close_window);
    controler.Controls.Add(quit);

    //Prepare the refresh clock.  A button will start this clock ticking.
    user_interface_refresh_clock.Enabled = false;  //Initially this clock is stopped.
    user_interface_refresh_clock.Interval = refresh_clock_interval;
    user_interface_refresh_clock.Elapsed += new ElapsedEventHandler(Refresh_user_interface);

   //Prepare the ball clock.  A button will start this clock ticking.
    ball_clock.Enabled = false;  //Initially this clock is stopped.
    ball_clock.Interval = ball_clock_interval;
    ball_clock.Elapsed += new ElapsedEventHandler(Update_ball_coordinates);

    //Attach various elements (controls) to the form.
    Controls.Add(header);
    Controls.Add(graph_panel);
    Controls.Add(controler);

    //Change units of speed of ball
    ball_speed_pixel_per_tic = ball_linear_speed/motion_clock_rate;  //pixels per tic.

    //Make sure this UI window appears in the center of the monitor.
    CenterToScreen();

   }//End of constructor                               //<=======================

   //Handler for the ball clock.
   protected void Update_ball_coordinates(System.Object sender, ElapsedEventArgs even)
   {//This function is called each time the ball_clock makes one tic.
    // Write the coordinates of the ball to the console
    curr_location.Text = "(" + string.Format("{0:0.00}", (ball_center_x+Δx)) + ", " + string.Format("{0:0.00}", (ball_center_y+Δy)) + ")";
    //System.Console.WriteLine("Δy : " + Δy );
    tic_counter++;
    switch (current_direction)
    {  case Compass.Down:
                // ball will go down once the x has reached the first point
                 if(ball_center_x+Δx >= v2.X)
                      {ball_center_x += Δx;    //<= Ball moves directly Down.
                       ball_center_y += Δy;    //However, Δy is 0.0 when moving Down.
                       Δx = ((v2.X - v1.X)*5)/(lineHeight * (1));
                       Δy = ((v2.Y - v1.Y)*5)/(lineHeight * (1));
                      }
                else //ball_center_x+Δx < v2.X
                     {
                      current_direction = Compass.Up;
                     }//End of else
                 break;
        case Compass.Up:
                 //System.Console.WriteLine("There is no road going Up.  This program will end.");
                 // ball will go up once the x has reached the first point
                if (Math.Abs(ball_center_x+Δx) <= v1_X) {
                  ball_center_x += Δx;
                  ball_center_y += Δy;
                  Δx = ((v2.X - v1.X)*5)/(lineHeight * (-1));
                  Δy = ((v2.Y - v1.Y)*5)/(lineHeight * (-1));
                } else {
                  current_direction = Compass.Down;
                }
                 //current_direction = Compass.Up;

                 graph_panel.Invalidate();
                 break;
        default:
                 System.Console.WriteLine("You have a serious bug in this program.  Try again");
                 break;
    }//End of switch
   }//End of method Update_ball_coordinates

   //Handler for the Start button
   protected void Start(System.Object sender, EventArgs even)
   {
     v1_X = int.Parse(v1_x.Text);
     v1_Y = int.Parse(v1_y.Text);
     v2_X = int.Parse(v2_x.Text);
     v2_Y = int.Parse(v2_y.Text);
     // set the points
     v1 = new Point(v1_X, v1_Y);
     v2 = new Point(v2_X, v2_Y);
     // set the ball position
     ball_center_x = v1.X;
     ball_center_y = v1.Y;
    // math to draw the line
     lineHeight = Math.Sqrt(Math.Pow(v2.Y - v1.Y, 2) + Math.Pow(v2.X - v1.X, 2));
     //Set the values for Δx and Δy for initial direction "Down".
     Δx = ((v2.X - v1.X)*5)/lineHeight;
     Δy = ((v2.Y - v1.Y)*5)/lineHeight;


     start.Enabled = false;
     controler.Controls.Remove(start);

     go_and_pause.Enabled = true;
     controler.Controls.Add(go_and_pause);
   }

   //Handler for the Go_and_pause button
   protected void Go_pause(System.Object sender, EventArgs even)
   {
     if(both_clocks_are_stopped)
         {//Change the message on the button
          go_and_pause.Text = "Pause";

          //Start the refresh clock running.
          user_interface_refresh_clock.Enabled = true;

          //Start the animation clock running.
          ball_clock.Enabled = true;
         }
    else
         {
           // if flag is true that means that the coordinates have been set

           // set flag to false
           flag = false;
           //Stop the refresh clock.
          user_interface_refresh_clock.Enabled = false;

          //Stop the animation clock running.
          ball_clock.Enabled = false;

          //Change the message on the button
          go_and_pause.Text = "Go";

         }//End of if
    //Toggle the variable both_clocks_are_stopped to be its negative
    both_clocks_are_stopped = !both_clocks_are_stopped;
   }//End of event handler Go_pause

   //Declare the handler method for the refresh clock
   protected void Refresh_user_interface(System.Object sender, ElapsedEventArgs even)
   {//elapsed_time_value.Text = String.Format("{0:000.00}",(double)tic_counter/motion_clock_rate);
    graph_panel.Invalidate();
   }//End of event handler Refresh_user_interface

   //Declare the handler method of the Quit button.
   protected void Close_window(System.Object sender, EventArgs even)
   {System.Console.WriteLine("This program will close the UI window and stop execution.");
    Close();
   }//End of event handler Close_window


    //Define the Graphic Panel
    public class Graphicpanel: Panel
    {private Pen schaffer = new Pen(Color.Orange,2);
        protected override void OnPaint(PaintEventArgs ee)
        {   Graphics graph = ee.Graphics;
            graph.DrawLine(schaffer,v1.X,v1.Y,v2.X,v2.Y);
            ball_corner_x = ball_center_x - ball_radius;
            ball_corner_y = ball_center_y - ball_radius;
            graph.FillEllipse(Brushes.Navy,
                              (int)System.Math.Round(ball_corner_x),
                              (int)System.Math.Round(ball_corner_y),
                              (int)System.Math.Round(2.0*ball_radius),
                              (int)System.Math.Round(2.0*ball_radius));

            base.OnPaint(ee);
        }//End of method OnPaint
    }//End of class Graphicpanel

}//End of class DrawLine

//======End of file=============================================================
