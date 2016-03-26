using System;
using Gtk;


namespace FileSearch
{
	public partial class anDialog : Gtk.Dialog
	{
		public anDialog (string an, string pe, string osnovanie, string opisanie, string mka, string filename)
		{
			this.Build ();

			this.ActionArea.Visible = false;
			this.HasSeparator = false;

			label2.Text = "<b>" + opisanie + "</b>";	label2.UseMarkup = true;
			label4.Text = "<b>" + mka + "</b>";			label4.UseMarkup = true;
			label6.Text = "<b>" + osnovanie + "</b>";	label6.UseMarkup = true;
			label10.Text = filename.Replace ( ".efo", "" );

			this.Title = opisanie + "  [" + mka + "]";

			setAn ( an, pe );
		}

		private void setAn (string an, string pe)
		{
			try {
				FSAn _anCalc = new FSAn ( an, pe );

				labelTrdO.Text = "Труд ["			+ _anCalc.dopPrArray[0] + "%]:";	labelTrdV.Text = _anCalc.dopArray[0];
				labelMtrO.Text = "Материали ["		+ _anCalc.dopPrArray[1] + "%]:";	labelMtrV.Text = _anCalc.dopArray[1];
				labelMhnO.Text = "Механизация ["	+ _anCalc.dopPrArray[2] + "%]:";	labelMhnV.Text = _anCalc.dopArray[2];
				labelUslO.Text = "Услуги ["			+ _anCalc.dopPrArray[3] + "%]:";	labelUslV.Text = _anCalc.dopArray[3];

				labelDopV.Text = _anCalc.dop;

				labelPrekiV.Text = _anCalc.preki;
				labelPDV.Text = _anCalc.pd;
				labelPeO.Text = "Печалба ["			+ _anCalc._pe + "%]:";				labelPeV.Text = _anCalc.pe4;
				labelSumV.Text = _anCalc.sum;

				anTable _anTable = new anTable ( treeview1, _anCalc );
				_anTable.setAn ( an.Split ( '@' )[1], _anCalc );

			} catch {
			}
		}
	
	}

	public class anTable
	{
		//Основни данни
		public TreeStore _store = new TreeStore (
			typeof (string),
			typeof (string),
			typeof (string),
			typeof (string),
			typeof (string),
			typeof (string),
			typeof (string),
			typeof (string),
			typeof (string)
			);
		
		public TreeView _view;

		
		//Дефиниране на колони
		private TreeViewColumn colOSN			= new TreeViewColumn ();	//01
		private TreeViewColumn colName			= new TreeViewColumn ();	//02
		private TreeViewColumn colMka			= new TreeViewColumn ();	//03
		private TreeViewColumn colRN			= new TreeViewColumn ();	//04
		private TreeViewColumn colK				= new TreeViewColumn ();	//05
		private TreeViewColumn colPrice			= new TreeViewColumn ();	//06
		private TreeViewColumn colSum			= new TreeViewColumn ();	//07
		private TreeViewColumn colIndicator		= new TreeViewColumn ();	//08
		private TreeViewColumn colType			= new TreeViewColumn ();	//09
		
		//Дефиниране на клетки
		private CellRendererText cellOSN		= new CellRendererText ();	//01
		private CellRendererText cellName		= new CellRendererText ();	//02
		private CellRendererText cellMka		= new CellRendererText ();	//03
		private CellRendererText cellRN			= new CellRendererText ();	//04
		private CellRendererText cellK			= new CellRendererText ();	//05
		private CellRendererText cellPrice		= new CellRendererText ();	//06
		private CellRendererText cellSum		= new CellRendererText ();	//07
		private CellRendererText cellIndicator	= new CellRendererText ();	//08
		private CellRendererText cellType		= new CellRendererText ();	//09
		
		public anTable ( TreeView view, FSAn _anCalc )
		{
			_view = view;
			
			_view.Model = _store;
			
			createTable ();
			formatTable ();
			

			
			_view.ExpanderColumn = colName;
			_view.Reorderable = false;
		}
		
