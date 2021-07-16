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

public class Calculus {

	public string text;
	public operation op;
	
	public Calculus(string text)
	{
		this.text = text;
		this.op = new operation(this.text);
		this.op.correct();
	}
	
	// numerical derivative at the point x
	public double ndfdx(double x)
	{
		double h = x + 0.000001d;
		double top = op.compute(h)-op.compute(x);
		double bottom = h-x;
		return Math.Round(top/bottom, 4);
	}
	
	// numerical second derivative at the point x
	public double n2dfdx(double x)
	{
		double h = x + 0.0001d;
		double h2 = x + 0.0002d;
		double top = op.compute(h2) - op.compute(h) - op.compute(h) + op.compute(x);
		double bottom = (h-x)*(h-x);
		return Math.Round(top/bottom, 4);
	}
	
	// numerical integral of a function on the interval [a,b] using Simpson's Rule.
	public double simpsonsrule(double a, double b)
	{
		double n = 100.0d;
		double total = 0;
		double h = Math.Abs(b-a)/n;
		double[] l = new double[120];
		double x = a;
		l[0] = op.compute(x);

		int count = 1;
		while (count < n) {
			x = (double)a + count*h;
			
			if (count%2 == 0) {
				l[count] = 2*op.compute(x);
			}
			else {
				l[count] = 4*op.compute(x);
			} 		
			count++;
		}
		
		x = (double)b;
		l[count] = op.compute(x);
		
		for (int i = 0; i <=count; i++) {
			total = total + l[i];
		}
		return Math.Round((total*h)/3d, 4);			
	}
	
	public double averagevalue(double a, double b)
	{
		return Math.Round((simpsonsrule(a,b))/(b-a), 4);
	}
}
