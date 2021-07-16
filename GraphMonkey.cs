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

public class GraphMonkey : Window {
	public Entry entry_eq1;
	public Entry entry_eq2;
	public Entry entry_eq3;
	
	public Entry entry_x_value;
	public Entry entry_result;
	
	public Entry entry_xmin;
	public Entry entry_xmax;
	public Entry entry_ymin;
	public Entry entry_ymax;
	public Entry entry_graduation;
	
	
	public double x_min;
	public double x_max;
	public double y_min;
	public double y_max;
	public double graduation;
	
	public Label label_info;
		
	public GraphMonkey () : base ("GraphMonkey")
	{
		this.Move(100,100);
		this.SetDefaultSize (400, 300);
		this.DeleteEvent += new DeleteEventHandler (OnMyWindowDelete);
		this.Resizable = false;
		//this.ShowAll ();
		
		Gdk.Pixbuf icon = new Gdk.Pixbuf(null, "gm.png");
		this.Icon = icon; 	
		
		VBox vbox = new VBox(false,2);
		MenuBar bar = new MenuBar ();
		
		Menu file_menu = new Menu ();
		MenuItem file_menu_item = new MenuItem ("_File");
		file_menu_item.Submenu = file_menu;
		
		ImageMenuItem file_exit = new ImageMenuItem("_Quit");
		file_exit.Image = new Gtk.Image(Gtk.Stock.Quit, Gtk.IconSize.Menu);
		file_exit.Activated += new EventHandler (exit_cb);
		file_menu.Append (file_exit);
		bar.Append (file_menu_item);

		Menu help_menu = new Menu ();
		MenuItem help_menu_item = new MenuItem ("_Help");
		help_menu_item.Submenu = help_menu;
		
		ImageMenuItem help_help = new ImageMenuItem("_Help");
		help_help.Image = new Gtk.Image(Gtk.Stock.Help, Gtk.IconSize.Menu);
		help_help.Activated += new EventHandler (help_cb);
		help_menu.Append (help_help);
			
		ImageMenuItem help_about = new ImageMenuItem("_About");
		help_about.Image = new Gtk.Image(Gtk.Stock.DialogInfo, Gtk.IconSize.Menu);
		help_about.Activated += new EventHandler (about_cb);
		help_menu.Append (help_about);
		
		bar.Append (help_menu_item);
		bar.ShowAll ();
		
		vbox.PackStart(bar, true, true, 0);
		
		
		// create a table 6 on 10
		Table tableLayout = new Table(6, 10, false);

		//equation 1
		Label label_y1 = new Label("Function 1 :   y   =");
		tableLayout.Attach(label_y1, 0, 1, 0, 1);						
		
		entry_eq1 = new Entry ("");
		tableLayout.Attach(entry_eq1, 1, 2, 0, 1);
		entry_eq1.Activated += new EventHandler(button_trace_click);
		
		//equation 2
		Label label_y2 = new Label("Function 2 :   y   =");
		tableLayout.Attach(label_y2, 0, 1, 1, 2);				
				
		entry_eq2 = new Entry ("");
		tableLayout.Attach(entry_eq2, 1, 2, 1, 2);
		entry_eq2.Activated += new EventHandler(button_trace_click);
		
		//equation 3						
		Label label_y3 = new Label("Function 3 :   y   =");
		tableLayout.Attach(label_y3, 0, 1, 2, 3);				
				
		entry_eq3 = new Entry ("");
		tableLayout.Attach(entry_eq3, 1, 2, 2, 3);
		entry_eq3.Activated += new EventHandler(button_trace_click);
		
		Button button_trace = new Button (" Trace ! ");
		button_trace.Clicked += new EventHandler (button_trace_click);
		tableLayout.Attach(button_trace, 3, 4, 0, 3);
																												
		//------------------------------------------------
		
		// f(x)
		Label label_x_value = new Label(" x value for Fn 1 :  ");
		tableLayout.Attach(label_x_value, 0, 1, 3, 4);			
		
		entry_x_value = new Entry ("");
		tableLayout.Attach(entry_x_value, 1, 2, 3, 4);
		entry_x_value.Activated += new EventHandler(button_result_click);
		
		//Label label_egal = new Label("=");
		//tableLayout.Attach(label_egal, 3, 4, 1, 2);

		Button button_result = new Button (" f(x) = ");
		button_result.Clicked += new EventHandler (button_result_click);
		tableLayout.Attach(button_result, 2, 3, 3, 4);	
		
		entry_result = new Entry ("");
		entry_result.IsEditable = false;
		tableLayout.Attach(entry_result, 3, 4, 3, 4);
		
		//------------------------------------------------
		
		// range
		Label label_range = new Label("");
		tableLayout.Attach(label_range, 0, 1, 4, 5);
		
		//xmin
		Label label_xmin = new Label("Xmin   =");
		tableLayout.Attach(label_xmin, 0, 1, 5, 6);
				
		entry_xmin = new Entry ("-10");
		tableLayout.Attach(entry_xmin, 1, 2, 5, 6);
		entry_xmin.Activated += new EventHandler(button_trace_click);
		
		//xmax
		Label label_xmax = new Label("Xmax   =");
		tableLayout.Attach(label_xmax, 0, 1, 6, 7);		
				
		entry_xmax = new Entry ("10");
		tableLayout.Attach(entry_xmax, 1, 2, 6, 7);
		entry_xmax.Activated += new EventHandler(button_trace_click);
		
		//ymin
		Label label_ymin = new Label("Ymin   =");
		tableLayout.Attach(label_ymin, 0, 1, 7, 8);
				
		entry_ymin = new Entry ("-10");
		tableLayout.Attach(entry_ymin, 1, 2, 7, 8);
		entry_ymin.Activated += new EventHandler(button_trace_click);
		
		//ymax
		Label label_ymax = new Label("Ymax   =");
		tableLayout.Attach(label_ymax, 0, 1, 8, 9);
				
		entry_ymax = new Entry ("10");
		tableLayout.Attach(entry_ymax, 1, 2, 8, 9);
		entry_ymax.Activated += new EventHandler(button_trace_click);
		
		//graduation
		Label label_graduation = new Label("Scale =");
		tableLayout.Attach(label_graduation, 0, 1, 9, 10);
				
		entry_graduation = new Entry ("1");
		tableLayout.Attach(entry_graduation, 1, 2, 9, 10);
		entry_graduation.Activated += new EventHandler(button_trace_click);

		//initialize range
		Button button_initialize_range = new Button ("Initialize Range");
		button_initialize_range.Clicked += new EventHandler (button_initialize_range_click);
		tableLayout.Attach(button_initialize_range, 3, 4, 8, 9);
		
		//initialize
		Button button_initialize_all = new Button ("Initialize All");
		button_initialize_all.Clicked += new EventHandler (button_initialize_all_click);
		tableLayout.Attach(button_initialize_all, 3, 4, 7, 8);
				
		//help
		Button button_about = new Button ("Help");
		button_about.Clicked += new EventHandler (button_about_click);
		tableLayout.Attach(button_about, 3, 4, 9, 10);
		
		// info
		label_info = new Label("Ready!");
		tableLayout.Attach(label_info, 3, 4, 5, 6);
		
		vbox.PackStart(tableLayout,true,true,0);
		
		tableLayout.ShowAll();
		//this.Add (tableLayout);
		this.Add(vbox);
		this.ShowAll ();
		
		//  range initialization
		x_min = -10f;
		x_max = 10f;
		y_min = -10f;
		y_max = 10f;
		graduation = 1f;
	}
	
