/*
 * Created by SharpDevelop.
 * User: kafeinaltor
 * Date: 20.03.2020
 * Time: 20:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace StringMath
{
	/// <summary>
	/// Description of MathHelper.
	/// </summary>
	public static class MathHelper
	{
		public static int Compare(this string num1, string num2)
		{
			string sinum1 = num1.Simplify();
			string sinum2 = num2.Simplify();
			
			int s1 = sinum1.Sign();
			int s2 = sinum2.Sign();
			
			if (sinum1 == sinum2)
				return 0;
			if (s1 > s2)
				return 1;
			if (s1 < s2)
				return -1;
			if (s1 == 0 && s2 == 0)
				return 0;

			string usnum1 = sinum1.WoSign();
			string usnum2 = sinum2.WoSign();
			
			var arr = usnum1.EqualizeLength(usnum2);
			usnum1 = arr[0];
			usnum2 = arr[1];
			
			int len1 = usnum1.Length;
			bool isgreater = false;
			

			for (int i = 0; i < len1; i++) {
				if (usnum1[i] != usnum2[i]) {
					isgreater = usnum1[i] > usnum2[i];
					break;
				}
			}
				
			return s1 == 1 ? isgreater ? 1 : -1 : !isgreater ? 1 : -1;
			
		}
		public static int GetDecimalLength(this string num)
		{
			var arr = num.Split('.');
			return arr.Length > 1 ? arr[1].Length : 0;
		}
		public static string WoSign(this string num)
		{
			num.CheckIfNumber();
			
			const string signPattern = @"^(\+|-)*";
			Match m = Regex.Match(num, signPattern);
			if (m != null)
				return Regex.Replace(num, signPattern, "");
			else
				return num;
		}
		public static int Sign(this string num)
		{
			num.CheckIfNumber();
			
			if (num.Replace("-", "").Replace("+", "").Replace(".", "").All(x => x == '0'))
				return 0;
			
			const string signPattern = @"^(\+|-)*";
			Match m = Regex.Match(num, signPattern);
			if (m != null) {
				return m.Value.ToCharArray().Count(x => x == '-') % 2 == 0 ? 1 : -1;
			} else
				return 1;
		}
		
		public static bool IsNumber(this string num)
		{
			const string numberPattern = @"^(\+|-)*[0-9]+(\.[0-9]+)?$";
			return Regex.IsMatch(num, numberPattern);
		}
		
		public static string Simplify(this string num)
		{
			num.CheckIfNumber();
			string pattern = @"(?:[1-9][0-9]*)?0?(?:\.[0-9]*[1-9])?";
			var mc  = Regex.Matches(num, pattern).Cast<Match>();
			var selected = mc.First(x=> x.Value.Length == mc.Max(a=>a.Value.Length));
			
			return (num.Sign() > 0 ? "" : "-") + selected;
		}
		public static void CheckIfNumber(this string num)
		{
			if (!num.IsNumber())
				throw new Exception(num + " is not a valid number");
		}
		public static string Change(this string num, int i, string newStr)
		{
			return num.Insert(i, newStr).Remove(i + newStr.Length, 1);
		}
		public static string[] EqualizeLength(this string num1, string num2)
		{
			if (num1.Sign() == -1 || num2.Sign() == -1)
				throw new Exception();
			
			var arr1 = num1.Split('.');
			var arr2 = num2.Split('.');
			
			var intPart1 = arr1[0];
			var intPart2 = arr2[0];
			
			var decPart1 = arr1.Length > 1 ? arr1[1] : "";
			var decPart2 = arr2.Length > 1 ? arr2[1] : "";

			int max = Math.Max(decPart1.Length, decPart2.Length);
			int decLen1 = decPart1.Length;
			int decLen2 = decPart2.Length;
			for (int j = 0; j < max - decLen1; j++) {
				decPart1 += "0";
			}
			for (int j = 0; j < max - decLen2; j++) {
				decPart2 += "0";
			}
			
			int intLen1 = intPart1.Length;
			int intLen2 = intPart2.Length;
			
			max = Math.Max(intLen1, intLen2);
			for (int j = 0; j < max - intLen1; j++) {
				intPart1 = intPart1.Insert(0, "0");
			}
			for (int j = 0; j < max - intLen2; j++) {
				intPart2 = intPart2.Insert(0, "0");
			}
			return new string[] {
				intPart1 + (decPart1 != "" ? "." + decPart1 : ""),
				intPart2 + (decPart2 != "" ? "." + decPart2 : "")
			};
		}
		public static int FindDivider(this string num1, string num2)
		{
			for (int i = 1; i <= 10; i++) {
				string res = num2.Multiply(i.ToString());
				int comp = res.Compare(num1);
				if (comp == 1)
					return i - 1;
				if (comp == 0)
					return i;
			}
			
			throw new Exception("Divider must be in between 0-9");
		}
		public static string DivSubstring(this string num1, int start, int count)
		{
			if (start >= num1.Length) {
				return "".InsertZerosToEnd(count);
			}
			if (start + count > num1.Length) {
				string zeros = "".InsertZerosToEnd(start + count - num1.Length);
				string substr = num1.Substring(start, num1.Length - start);
				return substr + zeros;
			}
			return num1.Substring(start, count);
		}
		public static string BorrowingFromNeighbour(this string num1, string num2, int a)
		{
			for (int i = a - 1; i >= 0; i--) {
				string t1 = num1[i].ToString();
				string t2 = num2[i].ToString();
				
				if (t1 == ".")
					continue;
				
				int x = int.Parse(t1);
				int y = int.Parse(t2);
				
				if (x > 0)
					return num1.Change(i, (x - 1).ToString());
				
				num1 = num1.Change(i, "9");
			}
			return num1;
		}
	

		public static string InsertZerosToEnd(this string num, int numOfZeros)
		{
			for (int i = 0; i < numOfZeros; i++) {
				num += "0";
			}
			return num;
		}
		public static string FixNumber(this string woSignNum)
		{
			if (woSignNum[0] == '.')
				woSignNum = woSignNum.Insert(0, "0");
			
			return woSignNum;
		}
	}
}
