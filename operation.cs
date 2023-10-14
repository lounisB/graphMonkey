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
    
public class operation : object{
    public string text;                 // full text of the operation
    public string op;                   // operator
    public operation right;             // left side of the operation
    public operation left;              // right side of the operation

    public operation(string text) {
        this.text=text;            
    }
    
    public void split(){
        //search a + (the more at rigth) not in parenthesis
        int plusPos = this.text.LastIndexOf("+");
        bool plusFind = false;
        while (plusPos != -1 && plusFind == false ){
            if (this.countUpTo("(", plusPos) != this.countUpTo(")", plusPos)){
                plusPos = this.text.LastIndexOf("+", plusPos-1, plusPos);
            }
            else{
                plusFind = true;
            }
        }
        if (plusFind == false){
            plusPos = -1;
        }
        
        //search a - (the more at rigth) not in parenthesis
        int lessPos = this.text.LastIndexOf("-");
        bool lessFind = false;
        while (lessPos != -1 && lessFind == false ){
            if (this.countUpTo("(", lessPos) != this.countUpTo(")", lessPos)){
                lessPos = this.text.LastIndexOf("-", lessPos - 1, lessPos);
            }
            else{
                lessFind = true;
            }
        }
        if (lessFind == false){
            lessPos = -1;
        }
        
        // priority for + and -
        
        // initially a +
        if ( (plusPos != -1 && lessPos == -1) || (plusPos != -1 && lessPos != -1 && plusPos > lessPos)){
            this.op = "+";
            
            string leftText = this.text;
            leftText = leftText.Remove(plusPos, leftText.Length - plusPos);    
            this.left = new operation(leftText);
            
            string rightText=this.text;
            rightText = rightText.Remove(0, plusPos + 1);
            this.right = new operation(rightText);                
        }
        // initially a -
        else if ( (plusPos == -1 && lessPos != -1) || (plusPos != -1 && lessPos != -1 && lessPos > plusPos)){
            this.op = "-";
            
            string leftText = this.text;
            leftText = leftText.Remove(lessPos, leftText.Length - lessPos);    
            this.left = new operation(leftText);
            
            string rightText=this.text;
            rightText = rightText.Remove(0, lessPos + 1);
            this.right = new operation(rightText);    
        }
        
        // now * and /
        else {
            //search a * (the more at rigth) not in parenthesis
            int multiplyPos = this.text.LastIndexOf("*");
            bool multiplyFind = false;
            while (multiplyPos != -1 && multiplyFind == false){
                if (this.countUpTo("(", multiplyPos) != this.countUpTo(")", multiplyPos)){
                    multiplyPos = this.text.LastIndexOf("*", multiplyPos-1, multiplyPos);
                }
                else{
                    multiplyFind = true;
                }
            }
            if (multiplyFind == false){
                multiplyPos= -1;
            }
                
            //search a / (the more at rigth) not in parenthesis
            int dividePos = this.text.LastIndexOf("/");
            bool divideFind = false;
            while (dividePos != -1 && divideFind == false ){
                if (this.countUpTo("(", dividePos) != this.countUpTo(")", dividePos)){
                    dividePos = this.text.LastIndexOf("/", dividePos - 1, dividePos);
                }
                else{
                    divideFind = true;
                }
            }                
            if (divideFind == false){
                dividePos= -1;
            }                
                
            // initially  a *
            if ( (multiplyPos != -1 && dividePos == -1) || (multiplyPos != -1 && dividePos != -1 && multiplyPos > dividePos)){ // initially  a *
                this.op = "*";
                    
                string leftText = this.text;
                leftText = leftText.Remove(multiplyPos, leftText.Length - multiplyPos);    
                this.left = new operation(leftText);
                
                string rightText = this.text;
                rightText = rightText.Remove(0, multiplyPos + 1);
                this.right = new operation(rightText);                
            }
            // initially  a /
            else if ( (multiplyPos == -1 && dividePos != -1) || (multiplyPos != -1 && dividePos != -1 && dividePos > multiplyPos)){ // initially  a /
                this.op = "/";
                
                string leftText = this.text;
                leftText = leftText.Remove(dividePos, leftText.Length - dividePos);    
                this.left = new operation(leftText);
                
                string rightText = this.text;
                rightText = rightText.Remove(0,dividePos + 1);
                this.right = new operation(rightText);    
            }
            
            // now ^
            else{
                //search a ^ (the more at rigth) not in parenthesis
                int powerPos = this.text.IndexOf("^");
                bool powerFind = false;
                while (powerPos != -1 && powerFind == false ){
                    if (this.countUpTo("(", powerPos) != this.countUpTo(")", powerPos) ){
                        powerPos = this.text.IndexOf("^", powerPos+1);
                    }
                    else{
                        powerFind = true;
                    }
                }
                if (powerFind == false){                    
                    powerPos = -1;
                }                
                            
                if ( (powerPos != -1) ){ // there is a ^
                    this.op = "^";
                
                    string leftText = this.text;
                    leftText = leftText.Remove(powerPos, leftText.Length - powerPos);    
                    this.left = new operation(leftText);
                
                    string rightText = this.text;
                    rightText = rightText.Remove(0,powerPos + 1);
                    this.right = new operation(rightText);                
                }                
                else{ // there is no + - / * or ^ so text is like "(2+x-3)" or "2" or "x" or r(2*x)...
                            
                    //if text is in parenthesis (2+x-3), delete parenthesis
                    if (this.text.StartsWith("(")){
                        this.op = "()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,1);                        //delete "("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"      
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("sqrt(")){ // root square
                        this.op = "sqrt()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,5);                        //delete "sqrt("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"                
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("cos(")){ // cos
                        this.op = "cos()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,4);                        //delete "cos("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"               
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("sin(")){ // sin
                        this.op = "sin()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,4);                        //delete "sin("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"             
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("tan(")){ // tan
                        this.op = "tan()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,4);                        //delete "tan("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"               
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("ln(")){ // ln
                        this.op = "ln()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,3);                        //delete "ln("
                        leftText = leftText.Remove(leftText.Length -1, 1);      //delete ")"              
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("log(")){ // log
                        this.op = "log()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,4);                        //delete "log("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"              
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("acos(")){ // acos
                        this.op = "acos()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,5);                        //delete "acos("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"            
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("asin(")){ // asin
                        this.op = "asin()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,5);                        //delete "asin("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"              
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("atan(")){ // atan
                        this.op = "atan()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,5);                        //delete "atan("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //on supprime le ")"               
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("cosh(")){ // cosh
                        this.op = "cosh()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,5);                        //delete "cosh("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"               
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("sinh(")){ // sinh
                        this.op = "sinh()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,5);                        //delete "sinh("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"              
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("tanh(")){ // tanh
                        this.op = "tanh()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,5);                        //delete "tanh(
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"               
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("exp(")){ // exp
                        this.op = "exp()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,4);                        //delete "exp("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"                
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("abs(")){ // abs
                        this.op = "abs()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,4);                        //delete "abs("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"              
                        this.left = new operation(leftText);
                    }
                    else if (this.text.StartsWith("int(")){    // int
                        this.op = "int()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0,4);                        //delete "int("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     //delete ")"              
                        this.left = new operation(leftText);
                    }
                }
            }
        }
    }
        
    public void correct(){
        if (this.text.StartsWith("-")){
            this.text = this.text.Insert(0, "0");
        }
        
        this.text = this.text.Replace("X", "x");
        this.text = this.text.Replace("(-", "(0-");
        
        this.text = this.text.Replace("x(", "x*(");
        this.text = this.text.Replace("0(", "0*(");
        this.text = this.text.Replace("1(", "1*(");
        this.text = this.text.Replace("2(", "2*(");
        this.text = this.text.Replace("3(", "3*(");
        this.text = this.text.Replace("4(", "4*(");
        this.text = this.text.Replace("5(", "5*(");
        this.text = this.text.Replace("6(", "6*(");
        this.text = this.text.Replace("7(", "7*(");
        this.text = this.text.Replace("8(", "8*(");
        this.text = this.text.Replace("9(", "9*(");
        
        this.text = this.text.Replace(")x", ")*x");
        
        this.text = this.text.Replace("0x", "0*x");
        this.text = this.text.Replace("1x", "1*x");
        this.text = this.text.Replace("2x", "2*x");
        this.text = this.text.Replace("3x", "3*x");
        this.text = this.text.Replace("4x", "4*x");
        this.text = this.text.Replace("5x", "5*x");
        this.text = this.text.Replace("6x", "6*x");
        this.text = this.text.Replace("7x", "7*x");
        this.text = this.text.Replace("8x", "8*x");
        this.text = this.text.Replace("9x", "9*x");
            
        this.text = this.text.Replace(")(", ")*(");
    }
        
        
    public double compute(double x){
        double result = 0;
        this.split();
        
        // case: + - * / ^
        if(this.left != null && this.right != null){
            if(this.op == "+"){
                result = this.left.compute(x) + this.right.compute(x);
                return result;
            }
            else if(this.op == "*"){
                result=this.left.compute(x) * this.right.compute(x);
                return result;
            }
            else if(this.op == "-"){
                result = this.left.compute(x) - this.right.compute(x);
                return result;                    
            }
            else if(this.op == "/"){
                result = this.left.compute(x) / this.right.compute(x);
                return result;
            }
            else if(this.op == "^"){
                double left_double = this.left.compute(x);
                double right_double = this.right.compute(x);
                result = Math.Pow(left_double, right_double);
                return result;
            }
        }
        // case: () r() cos() sin() tan() and n()
        else if(this.left != null){
            if (this.op == "()"){
                result = this.left.compute(x);
                return result;
            }
            else if (this.op == "sqrt()"){
                double left_double = this.left.compute(x);
                result = Math.Sqrt(left_double);
                return result;
                
            }
            else if (this.op == "cos()"){
                double left_double = this.left.compute(x);
                result = Math.Cos(left_double);
                return result;
                
            }
            else if (this.op == "sin()"){
                double left_double = this.left.compute(x);
                result = Math.Sin(left_double);
                return result;
                
            }
            else if (this.op == "tan()"){    
                double left_double = this.left.compute(x);
                result = Math.Tan(left_double);
                return result;
                
            }
            else if (this.op == "ln()"){
                double left_double = this.left.compute(x);
                result = Math.Log(left_double);
                return result;
                
            }
            else if (this.op == "log()"){
                double left_double = this.left.compute(x);
                result = Math.Log10(left_double);
                return result;
                
            }
            else if (this.op == "acos()"){
                double left_double = this.left.compute(x);
                result = Math.Acos(left_double);
                return result;
                
            }
            else if (this.op == "asin()"){
                double left_double = this.left.compute(x);
                result = Math.Asin(left_double);
                return result;
                
            }
            else if (this.op == "atan()"){
                double left_double = this.left.compute(x);
                result = Math.Atan(left_double);
                return result;
                
            }
            else if (this.op == "cosh()"){
                double left_double = this.left.compute(x);
                result = Math.Cosh(left_double);
                return result;
                
            }
            else if (this.op == "sinh()"){
                double left_double = this.left.compute(x);
                result = Math.Sinh(left_double);
                return result;
                
            }
            else if (this.op == "tanh()"){
                double left_double = this.left.compute(x);
                result = Math.Tanh(left_double);
                return result;
                
            }
            else if (this.op == "exp()"){
                double left_double = this.left.compute(x);
                result = Math.Exp(left_double);
                return result;
                
            }
            else if (this.op == "abs()"){
                double left_double = this.left.compute(x);
                result = Math.Abs(left_double);
                return result;
                
            }
            else if (this.op == "int()"){
                double left_double = this.left.compute(x);
                result = Math.Floor(left_double);
                return result;
            }
        }
        
        else{ // text is like "x" or "2,1213" or "2" 
            if (this.text == "x" || this.text == "X"){
                return x;
            }
            else{
                decimal res_d = Convert.ToDecimal(this.text);
                double res_f = (double) res_d;    
                result=res_f;
                return result;
            }
        }
        return result;    
    }

    // countUpTo("a", 3) for "aaaaaa" => 3
    private int countUpTo(string ch, int end){
        int pos;
        int nb = 0;
        bool stop = false;
        int i = 0;
        while( (i <= end) && (stop == false) && (i <= this.text.Length)){
            pos=text.IndexOf(ch, i, end-i);
            if(pos != -1){
                nb++;
                i = pos + 1;
            }
            else{
                stop = true;
            }    
        }
        return nb;
    }
}
