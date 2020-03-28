/*
 * Created by SharpDevelop.
 * User: kafeinaltor
 * Date: 20.03.2020
 * Time: 19:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
namespace StringMath
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			Random rnd = new Random();
			
			Debug.WriteLine("0.13".Sum("0.190", true).Simplify());
			
			
		}
		
		public string[] iclerdislar(string a1, string b1, string a2, string b2)
		{
			string[] arr = new string[2];
			arr[0] = a1.Multiply(b2).Sum(a2.Multiply(b1), false);
			arr[1] = b1.Multiply(b2);
			return arr;
		}
	}
}