	void button_result_click (object o, EventArgs args)
	{
		try{
			string equation = this.entry_eq1.Text;
			string string_x = entry_x_value.Text;
			//string_x = string_x.Replace(".", ".");
		
			double x;
			if (equation != ""){
				if (string_x != ""){
					decimal decimal_x = Convert.ToDecimal(string_x);
					x = (double) decimal_x;
				}
				else{
					x = 0f; 
				}
			
				operation op =new operation(equation);

				op.correct();
				double res =op.compute(x);
				this.entry_result.Text=Convert.ToString(res);
					
				double res_verif = res;	
				if(Double.IsNaN(res_verif)){
					this.entry_result.Text = "NaN";
					label_info.Text = "Error!";
				}
				else{
					label_info.Text = "Ready!";
				}	
			}
		}
		catch(Exception ex)
		{
				label_info.Text = "Error!";
				this.entry_result.Text = "";	
		}	
	}
	
	void button_trace_click (object o, EventArgs args)
	{
		try{
			if( ( this.entry_eq1.Text != "" ||  this.entry_eq2.Text != "" || this.entry_eq3.Text != "") && entry_xmin.Text != "" && entry_xmax.Text != "" && entry_ymin.Text != "" && entry_ymax.Text != "" && entry_graduation.Text != "" ){
			
				decimal decimal_xmin = Convert.ToDecimal(entry_xmin.Text);
				x_min = (double) decimal_xmin;
				decimal decimal_xmax = Convert.ToDecimal(entry_xmax.Text);
				x_max = (double) decimal_xmax;
				decimal decimal_ymin = Convert.ToDecimal(entry_ymin.Text);
				y_min = (double) decimal_ymin;			
				decimal decimal_ymax = Convert.ToDecimal(entry_ymax.Text);
				y_max = (double) decimal_ymax;
				decimal decimal_graduation = Convert.ToDecimal(entry_graduation.Text);
				graduation = (double) decimal_graduation;
						
				if( x_min < x_max && y_min < y_max && graduation >0){
			
					Screen_Form screen = new Screen_Form(entry_eq1.Text, entry_eq2.Text, entry_eq3.Text, x_min, x_max, y_min, y_max, graduation);
					label_info.Text = "Ready!";
				}
				else{
					label_info.Text = "Range error!";
				}
				
			}
			else{
				if(this.entry_eq1.Text == "" &&  this.entry_eq2.Text == "" &&  this.entry_eq3.Text == ""){
					label_info.Text = "No equation!";
				}
				else{
					label_info.Text = "Range error!";
				}
			}
		}	
		catch(Exception ex)
		{
				label_info.Text = "Range error!";
		}				
	}
	
