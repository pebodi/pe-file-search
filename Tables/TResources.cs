using System;
using Gtk;

namespace FileSearch
{
	public class TResources
	{
		public enum dataTable { Trud = 0, Materiali, Mehanizacia }

		private NodeView _view;
		
		public enum columns {
			filename = 0,		//00
			osnovanie,			//01
			ime,				//02
			mka,				//03
			dsr,				//04
			cena				//05
		};
		
		public	TreeViewColumn		colFileName		= new TreeViewColumn ();	//00
		public	TreeViewColumn		colOsnovanie	= new TreeViewColumn ();	//01
		public	TreeViewColumn		colIme			= new TreeViewColumn ();	//02
		public	TreeViewColumn		colMka			= new TreeViewColumn ();	//03
		public	TreeViewColumn		colDSR			= new TreeViewColumn ();	//04
		public	TreeViewColumn		colCena			= new TreeViewColumn ();	//05

		public	CellRendererText	cellFileName	= new CellRendererText ();	//00
		public	CellRendererText	cellOsnovanie	= new CellRendererText ();	//01
		public	CellRendererText	cellIme			= new CellRendererText ();	//02
		public	CellRendererText	cellMka			= new CellRendererText ();	//03
		public	CellRendererText	cellDSR			= new CellRendererText ();	//04
		public	CellRendererText	cellCena		= new CellRendererText ();	//05


		public TResources (NodeView view )
		{
			_view = view;
			
			createTable ();
			formatTable ();
		}
		


		private void createTable ()
		{
			createColumn ( colFileName,		cellFileName,	(int) columns.filename	);	//00
			createColumn ( colOsnovanie,	cellOsnovanie,	(int) columns.osnovanie	);	//01
			createColumn ( colIme,			cellIme,		(int) columns.ime		);	//02
			createColumn ( colMka,			cellMka,		(int) columns.mka		);	//03
			createColumn ( colDSR,			cellDSR,		(int) columns.dsr		);	//04
			createColumn ( colCena,			cellCena,		(int) columns.cena		);	//05
			
			colIme.Expand = true;
		}

		private void createColumn (TreeViewColumn column, CellRendererText cell, int id)
		{
			column.PackStart ( cell, true );
			column.AddAttribute ( cell, "text", id );
			_view.AppendColumn ( column );
		}

		private void formatTable ()
		{
			colFileName.Title	= "Файл";		colFileName.Resizable	= true;		cellFileName.Xalign = 0;	colFileName.MinWidth = 90;
			colFileName.Visible = false;

			colOsnovanie.Title	= "Основание";	colOsnovanie.Resizable	= true;		cellOsnovanie.Xalign = 0;	colOsnovanie.MinWidth = 90;
			colIme.Title		= "Описание";	colIme.Resizable		= true;		cellIme.Xalign = 0;			colIme.MaxWidth = 500;
			colMka.Title		= "м-ка";		colMka.Resizable		= true;		cellMka.Xalign = 0;			colMka.MinWidth = 40;
			colDSR.Title		= "ДСР";		colDSR.Resizable		= true;		cellDSR.Xalign = 1;			colDSR.MinWidth = 40;
			colCena.Title		= "Цена";		colCena.Resizable		= true;		cellCena.Xalign = 1;		colCena.MinWidth = 40;
		}
	}
}

