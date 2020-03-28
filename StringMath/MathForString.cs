/*
 * Created by SharpDevelop.
 * User: kafeinaltor
 * Date: 20.03.2020
 * Time: 19:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
namespace StringMath
{
	/// <summary>
	/// Description of MathForString.
	/// </summary>
	public static class MathForString
	{
		public static string Sum(this string num1, string num2, bool omitZeros)
		{
			int s1 = num1.Sign();
			int s2 = num2.Sign();
        	
			string usNum1 = num1.WoSign();
			string usNum2 = num2.WoSign();
			
			if (s1 * s2 == -1) {
				return s1 == -1 ? usNum2.Minus(usNum1, omitZeros) : usNum1.Minus(usNum2, omitZeros);
			}
			if (s1 * s2 == 0) {
				return s1 != 0 ? num1 : num2;
			}
			
			var arr = usNum1.EqualizeLength(usNum2);
			usNum1 = arr[0];
			usNum2 = arr[1];
        	
			int max = usNum1.Length;
			string result = "";
			int rem = 0;
			for (int i = max - 1; i >= 0; i--) {
				if (usNum1[i] == '.') {
					result = result.Insert(0, ".");
					continue;
				}
				int a = int.Parse(usNum1[i].ToString());
				int b = int.Parse(usNum2[i].ToString());
				int total = a + b + rem;
				int r = total % 10;
				rem = total / 10;
        		
				result = (i == 0) ? result.Insert(0, total.ToString()) : result.Insert(0, r.ToString());
			}
			if (omitZeros)
				result = result.FixNumber().Simplify();
			return result.Insert(0, s1 == -1 ? "-" : "");
		}
        
		public static string Minus(this string num1, string num2, bool omitZeros)
		{
			int s1 = num1.Sign();
			int s2 = num2.Sign();

			string wsnum1 = num1.WoSign();
			string wsnum2 = num2.WoSign();
			
			if (s1 * s2 == 0) {
				return s1 == 0 ? wsnum2.Insert(0, s2 == 1 ? "-" : "") : num1;
			}

			if (s1 * s2 == -1) {
				return wsnum1.Sum(wsnum2, omitZeros).Insert(0, s1 == -1 ? "-" : "");
			}

			int comp = wsnum1.Compare(wsnum2);

			if (comp == 0)
				return "0";

			if (comp == -1) {
				string temp = wsnum1;
				wsnum1 = wsnum2;
				wsnum2 = temp;
			}
			
			var arr = wsnum1.EqualizeLength(wsnum2);
			string usNum1 = arr[0];
			string usNum2 = arr[1];
			
			int max = usNum1.Length;
			string result = "";
			for (int i = max - 1; i >= 0; i--) {
				if (usNum1[i] == '.') {
					result = result.Insert(0, ".");
					continue;
				}
				int a = int.Parse(usNum1[i].ToString());
				int b = int.Parse(usNum2[i].ToString());
				
				if (a < b) {
					usNum1 = usNum1.BorrowingFromNeighbour(usNum2, i);
					result = result.Insert(0, (a + 10 - b).ToString());
				} else {
					result = result.Insert(0, (a - b).ToString());
				}
				
			}
            
			if (omitZeros)
				result = result.FixNumber().Simplify();
			
			if (comp == 1) {
				return (s1 == -1 ? "-" : "") + result;
			}
			if (comp == -1) {
				return (s1 == 1 ? "-" : "") + result;
			}
            
			return result;

		}
        
		public static string Multiply(this string num1, string num2)
		{
			if (num1.Sign() == 0 || num2.Sign() == 0)
				return "0";
			
			int dot1 = num1.IndexOf('.');
			int dot2 = num2.IndexOf('.');
			
			int decLen1 = dot1 > 0 ? num1.Length - 1 - dot1 : 0;
			int decLen2 = dot2 > 0 ? num2.Length - 1 - dot2 : 0;
			
			string rnum1 = num1.WoSign().Replace(".", "").FixNumber();
			string rnum2 = num2.WoSign().Replace(".", "").FixNumber();
			
			int len1 = rnum1.Length;
			int len2 = rnum2.Length;

			List<string> list = new List<string>();
			string result = "";
			int rem = 0;
			for (int i = len1 - 1; i >= 0; i--) {
				result = "";
				rem = 0;
				int a = int.Parse(rnum1[i].ToString());
				for (int j = len2 - 1; j >= 0; j--) {
					int b = int.Parse(rnum2[j].ToString());
					int res = (a * b) + rem;
					rem = res / 10;
					int t = res % 10;
					result = result.Insert(0, t.ToString());
				}
				
				list.Add(result.Insert(0, rem == 0 ? "" : rem.ToString()));
			}
			string multResult = "0";
			for (int i = 0; i < list.Count; i++) {
				multResult = multResult.Sum(list[i].InsertZerosToEnd(i), true);
			}
			
			int nofComma = decLen1 + decLen2;
			int len = -multResult.Length + nofComma;
			if (nofComma > 0) {
				if (multResult.Length - nofComma > 0) {
					multResult = multResult.Insert(multResult.Length - nofComma, ".");
				} else {
					for (int i = 0; i < len; i++) {
						multResult = multResult.Insert(0, "0");
					}
					multResult = multResult.Insert(0, "0.");
					
				}
			}
			return multResult.FixNumber().Simplify().Insert(0, num1.Sign() == num2.Sign() ? "" : "-");
			
		}
        
		public static string Divide(this string num1, string num2, int nofMaxDecimalDigits)
		{
			
			string usnum1 = num1.WoSign();
			string usnum2 = num2.WoSign();
			string result = "";
			
			int s1 = num1.Sign();
			int s2 = num2.Sign();
			
			if(s1==0)
			{
				return s2 == 0 ? "NaN" : "0";
			}
			if(s2==0)
			{
				return "NaN";
			}
			
			int nofDec1 = num1.GetDecimalLength();
			int nofDec2 = num2.GetDecimalLength();
			
			int nofMaxDec = Math.Max(nofDec1, nofDec2);
			for (int i = 0; i < nofMaxDec; i++) {
				usnum1 = usnum1.Multiply("10");
				usnum2 = usnum2.Multiply("10");
			}
			
			bool dotPlaced = false;
			while (usnum1.Compare(usnum2) < 0) {
				usnum1 = usnum1.Multiply("10");
				if (dotPlaced) {
					result += "0";
				} else {
					result = result.Insert(0, "0.");
					dotPlaced = true;
				}
			}
			
			int start = 0;
			int count = 1;
			string remainder = "";
			string divResult = "";
			string totalResult = "";
			while (true) {
				if(start + count > usnum1.Length && !dotPlaced)
				{
					divResult = divResult.Insert(divResult.Length, ".");
					dotPlaced = true;
				}
				string partial = remainder + usnum1.DivSubstring(start, count);
				int div = partial.FindDivider(usnum2);
				string toSubtract = usnum2.Multiply(div.ToString());
				remainder = partial.Minus(toSubtract, true);
				divResult = divResult.Insert(divResult.Length, div.ToString());
				start++;
				count = 1;
				
				totalResult = result + divResult.FixNumber().Simplify();
				
				if(totalResult.GetDecimalLength() >= nofMaxDecimalDigits)
					return ((s1*s2) < 0 ? "-" : "") + totalResult;
				if(remainder.Sign() == 0 && start >= usnum1.Length)
					return ((s1*s2) < 0 ? "-" : "") + totalResult;
			}
			
		}
        
		public static string Factorial(this string num)
		{
			if (num.Sign() == 0 || num.FixNumber() == "1")
				return "1";
			
			return num.Multiply(num.Minus("1", true).Factorial());
		}
	}

}