		private void createTable ()
		{
			try {
				colOSN.PackStart			(cellOSN,		true);		//01
				colName.PackStart			(cellName,		true);		//02
				colMka.PackEnd				(cellMka,		true);		//03
				colRN.PackEnd 				(cellRN,		true);		//04
				colK.PackEnd				(cellK,			true);		//05
				colPrice.PackEnd			(cellPrice,		true);		//06
				colSum.PackEnd				(cellSum,		true);		//07
				colIndicator.PackEnd		(cellIndicator,	true);		//08
				colType.PackEnd				(cellType,		true);		//09
				
				colOSN.AddAttribute			(cellOSN,		"text",	00);	//01
				colName.AddAttribute		(cellName,		"text", 01);	//02
				colMka.AddAttribute			(cellMka,		"text",	02);	//03
				colRN.AddAttribute			(cellRN,		"text",	03);	//04
				colK.AddAttribute			(cellK,			"text",	04);	//05
				colPrice.AddAttribute		(cellPrice,		"text", 05);	//06
				colSum.AddAttribute			(cellSum,		"text",	06);	//07
				colIndicator.AddAttribute	(cellIndicator,	"text",	07);	//08
				colType.AddAttribute		(cellType,		"text",	08);	//09
				
				_view.AppendColumn ( colOSN			);				//01
				_view.AppendColumn ( colName		);				//02
				_view.AppendColumn ( colMka			);				//03
				_view.AppendColumn ( colRN			);				//04
				_view.AppendColumn ( colK			);				//05
				_view.AppendColumn ( colPrice		);				//06
				_view.AppendColumn ( colSum			);				//07
				_view.AppendColumn ( colIndicator	);				//08
				_view.AppendColumn ( colType		);				//09
				
				colName.Expand = true;
				
			} catch {
				
			}
		}
		
		private void formatColumn (TreeViewColumn col, CellRendererText cell, string title, int width, bool editable, bool resizable, bool visible)
		{
			try {
				col.MinWidth = width;
				col.Title = title;
				col.Resizable = resizable;
				col.Visible = visible;
				
				cell.Editable = editable;
				
				col.SetCellDataFunc	(cell,	new  Gtk.TreeCellDataFunc (RenderStatus));
			} catch {
			}
		}
		
		private void formatTable ()
		{
			try {
				formatColumn ( colOSN,			cellOSN,		"Основание",	80, true, true, true );	//01
				formatColumn ( colName,			cellName,		"Описание",		250, true, true, true );	//02
				formatColumn ( colMka,			cellMka,		"м-ка",			30, true, true, true );	//03
				formatColumn ( colRN,			cellRN,			"РН",			60, true, true, true );	//04
				formatColumn ( colK,			cellK,			"К", 			60, true, true, true   );	//05
				formatColumn ( colPrice,		cellPrice,		"Цена",			60, true, true, true );	//06
				formatColumn ( colSum,			cellSum,		"Общо",			60, true, true, true );							//08
				formatColumn ( colIndicator,	cellIndicator,	"Мат.",			60, true, true, true );							//07
				formatColumn ( colType,			cellType,		"Усл.",			60, true, true, true );							//09
				
				
				colRN.Alignment = 1;		cellRN.Xalign = 1;
				colK.Alignment = 1;			cellK.Xalign = 1;
				colPrice.Alignment = 1;		cellPrice.Xalign = 1;
				colSum.Alignment = 1;		cellSum.Xalign = 1;
				
				colIndicator.Visible = false;
				colType.Visible = false;

			} catch {
			}
		}
		
