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

public class About_Form : Window
{
	public About_Form () : base ("About")
	{
		this.Move(200,200);
		this.SetDefaultSize (100, 300);
		this.Resizable = false;
		Gdk.Pixbuf icon = new Gdk.Pixbuf(null, "gm.png");
		this.Icon = icon; 
		Table tableLayout = new Table(3, 1, false);

		//Logo
		Gtk.Image im_graphmonkey = new Gtk.Image(new Gdk.Pixbuf(null,"graphmonkey.png"));
		tableLayout.Attach(im_graphmonkey, 0, 1, 0, 1); 
	
		//blank
		Label label_blank = new Label("");
		tableLayout.Attach(label_blank, 0, 1, 2, 3);
		
		//GraphMonkey
		Label label_monkey = new Label("GraphMonkey 1.7");
		tableLayout.Attach(label_monkey, 0, 1, 3, 4);
		
		//name
		Label label_name = new Label("     By Lounis Bellabes     ");
		tableLayout.Attach(label_name, 0, 1, 4, 5);

		//email
		Label label_email = new Label("   nolius@users.sourceforge.net   ");
		tableLayout.Attach(label_email, 0, 1, 6, 7);

		//GPL
		Label label_gpl = new Label("GPL license");
		tableLayout.Attach(label_gpl, 0, 1, 7, 8);
		
		tableLayout.ShowAll();
		this.Add (tableLayout);
		this.ShowAll ();
	}
}
