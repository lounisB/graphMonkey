/*
* 26/11/2006 - 17:02
*
* GraphMonkey - mono based graphing calculator
* Copyright (C) 2006 Lounis Bellabes
* nolius@users.sourceforge.net
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/

using System;
using Gtk;

public class Help_Form : Window
{
    public Help_Form () : base ("Help")
    {
        this.Move(300,100);
        this.SetDefaultSize (100, 300);
        this.Resizable = false;
        Gdk.Pixbuf icon = new Gdk.Pixbuf(null, "gm.png");
        this.Icon = icon; 
        Table tableLayout = new Table(1, 3, false);

        //blank
        Label label_blank4 = new Label("");
        tableLayout.Attach(label_blank4, 0, 1, 0, 1);
        
        //GraphMonkey
        Label label_monkey = new Label("GraphMonkey 1.7");
        tableLayout.Attach(label_monkey, 0, 1, 1, 2);
        
        //blank
        Label label_blank = new Label("");
        tableLayout.Attach(label_blank, 0, 1, 2, 3);
        
        //name
        Label label_name = new Label("     By Lounis Bellabes     ");
        tableLayout.Attach(label_name, 0, 1, 3, 4);
        
        //email
        Label label_email = new Label("   nolius@users.sourceforge.net   ");
        tableLayout.Attach(label_email, 0, 1, 4, 5);

        //GPL
        Label label_gpl = new Label("GPL license");
        tableLayout.Attach(label_gpl, 0, 1, 5, 6);
        
        //blank2
        Label label_blank2 = new Label("-----------------------------");
        tableLayout.Attach(label_blank2, 0, 1, 6, 7);        
        
        //squareroots
        Label label_squareroots = new Label("square root : sqrt()");
        tableLayout.Attach(label_squareroots, 0, 1, 8, 9);
        
        //powers square
        Label label_powerssquare  = new Label("powers : ^");
        tableLayout.Attach(label_powerssquare , 0, 1, 9, 10);        

        //sine
        Label label_sine = new Label("sine : sin()");
        tableLayout.Attach(label_sine, 0, 1, 10, 11);

        //cosine
        Label label_cosine = new Label("cosine : cos()");
        tableLayout.Attach(label_cosine, 0, 1, 11, 12);
    
        //tangent
        Label label_tangent = new Label("tangent : tan()");
        tableLayout.Attach(label_tangent, 0, 1, 12, 13);
        
        //arcsine
        Label label_arcsine = new Label("arcsine : asin()");
        tableLayout.Attach(label_arcsine, 0, 1, 13, 14);

        //arccosine
        Label label_arccosine = new Label("arccosine : acos()");
        tableLayout.Attach(label_arccosine, 0, 1, 14, 15);
        
        //arctangent
        Label label_arctangent = new Label("arctangent : atan()");
        tableLayout.Attach(label_arctangent, 0, 1, 15, 16);

        //hyperbolic sine
        Label label_hyperbolicsine = new Label("hyperbolic sine : sinh()");
        tableLayout.Attach(label_hyperbolicsine, 0, 1, 16, 17);        

        //hyperbolic cosine
        Label label_hyperboliccosine = new Label("hyperbolic cosine : cosh()");
        tableLayout.Attach(label_hyperboliccosine, 0, 1, 17, 18);
        
        //hyperbolic tangent
        Label label_hyperbolictangent = new Label("hyperbolic tangent : tanh()");
        tableLayout.Attach(label_hyperbolictangent, 0, 1, 18, 19);
        
        //natural logarithm
        Label label_logarithm = new Label("natural logarithm: ln()");
        tableLayout.Attach(label_logarithm, 0, 1, 19, 20);
        
        //base 10 logarithm
        Label label_logarithm10 = new Label("base 10 logarithm: log()");
        tableLayout.Attach(label_logarithm10, 0, 1, 20, 21);
        
        //exponential
        Label label_exponential = new Label("exponential : exp()");
        tableLayout.Attach(label_exponential, 0, 1, 21, 22);
        
        //absolute value
        Label label_absolutevalue = new Label("absolute value:  abs()");
        tableLayout.Attach(label_absolutevalue, 0, 1, 22, 23);
        
        //greatest integer
        Label label_greatestinteger = new Label("greatest integer: int()");
        tableLayout.Attach(label_greatestinteger, 0, 1, 23, 24);
        
        //blank3
        Label label_blank3 = new Label("");
        tableLayout.Attach(label_blank3, 0, 1, 24, 25);        

        tableLayout.ShowAll();
        this.Add (tableLayout);
        this.ShowAll ();
    }
}
