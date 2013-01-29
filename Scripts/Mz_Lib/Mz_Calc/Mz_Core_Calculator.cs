using UnityEngine;
using System.Collections;

public class Mz_Core_Calculator {
	
	public static double ADD(double a , double b) {
		double result = a + b;
		
		return result;
	} 
	
	public static double Minus(double a, double b) {
		double result = a - b;
		
		return result;
	}
	
	public static double Multiply(double a, double b) {
		double result = a*b;
		
		return result;
	}
	
	public static double Divide(double a, double b) {
        if (b != 0)
        {
            double result = a / b;

            return result;
        }
        else {		
            //Warning...Divide By Zero.
            return 0;
        }
	}
}