	void button_initialize_range_click (object o, EventArgs args)
	{	
		entry_xmin.Text = "-10";
		entry_xmax.Text = "10";
		entry_ymin.Text = "-10";
		entry_ymax.Text = "10";
		entry_graduation.Text = "1";
	}
	
	void button_initialize_all_click (object o, EventArgs args)
	{
		entry_eq1.Text = "";
		entry_eq2.Text = "";
		entry_eq3.Text = "";
	
		entry_x_value.Text = "";
		entry_result.Text = "";
		
		entry_xmin.Text = "-10";
		entry_xmax.Text = "10";
		entry_ymin.Text = "-10";
		entry_ymax.Text = "10";
		entry_graduation.Text = "1";
	}

	void button_about_click (object o, EventArgs args)
	{	
		Help_Form help = new Help_Form();
		help.Show();
	}		
	
	void OnMyWindowDelete (object o, DeleteEventArgs args)
	{
		Application.Quit ();
	}
	
	void exit_cb (object o, EventArgs args)
	{
		Application.Quit ();
	}
	
	void help_cb (object o, EventArgs args)
	{
		Help_Form help = new Help_Form();
		help.Show();
	}
	
	void about_cb (object o, EventArgs args)
	{
		About_Form about = new About_Form();
		about.Show();
	}
}
