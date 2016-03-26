using System;
using Gtk;

namespace FileSearch
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow (args);
			win.Show ();
			Application.Run ();
		}
	}
}
