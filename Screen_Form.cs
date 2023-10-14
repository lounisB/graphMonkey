/*
* 26/11/2006 - 17:02
*
* GraphMonkey - mono based graphing calculator
* Copyright (C) 2006-2023 Lounis Bellabes
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
using Cairo;
using Color = Cairo.Color;

class Screen_Form : DrawingArea
{
    private Gtk.Window win;
    private HBox hbox;
    private VBox vbox;

    public string equation;
    public string equation2;
    public string equation3;
        
    public double x_min;
    public double x_max;
    public double y_min;
    public double y_max;
    public double graduation;
        
    // colors
    private Color red = new Color(0xff, 0, 0);
    private Color green = new Color(0 , 0xff, 0);
    private Color blue = new Color(0 , 0, 0xff);
    private Color black = new Color(0 , 0, 0);
    private Color white = new Color(0xff, 0xff, 0xff);
    private Color gray = new Color(0.85, 0.85, 0.85);
    private Color cyan = new Color(0, 0xff, 0xff);
    private Color orange = new Color(0xff, 140, 0);
    private Color color_a = new Color(0xff, 0, 0xff);

    // saving results
    public bool first_draw = true;
    private double[] tab_result1_x = new double[501];
    private double[] tab_result1_y = new double[501];
    private double[] tab_result2_x = new double[501];
    private double[] tab_result2_y = new double[501];
    private double[] tab_result3_x = new double[501];
    private double[] tab_result3_y = new double[501];
    private double[] tab_result_derivative_x = new double[501];
    private double[] tab_result_derivative_y = new double[501];
    private double[] tab_result_secondderivative_x = new double[501];
    private double[] tab_result_secondderivative_y = new double[501];
    
    public Label label_coord;
    
    // status of options
    private bool black_screen;
    private bool grid;
    
    // status of calculus
    private bool simpson;
    private bool derivative;
    private bool secondderivative;
    
    private bool extension_exist;
    private bool extension_show;
    private Statusbar statusbar;

    // graph context
    private Context context;

    public Screen_Form (string equation1, string equation21, string equation31, double x_min1, double x_max1, double y_min1, double y_max1, double graduation1)
    {
        this.WidthRequest = 501;
        this.HeightRequest = 501;
        vbox = new VBox(false,3);
        
        MenuBar bar = new MenuBar ();
        Menu calculus_menu = new Menu ();
        MenuItem calculus_menu_item = new MenuItem ("_Calculus");
        calculus_menu_item.Submenu = calculus_menu;
        
        ImageMenuItem simpson_item = new ImageMenuItem("_Net Area Under Fn 1 [Xmin, Xmax]");
        simpson_item.Image = new Gtk.Image(Gtk.Stock.GoDown, Gtk.IconSize.Menu);
        simpson_item.Activated += new EventHandler(simpson_cb);
        calculus_menu.Append(simpson_item);

        ImageMenuItem averagevalue_item = new ImageMenuItem("A_verage Value of Fn 1 [Xmin, Xmax]");
        averagevalue_item.Image = new Gtk.Image(Gtk.Stock.Index, Gtk.IconSize.Menu);
        averagevalue_item.Activated += new EventHandler(averagevalue_cb);
        calculus_menu.Append(averagevalue_item);

        ImageMenuItem derivative_item = new ImageMenuItem("Graph _Derivative of Fn 1");
        derivative_item.Image = new Gtk.Image(Gtk.Stock.Execute,Gtk.IconSize.Menu);
        derivative_item.Activated += new EventHandler(derivative_cb);
        calculus_menu.Append(derivative_item);

        ImageMenuItem secondderivative_item = new ImageMenuItem("Graph _Second Derivative of Fn 1");
        secondderivative_item.Image = new Gtk.Image(Gtk.Stock.Execute,Gtk.IconSize.Menu);
        secondderivative_item.Activated += new EventHandler(secondderivative_cb);
        calculus_menu.Append(secondderivative_item);

        ImageMenuItem clear_item = new ImageMenuItem("_Clear");
        clear_item.Image = new Gtk.Image(Gtk.Stock.Clear,Gtk.IconSize.Menu);
        clear_item.Activated += new EventHandler(clear_cb);
        calculus_menu.Append(clear_item);

        bar.Append (calculus_menu_item);

        Menu option_menu = new Menu ();
        MenuItem option_menu_item = new MenuItem ("_Options");
        option_menu_item.Submenu = option_menu;
        
        ImageMenuItem wbscreen_item = new ImageMenuItem("White/Black _screen");
        wbscreen_item.Image = new Gtk.Image(Gtk.Stock.Preferences, Gtk.IconSize.Menu);
        wbscreen_item.Activated += new EventHandler (wbscreen_cb);
        option_menu.Append (wbscreen_item);
            
        ImageMenuItem grid_item = new ImageMenuItem("On/Off _grid");
        grid_item.Image = new Gtk.Image(Gtk.Stock.Preferences, Gtk.IconSize.Menu);
        grid_item.Activated += new EventHandler (grid_cb);
        option_menu.Append (grid_item);
        
        bar.Append (option_menu_item);
        bar.ShowAll ();
        vbox.PackStart(bar,true,true,0);
        
        vbox.PackStart(this,true,true,0);
        hbox = new HBox(false,5);
        
        Image image_GoUp = new Gtk.Image(Gtk.Stock.GoUp , Gtk.IconSize.Button);
        Button button_zoom_up = new Button (image_GoUp);
        button_zoom_up.Clicked += new EventHandler (button_zoom_up_click);
        hbox.PackStart(button_zoom_up,false,false,0);
        
        Image image_GoDown = new Gtk.Image(Gtk.Stock.GoDown, Gtk.IconSize.Button);
        Button button_zoom_down = new Button (image_GoDown);
        button_zoom_down.Clicked += new EventHandler (button_zoom_down_click);
        hbox.PackStart(button_zoom_down,false,false,0);
        
        Image image_GoBack = new Gtk.Image(Gtk.Stock.GoBack , Gtk.IconSize.Button);
        Button button_zoom_left = new Button (image_GoBack );
        button_zoom_left.Clicked += new EventHandler (button_zoom_left_click);
        hbox.PackStart(button_zoom_left,false,false,0);
        
        Image image_GoForward = new Gtk.Image(Gtk.Stock.GoForward, Gtk.IconSize.Button);
        Button button_zoom_right = new Button (image_GoForward);
        button_zoom_right.Clicked += new EventHandler (button_zoom_right_click);
        hbox.PackStart(button_zoom_right,false,false,0);
        
        Image image_ZoomIn = new Gtk.Image(Gtk.Stock.ZoomIn, Gtk.IconSize.Button);
        Button button_zoom_in = new Button (image_ZoomIn);
        button_zoom_in.Clicked += new EventHandler (button_zoom_in_click);
        hbox.PackStart(button_zoom_in,false,false,0);
        
        Image image_ZoomOut = new Gtk.Image(Gtk.Stock.ZoomOut, Gtk.IconSize.Button);
        Button button_zoom_out = new Button (image_ZoomOut);
        button_zoom_out.Clicked += new EventHandler (button_zoom_out_click);
        hbox.PackStart(button_zoom_out,false,false,0);
        
        Image image_Zoom100 = new Gtk.Image(Gtk.Stock.Zoom100, Gtk.IconSize.Button);
        Button button_zoom_100 = new Button (image_Zoom100);
        button_zoom_100.Clicked += new EventHandler (button_zoom_100_click);
        hbox.PackStart(button_zoom_100,false,false,0);
        
        // Image image_Save = new Gtk.Image(Gtk.Stock.Save, Gtk.IconSize.Button);
        // Button button_save = new Button (image_Save);
        // button_save.Clicked += new EventHandler (button_save_click);
        // hbox.PackStart(button_save,false,false,0);
        
        label_coord = new Label ("( x ; y)");
        hbox.PackEnd(label_coord,false,false,0);
        
        this.MotionNotifyEvent += new MotionNotifyEventHandler(on_mouse_move_graph);
        this.LeaveNotifyEvent += new LeaveNotifyEventHandler(on_mouse_leave_graph);
        this.Events = Gdk.EventMask.ExposureMask | Gdk.EventMask.LeaveNotifyMask | Gdk.EventMask.ButtonPressMask | Gdk.EventMask.PointerMotionMask | Gdk.EventMask.PointerMotionHintMask;
        
        vbox.PackStart(hbox,true,true,0);
        win = new Gtk.Window ("Screen");
        Gdk.Pixbuf icon = new Gdk.Pixbuf(null, "gm.png");
        win.Icon = icon;                                                                                                                                                                                                                   
        win.SetDefaultSize (501, 501);
        win.Resizable = false;
          
        //this.ExposeEvent += OnExposed;
        this.Drawn += OnDrawn;
                    
        win.Add (vbox);
        win.ShowAll ();
                   
        this.equation = equation1;
        this.equation2 = equation21; 
        this.equation3 = equation31;  
        this.x_min = x_min1;
        this.x_max = x_max1;
        this.y_min = y_min1;
        this.y_max = y_max1;
        this.graduation = graduation1;
        
        this.black_screen = true;
        this.grid = false;
        
        this.simpson = false;
        this.derivative = false;
        this.secondderivative = false;
        
        this.extension_exist = false;
        this.extension_show = false;
    }
 
    void OnDrawn (object o, DrawnArgs args)
    {
        this.context = args.Cr;
        trace_all();
    }
    
    void trace_all()
    {        
        clear_graph();
        if(simpson)
            trace_simpson();
        trace_axe();
        
        if(derivative)
            trace_derivative();
        if(secondderivative)
            trace_secondderivative();

        if(first_draw){
            trace_equation();
            first_draw = false;
        }
        else
            trace_equation_buffer();
    }
 
     void clear_graph()
     {
        if (this.black_screen)
            context.SetSourceColor(black);
        else
            context.SetSourceColor(white);

        context.Rectangle(x: -10, y: -10, width: 520, height: 520);
        context.Fill();
     }
 
     // draw axes
    void trace_axe()
    {
        int x0 = (int) Math.Round(trans_x(0f));    
        int y0 = (int) Math.Round(trans_y(0f));
        int xmax = (int) Math.Round(trans_x(x_max));
        int xmin = (int) Math.Round(trans_x(x_min));
        int ymax = (int) Math.Round(trans_y(y_max));
        int ymin = (int) Math.Round(trans_y(y_min));
            
        // graduation for x
        if (x_max > 0){    
            //0 to max        
            for(double i2 = trans_x(0f); i2 <= trans_x(x_max); i2 = i2 + graduation * 500/(x_max - x_min)){
                if (this.grid){
                    context.SetSourceColor(gray);
                    context.MoveTo((int) Math.Round(i2), ymin);
                    context.LineTo((int) Math.Round(i2), ymax);
                    context.Stroke();
                }
                context.SetSourceColor(blue);
                context.MoveTo((int) Math.Round(i2), y0);
                context.LineTo((int) Math.Round(i2), y0-5);
                context.Stroke();
            }
        }
        if (x_min<0){
            //0 to min
            for(double i2 = trans_x(0f); i2 >= trans_x(x_min); i2 = i2 - graduation * 500/(x_max - x_min) ){
                if (this.grid){
                    context.SetSourceColor(gray);
                    context.MoveTo((int) Math.Round(i2), ymin);
                    context.LineTo((int) Math.Round(i2), ymax);
                    context.Stroke();
                }
                context.SetSourceColor(blue);
                context.MoveTo((int) Math.Round(i2), y0);
                context.LineTo((int) Math.Round(i2), y0-5);
                context.Stroke();
            }
        }                    
                
        // graduation for y
        if (y_min<0){
            //0 to min
            for(double i2 = trans_y(0f); i2 <= trans_y(y_min); i2 = i2 + graduation * 500/(y_max - y_min) ){
                if (this.grid){
                    context.SetSourceColor(gray);
                    context.MoveTo((int) xmin, (int) Math.Round(i2));
                    context.LineTo((int) xmax, (int) Math.Round(i2));
                    context.Stroke();
                }
                context.SetSourceColor(blue);
                context.MoveTo(x0, (int) Math.Round(i2));
                context.LineTo(x0+5, (int) Math.Round(i2));
                context.Stroke();
            }
        }
        if (y_max>0){
            //0 to max    
            for(double i2 = trans_y(0f); i2 >= trans_y(y_max); i2 = i2 - graduation * 500/(y_max - y_min) ){
                if (this.grid){
                    context.SetSourceColor(gray);
                    context.MoveTo((int) xmin, (int) Math.Round(i2));
                    context.LineTo((int) xmax, (int) Math.Round(i2));
                    context.Stroke();
                }
                context.SetSourceColor(blue);
                context.MoveTo(x0, (int) Math.Round(i2));
                context.LineTo(x0+5, (int) Math.Round(i2));
                context.Stroke();
            }
        }
        
        //axe
        context.SetSourceColor(blue);
        context.MoveTo(xmin, y0);
        context.LineTo(xmax, y0);
        context.Stroke();
        context.MoveTo(x0, ymin);
        context.LineTo(x0, ymax);
        context.Stroke();
     }
     
     // draw 3 functions     
    void trace_equation()
    {
        Color color_eq;    
        for(int i_eq = 0; i_eq <3; i_eq++){
             
            operation op = new operation(equation);
            color_eq = red;
                      
            if(i_eq==1){
                op = new operation(equation2);                
                color_eq = color_a;
            }
            else if(i_eq==2){
                op = new operation(equation3);
                color_eq = green;
            }
                 
            op.correct();

            try{
                double xc_1 = x_min;
                double yc_1 = op.compute(x_min);            
                double xc_2;
                double yc_2;
                                                
                double pitch = (x_max-x_min)/500;
                double x_1;
                double y_1;
                double x_2;
                double y_2;
                
                // save results
                int i_save = 0;
                if(i_eq == 0){
                    tab_result1_x[i_save] = trans_x(xc_1);
                    tab_result1_y[i_save] = trans_y(yc_1);
                }
                else if(i_eq ==1){
                    tab_result2_x[i_save] = trans_x(xc_1);
                    tab_result2_y[i_save] = trans_y(yc_1);
                }
                else if(i_eq ==2){
                    tab_result3_x[i_save] = trans_x(xc_1);
                    tab_result3_y[i_save] = trans_y(yc_1);
                }                            
                                                
                for(double i_graph = x_min; i_graph <= x_max; i_graph = i_graph + pitch){
                    xc_2 = xc_1 + pitch;
                    yc_2 = op.compute(xc_2);
                                                            
                    x_1 = trans_x(xc_1);
                    y_1 = trans_y(yc_1);
                    x_2 = trans_x(xc_2);
                    y_2 = trans_y(yc_2);
                    
                    // save results
                    i_save++;
                    if(i_eq == 0){
                        tab_result1_x[i_save] = x_2;
                        tab_result1_y[i_save] = y_2;
                    }
                    else if(i_eq ==1){
                        tab_result2_x[i_save] = x_2;
                        tab_result2_y[i_save] = y_2;
                    }
                    else if(i_eq ==2){
                        tab_result3_x[i_save] = x_2;
                        tab_result3_y[i_save] = y_2;
                    }

                    if(!Double.IsNaN(yc_1) && !Double.IsNaN(yc_2)){
                        if((yc_1 > y_min && yc_1 < y_max) || (yc_2 > y_min && yc_2 < y_max)){
                            context.SetSourceColor(color_eq);
                            context.MoveTo((int) Math.Round(x_1), (int) Math.Round(y_1));
                            context.LineTo((int) Math.Round(x_2), (int) Math.Round(y_2));
                            context.Stroke();
                        }
                    }
                                                            
                    // next point
                    xc_1 = xc_2;
                    yc_1 = yc_2;
                    
                }
            }    
            catch(Exception ex)
            {    
            }         
        }        
    }
    
    // draw 3 functions using buffers
    void trace_equation_buffer()
    {
        for(int i =0; i < 500; i++){
            if(!Double.IsNaN(tab_result1_y[i]) && !Double.IsNaN(tab_result1_y[i+1])){
                if((tab_result1_y[i] > 0 && tab_result1_y[i] < 500) || (tab_result1_y[i+1] > 0 && tab_result1_y[i+1] < 500)){    
                    context.SetSourceColor(red);
                    context.MoveTo((int) Math.Round(tab_result1_x[i]), (int) Math.Round(tab_result1_y[i]));
                    context.LineTo((int) Math.Round(tab_result1_x[i+1]), (int) Math.Round(tab_result1_y[i+1]));
                    context.Stroke();
                }
            }
        }
        
        for(int i =0; i < 500; i++){
            if(!Double.IsNaN(tab_result2_y[i]) && !Double.IsNaN(tab_result2_y[i+1])){
                if((tab_result2_y[i] > 0 && tab_result2_y[i] < 500) || (tab_result2_y[i+1] > 0 && tab_result2_y[i+1] < 500)){   
                    context.SetSourceColor(color_a);
                    context.MoveTo((int) Math.Round(tab_result2_x[i]), (int) Math.Round(tab_result2_y[i]));
                    context.LineTo((int) Math.Round(tab_result2_x[i+1]), (int) Math.Round(tab_result2_y[i+1]));
                    context.Stroke(); 
                }
            }
        }
        
        for(int i =0; i < 500; i++){
            if(!Double.IsNaN(tab_result3_y[i]) && !Double.IsNaN(tab_result3_y[i+1])){
                if((tab_result3_y[i] > 0 && tab_result3_y[i] < 500) || (tab_result3_y[i+1] > 0 && tab_result3_y[i+1] < 500)){   
                    context.SetSourceColor(green);
                    context.MoveTo((int) Math.Round(tab_result3_x[i]), (int) Math.Round(tab_result3_y[i]));
                    context.LineTo((int) Math.Round(tab_result3_x[i+1]), (int) Math.Round(tab_result3_y[i+1]));
                    context.Stroke();  
                }
            }
        }
    }
    
    // drawing net aera
    void trace_simpson()
    {
        try{
            if (first_draw){
                operation op = new operation(equation);
            
                double xc_1 = x_min;
                double yc_1 = op.compute(x_min);
                double xc_2;
                double yc_2;
                                            
                double pitch = (x_max-x_min)/500;
                double x_1;
                double y_1;
                double x_2;
                double y_2;
                                                                
                for(double i_graph = x_min; i_graph <= x_max; i_graph = i_graph+pitch){
                    xc_2 = xc_1 + pitch;
                    yc_2 = op.compute(xc_2);
                                                        
                    x_1 = trans_x(xc_1);
                    y_1 = trans_y(yc_1);
                    x_2 = trans_x(xc_2);
                    y_2 = trans_y(yc_2);
    
                    if(!Double.IsNaN(yc_1) && !Double.IsNaN(yc_2)){
                        context.SetSourceColor(red);
                        context.MoveTo((int) Math.Round(x_1), (int) Math.Round(trans_y(0f)));
                        context.LineTo((int) Math.Round(x_1), (int) Math.Round(y_1));
                        context.Stroke();
                        context.MoveTo((int) Math.Round(x_2), (int) Math.Round(trans_y(0f)));
                        context.LineTo((int) Math.Round(x_2), (int) Math.Round(y_2));
                        context.Stroke();  
                    }                                        
                    // next point
                    xc_1 = xc_2;
                    yc_1 = yc_2;    
                }
            }
            else {    // using buffer of Fn 1
                for(int i = 0; i < 500; i++){
                    if(!Double.IsNaN(tab_result1_y[i]) && !Double.IsNaN(tab_result1_y[i+1])){
                            context.SetSourceColor(red);
                            context.MoveTo((int) Math.Round(tab_result1_x[i]), (int) Math.Round(trans_y(0f)));
                            context.LineTo((int) Math.Round(tab_result1_x[i]), (int) Math.Round(tab_result1_y[i]));
                            context.Stroke();
                            context.MoveTo((int) Math.Round(tab_result1_x[i+1]), (int) Math.Round(trans_y(0f)));
                            context.LineTo((int) Math.Round(tab_result1_x[i+1]), (int) Math.Round(tab_result1_y[i+1]));
                            context.Stroke();  
                    }
                }
            }
        }    
        catch(Exception ex)
        {    
        }
    }
    
    void trace_derivative()
    {
        try{
            if (first_draw){
                Calculus c = new Calculus(equation);
            
                double xc_1 = x_min;
                double yc_1 = c.ndfdx(this.x_min);            
                double xc_2;
                double yc_2;
                                            
                double pitch = (x_max-x_min)/500;
                double x_1;
                double y_1;
                double x_2;
                double y_2;
                
                // save results
                int i_save = 0;
                
                tab_result_derivative_x[i_save] = trans_x(xc_1);
                tab_result_derivative_y[i_save] = trans_y(yc_1);
                                                                
                for(double i_graph = x_min; i_graph <= x_max; i_graph = i_graph + pitch){
                    xc_2 = xc_1 + pitch;
                    yc_2 = c.ndfdx(xc_2);
                                                        
                    x_1 = trans_x(xc_1);
                    y_1 = trans_y(yc_1);
                    x_2 = trans_x(xc_2);
                    y_2 = trans_y(yc_2);
                    
                    // save results
                    i_save++;
                    tab_result_derivative_x[i_save] = x_2;
                    tab_result_derivative_y[i_save] = y_2;
    
                    if(!Double.IsNaN(yc_1) && !Double.IsNaN(yc_2)){
                        if((yc_1 > y_min && yc_1 < y_max) || (yc_2 > y_min && yc_2 < y_max)){
                            context.SetSourceColor(cyan);
                            context.MoveTo((int) Math.Round(x_1), (int) Math.Round(y_1));
                            context.LineTo((int) Math.Round(x_2), (int) Math.Round(y_2));
                            context.Stroke();
                        }
                    }                                        
                    // next point
                    xc_1=xc_2;
                    yc_1=yc_2;    
                }
            }
            else {    // using buffer tab_result_derivative
                for(int i = 0; i < 500; i++){
                    if(!Double.IsNaN(tab_result_derivative_y[i]) && !Double.IsNaN(tab_result_derivative_y[i+1])){
                        if((tab_result_derivative_y[i] > 0 && tab_result_derivative_y[i] < 500) || (tab_result_derivative_y[i+1] > 0 && tab_result_derivative_y[i+1] < 500)){    
                            context.SetSourceColor(cyan);
                            context.MoveTo((int) Math.Round(tab_result_derivative_x[i]), (int) Math.Round(tab_result_derivative_y[i]));
                            context.LineTo((int) Math.Round(tab_result_derivative_x[i+1]), (int) Math.Round(tab_result_derivative_y[i+1]));
                            context.Stroke();
                        }
                    }
                }
            }
        }    
        catch(Exception ex)
        {    
        }
    }
    
    void trace_secondderivative()
    {
        try{
            if (first_draw){
                Calculus c = new Calculus(equation);
            
                double xc_1 = x_min;
                double yc_1 = c.n2dfdx(this.x_min);            
                double xc_2;
                double yc_2;
                                            
                double pitch = (x_max - x_min) / 500;
                double x_1;
                double y_1;
                double x_2;
                double y_2;
                
                // save results
                int i_save = 0;
                
                tab_result_secondderivative_x[i_save] = trans_x(xc_1);
                tab_result_secondderivative_y[i_save] = trans_y(yc_1);
                                                                
                for(double i_graph = x_min; i_graph <= x_max; i_graph = i_graph + pitch){
                    xc_2 = xc_1 + pitch;
                    yc_2 = c.n2dfdx(xc_2);
                                                        
                    x_1 = trans_x(xc_1);
                    y_1 = trans_y(yc_1);
                    x_2 = trans_x(xc_2);
                    y_2 = trans_y(yc_2);
                    
                    // save results
                    i_save++;
                    tab_result_secondderivative_x[i_save] = x_2;
                    tab_result_secondderivative_y[i_save] = y_2;
    
                    if(!Double.IsNaN(yc_1) && !Double.IsNaN(yc_2)){
                        if((yc_1 > y_min && yc_1 < y_max) || (yc_2 > y_min && yc_2 < y_max)){
                            context.SetSourceColor(orange);
                            context.MoveTo((int) Math.Round(x_1), (int) Math.Round(y_1));
                            context.LineTo((int) Math.Round(x_2), (int) Math.Round(y_2));
                            context.Stroke();
                        }
                    }                                        
                    // next point
                    xc_1=xc_2;
                    yc_1=yc_2;    
                }
            }
            else {    // using buffer tab_result_derivative
                for(int i = 0; i < 500; i++){
                    if(!Double.IsNaN(tab_result_secondderivative_y[i]) && !Double.IsNaN(tab_result_secondderivative_y[i+1])){
                        if((tab_result_secondderivative_y[i] > 0 && tab_result_secondderivative_y[i] < 500) || (tab_result_secondderivative_y[i+1] > 0 && tab_result_secondderivative_y[i+1] < 500)){    
                            context.SetSourceColor(orange);
                            context.MoveTo((int) Math.Round(tab_result_secondderivative_x[i]), (int) Math.Round(tab_result_secondderivative_y[i]));
                            context.LineTo((int) Math.Round(tab_result_secondderivative_x[i+1]), (int) Math.Round(tab_result_secondderivative_y[i+1]));
                            context.Stroke();
                        }
                    }
                }
            }
        }    
        catch(Exception ex)
        {    
        }
    }
         
    // transformation on x
    double trans_x(double x){
        return (500 / (x_max - x_min) * (x - x_min));    
    }
        
    // transformation on y
    double trans_y(double y){
        return (-500 / (y_max - y_min) * (y - y_max));
    }
    
    void button_zoom_up_click (object o, EventArgs args)
    {
        first_draw = true;
        double diff = (y_max - y_min)/10;
        y_min = y_min + diff;
        y_max = y_max + diff;
        this.simpson = false;
        this.QueueDraw();
    }
    
    void button_zoom_down_click (object o, EventArgs args)
    {
        first_draw = true;
        double diff = (y_max - y_min)/10;
        y_min = y_min - diff;
        y_max = y_max - diff;
        this.simpson = false;
        this.QueueDraw();   
    }
    
    void button_zoom_right_click (object o, EventArgs args)
    {
        first_draw = true;
        double diff = (x_max - x_min)/10;
        x_min = x_min + diff;
        x_max = x_max + diff;
        this.simpson = false;
        this.QueueDraw();
    }
    
    void button_zoom_left_click (object o, EventArgs args)
    {
        first_draw = true;
        double diff = (x_max - x_min)/10;
        x_min = x_min - diff;
        x_max = x_max - diff;
        this.simpson = false;
        this.QueueDraw();
    }
    
    void button_zoom_in_click (object o, EventArgs args)
    {
        first_draw = true;
        double diff = (y_max - y_min)/10;
        y_min = y_min + diff;
        y_max = y_max - diff;
        x_min = x_min + diff;
        x_max = x_max - diff;
        this.simpson = false;
        this.QueueDraw();
    }
    
    void button_zoom_out_click (object o, EventArgs args)
    {
        first_draw = true;
        double diff = (y_max - y_min)/10;
        y_min = y_min - diff;
        y_max = y_max + diff;
        x_min = x_min - diff;
        x_max = x_max + diff;
        this.simpson = false;
        this.QueueDraw();
    }
    
    void button_zoom_100_click (object o, EventArgs args)
    {
        first_draw = true;
        y_min = -10;
        y_max = 10;
        x_min = -10;
        x_max = 10;
        this.simpson = false;
        this.QueueDraw();
    }
    
    void on_mouse_leave_graph(object o, LeaveNotifyEventArgs args) 
    {
        label_coord.Text = "( x ; y )";
    }
    
    // display (X,Y) of the mouse point
    void on_mouse_move_graph(object o, MotionNotifyEventArgs args)
    {
        int x=0;
        int y=0;
        Gdk.Window window = args.Event.Window;

        if (args.Event.IsHint) {
            Gdk.ModifierType s;
            window.GetPointer (out x, out y, out s);
        } 
        else {
            x = (int) args.Event.X;
            y = (int) args.Event.Y;
        }
        
        label_coord.Text= "( "+Math.Round(x_min+x*((x_max-x_min)/500), 2).ToString() + " ; "+Math.Round((y_max+y*((y_max-y_min)/(-500))), 2).ToString()+" )";
        args.RetVal = true;
    }
    
    // change color of screen
    void wbscreen_cb (object o, EventArgs args)
    {
        this.black_screen = !this.black_screen;
        this.QueueDraw();
    }
    
    // display or not the grid
    void grid_cb (object o, EventArgs args)
    {
        this.grid = !this.grid;
        this.QueueDraw();
    }
    
    // Net Area
    void simpson_cb (object o, EventArgs args)
    {
        if(!extension_exist){
            statusbar = new Statusbar();
            statusbar.HeightRequest = 25;
            vbox.PackStart(statusbar,true,true,0);
            win.ShowAll ();
            extension_exist = true;
            extension_show = true;
            
        }
        else if(!extension_show){
            statusbar.Show();
            extension_show = true;
        }
            
        first_draw = true;
        this.simpson = true;
        this.QueueDraw();
        
        if(extension_exist){
            if (this.equation != ""){
                try {
                    Calculus c = new Calculus(this.equation);
                    double s = c.simpsonsrule(x_min, x_max);
                    statusbar.Push(0,"Net Area Under Fn 1 = " + s.ToString());
                }
                catch (Exception ex) {
                    statusbar.Push(0,"Could not calculate area under curve!");

                }
            }
            else
                statusbar.Push(0,"Fn 1 does not exist!");
        }
    }
    
    // average value
    void averagevalue_cb (object o, EventArgs args)
    {
        if(!extension_exist){
            statusbar = new Statusbar();
            statusbar.HeightRequest = 25;
            vbox.PackStart(statusbar,true,true,0);
            //statusbar.Push(0,"test");
            win.ShowAll ();
            extension_exist = true;
            extension_show = true;
        }
        else if(!extension_show){
            statusbar.Show();
            extension_show = true;
        }
        
        if(extension_exist)
        {
            if (this.equation != ""){
                try {
                    Calculus c = new Calculus(this.equation);
                    double s = c.averagevalue(x_min, x_max);
                    statusbar.Push(0,"Average Value of Eq 1 = " + s.ToString());
                }
                catch (Exception ex) {
                    statusbar.Push(0,"Could not calculate the average value!");

                }
            }
            else
                statusbar.Push(0,"Fn 1 does not exist!");
        }
    }
    
    // draw derivative
    void derivative_cb (object o, EventArgs args)
    {
        first_draw = true;
        this.derivative = true;
        this.QueueDraw();
    }
    
    // draw second derivative
    void secondderivative_cb (object o, EventArgs args)
    {
        first_draw = true;
        this.secondderivative = true;
        this.QueueDraw();
    }
    
    // clear all calculus
    void clear_cb (object o, EventArgs args)
    {
        if (extension_show)
        {
            statusbar.Hide();
            extension_show = false;
            win.SetSizeRequest(501, 501);
        }
        
        first_draw = true;
        this.simpson = false;
        this.derivative = false;
        this.secondderivative = false;
        this.QueueDraw();
    }
}