		//Функция за форматиране
		private void RenderStatus (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			try {
				
				bool _group		= ( (string) _store.GetValue ( iter, 5 ) ).Length == 0;
				bool _row		= ( (string) _store.GetValue ( iter, 5 ) ).Length > 0;
				
				//Дебелина на шрифта
				int _weight = 400;	if ( _group || column == colMka || column == colSum || column == colOSN ) _weight = 700;
				
				(cell as Gtk.CellRendererText).Weight = _weight;
				
				
				
				if ( _row ) {
					//Възстановяване на цветовата схема
					(cell as Gtk.CellRendererText).CellBackgroundGdk = new  Gdk.Color ( 255, 255, 255);
					
					
//					if ( column == colOSN )
//					{
//						(cell as Gtk.CellRendererText).ForegroundGdk = _settings.colorsForAn.fg_light;
//						(cell as Gtk.CellRendererText).CellBackgroundGdk = _settings.colorsForAn.bg_dark;
//					}
//					
//					if ( column == colName )
//					{
//						(cell as Gtk.CellRendererText).ForegroundGdk = _settings.colorsForAn.fg_normal;
//						(cell as Gtk.CellRendererText).CellBackgroundGdk = _settings.colorsForAn.bg_main;
//					}
//					
//					if ( column == colMka )
//					{
//						(cell as Gtk.CellRendererText).ForegroundGdk = _settings.colorsForAn.fg_normal;
//						(cell as Gtk.CellRendererText).CellBackgroundGdk = _settings.colorsForAn.bg_dark;
//					}
//					
//					if ( column == colRN )
//					{
//						(cell as Gtk.CellRendererText).ForegroundGdk = _settings.colorsForAn.fg_dark;
//						(cell as Gtk.CellRendererText).CellBackgroundGdk = _settings.colorsForAn.bg_main;
//					}
//					
//					if ( column == colK )
//					{
//						(cell as Gtk.CellRendererText).ForegroundGdk = _settings.colorsForAn.fg_normal;
//						(cell as Gtk.CellRendererText).CellBackgroundGdk = _settings.colorsForAn.bg_main;
//					}
//					
//					if ( column == colPrice )
//					{
//						(cell as Gtk.CellRendererText).ForegroundGdk = _settings.colorsForAn.fg_ultra_dark;
//						(cell as Gtk.CellRendererText).CellBackgroundGdk = _settings.colorsForAn.bg_main;
//					}
//					
//					if ( column == colSum )
//					{
//						(cell as Gtk.CellRendererText).ForegroundGdk = _settings.colorsForAn.fg_normal;
//						(cell as Gtk.CellRendererText).CellBackgroundGdk = _settings.colorsForAn.bg_dark;
//					}
					
					
					
					
					
				} 
				
				if ( _group ) {
					(cell as Gtk.CellRendererText).CellBackgroundGdk = new  Gdk.Color ( 150, 150, 150);
				}
				
				
				
				
			} catch {
			}
		}
		
		//Добавяне на анализ
		public bool setAn ( string _code, FSAn _anCalc )
		{
			try {
				string[] row = _code.Split ( '$' );
				
				_store.Clear ();
				
				//Проверка за типове
				bool hasTr = _code.Contains ( "trd" );
				bool hasMt = _code.Contains ( "mtr" );
				bool hasMh = _code.Contains ( "mhn" );
				bool hasUs = _code.Contains ( "usl" );
				
				int posTr = Convert.ToInt32 ( hasTr ) - 1;
				int posMt = Convert.ToInt32 ( hasTr ) + Convert.ToInt32 ( hasMt ) - 1;
				int posMh = Convert.ToInt32 ( hasTr ) + Convert.ToInt32 ( hasMt ) + Convert.ToInt32 ( hasMh ) - 1;
				int posUs = Convert.ToInt32 ( hasTr ) + Convert.ToInt32 ( hasMt ) + Convert.ToInt32 ( hasMh ) + Convert.ToInt32 ( hasUs )- 1;
				
				if ( hasTr ) _store.AppendValues ( "", "Труд",			"Нвр", _anCalc.nvr, "", "", _anCalc.trd, "", "trd" );
				if ( hasMt ) _store.AppendValues ( "", "Материали",		"", "", "", "", _anCalc.mtr, "", "mtr" );
				if ( hasMh ) _store.AppendValues ( "", "Механизация",	"", "", "", "", _anCalc.mhn, "", "mhn" );
				if ( hasUs ) _store.AppendValues ( "", "Услуги",		"", "", "", "", _anCalc.usl, "", "usl" );
				
				
				//Добавяне на типове
				TreeIter _type;	_store.GetIterFirst ( out _type );
				
				
				for (int i = 0; i < row.Length; i++ )
				{
					//Проверка за тип
					
					if ( row[i].Contains ("trd") ) _store.IterNthChild ( out _type, posTr );
					if ( row[i].Contains ("mtr") ) _store.IterNthChild ( out _type, posMt );
					if ( row[i].Contains ("mhn") ) _store.IterNthChild ( out _type, posMh );
					if ( row[i].Contains ("usl") ) _store.IterNthChild ( out _type, posUs );
					
					//Добавяне на ред
					string[] value = row[i].Split ( '^' );
					
					_store.AppendValues (
						_type,
						value [0],
						value [1],
						value [2],
						value [3],
						value [4],
						value [5],
						value [6],
						"",
						value [7]
						);
					
					
				}
				
				_view.ExpandAll ();
				
				return true;
			} catch {
				Console.WriteLine ( "Грешка при рендването на анализ" );
			}
			
			return false;
		}
	}
}

