using System;
using Gtk;

namespace FileSearch
{
	public class TBase
	{
		private NodeView _view;
		private ListStore _store;

		public enum columns { filename = 0, osnovanie, opisanie, mka, nvr, sum, trd, mtr, mhn, usl, dop, pe, an, pepr };

		public	TreeViewColumn		colFileName		= new TreeViewColumn ();	//00
		public	TreeViewColumn		colOsnovanie	= new TreeViewColumn ();	//01
		public	TreeViewColumn		colOpisanie		= new TreeViewColumn ();	//02
		public	TreeViewColumn		colMka			= new TreeViewColumn ();	//03
		public	TreeViewColumn		colNvr			= new TreeViewColumn ();	//04
		public	TreeViewColumn		colSum			= new TreeViewColumn ();	//05
		public	TreeViewColumn		colTrd			= new TreeViewColumn ();	//06
		public	TreeViewColumn		colMtr			= new TreeViewColumn ();	//07
		public	TreeViewColumn		colMhn			= new TreeViewColumn ();	//08
		public	TreeViewColumn		colUsl			= new TreeViewColumn ();	//09
		public	TreeViewColumn		colDop			= new TreeViewColumn ();	//10
		public	TreeViewColumn		colPe			= new TreeViewColumn ();	//11
		public	TreeViewColumn		colAn			= new TreeViewColumn ();	//12
		public	TreeViewColumn		colPePr			= new TreeViewColumn ();	//13
		
		public	CellRendererText	cellFileName	= new CellRendererText ();	//00
		public	CellRendererText	cellOsnovanie	= new CellRendererText ();	//01
		public	CellRendererText	cellOpisanie	= new CellRendererText ();	//02
		public	CellRendererText	cellMka			= new CellRendererText ();	//03
		public	CellRendererText	cellNvr			= new CellRendererText ();	//04
		public	CellRendererText	cellSum			= new CellRendererText ();	//05
		public	CellRendererText	cellTrd			= new CellRendererText ();	//06
		public	CellRendererText	cellMtr			= new CellRendererText ();	//07
		public	CellRendererText	cellMhn			= new CellRendererText ();	//08
		public	CellRendererText	cellUsl			= new CellRendererText ();	//09
		public	CellRendererText	cellDop			= new CellRendererText ();	//10
		public	CellRendererText	cellPe			= new CellRendererText ();	//11
		public	CellRendererText	cellAn			= new CellRendererText ();	//12
		public	CellRendererText	cellPePr		= new CellRendererText ();	//13


		public TBase ( NodeView view, ListStore store )
		{
			_view = view;
			_store = store;
			
			_view.Model = _store;


			createTable ();
			formatTable ();
		}

		private void createTable ()
		{
			createColumn ( colFileName,		cellFileName,	(int) columns.filename	);	//00
			createColumn ( colOsnovanie,	cellOsnovanie,	(int) columns.osnovanie);	//01
			createColumn ( colOpisanie,		cellOpisanie,	(int) columns.opisanie	);	//02
			createColumn ( colMka,			cellMka,		(int) columns.mka		);	//03
			createColumn ( colNvr,			cellNvr,		(int) columns.nvr		);	//04
			createColumn ( colSum,			cellSum,		(int) columns.sum		);	//05
			createColumn ( colTrd,			cellTrd,		(int) columns.trd		);	//06
			createColumn ( colMtr,			cellMtr,		(int) columns.mtr		);	//07
			createColumn ( colMhn,			cellMhn,		(int) columns.mhn		);	//08
			createColumn ( colUsl,			cellUsl,		(int) columns.usl		);	//09
			createColumn ( colDop,			cellDop,		(int) columns.dop		);	//10
			createColumn ( colPe,			cellPe,			(int) columns.pe		);	//11
			createColumn ( colAn,			cellAn,			(int) columns.an		);	//12
			createColumn ( colPePr,			cellPePr,		(int) columns.pepr		);	//13

			colOpisanie.Expand = true;
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
			colOsnovanie.Title	= "Основание";	colOsnovanie.Resizable	= true;		cellOsnovanie.Xalign = 0;	colOsnovanie.MinWidth = 90;
			colOpisanie.Title	= "Описание";	colOpisanie.Resizable	= true;		cellOpisanie.Xalign = 0;
			colMka.Title		= "м-ка";		colMka.Resizable		= true;		cellMka.Xalign = 0;			colMka.MinWidth = 40;
			colNvr.Title		= "Нвр";		colNvr.Resizable		= true;		cellNvr.Xalign = 1;			colNvr.MinWidth = 40;
			colSum.Title		= "Общо";		colSum.Resizable		= true;		cellSum.Xalign = 1;			colSum.MinWidth = 40;
			colTrd.Title		= "Труд";		colTrd.Resizable		= true;		cellTrd.Xalign = 1;			colTrd.MinWidth = 40;
			colMtr.Title		= "Мат.";		colMtr.Resizable		= true;		cellMtr.Xalign = 1;			colMtr.MinWidth = 40;
			colMhn.Title		= "Мех.";		colMhn.Resizable		= true;		cellMhn.Xalign = 1;			colMhn.MinWidth = 40;
			colUsl.Title		= "Усл.";		colUsl.Resizable		= true;		cellUsl.Xalign = 1;			colUsl.MinWidth = 40;
			colDop.Title		= "Доп.";		colDop.Resizable		= true;		cellDop.Xalign = 1;			colDop.MinWidth = 40;
			colPe.Title			= "Печ.";		colPe.Resizable			= true;		cellPe.Xalign = 1;			colPe.MinWidth = 40;
			colAn.Title			= "Анализ";		colAn.Resizable			= true;		cellAn.Xalign = 1;			colAn.MinWidth = 300;
			colPePr.Title		= "Печ. [%]";	colPePr.Resizable		= true;		cellPePr.Xalign = 1;		colPePr.MinWidth = 40;
		}

		public void fixedSize (bool isFixedSize)
		{
			if (isFixedSize) {
				colOpisanie.Resizable = false;
				colOpisanie.MaxWidth = 350;
			} else {
				colOpisanie.Resizable = true;
				colOpisanie.MaxWidth = 900;
			}

		}
	
	}
}