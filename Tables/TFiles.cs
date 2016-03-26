using System;
using Gtk;

namespace FileSearch
{
	public class TFiles
	{
		private NodeView _view;
		private ListStore _store;

		private TreeViewColumn		colFileName		= new TreeViewColumn ();
		private TreeViewColumn		colCheck		= new TreeViewColumn ();

		private CellRendererText	cellFileName	= new CellRendererText ();
		private CellRendererToggle	cellCheck		= new CellRendererToggle ();


		public TFiles (NodeView view, ListStore store)
		{
			_view = view;
			_store = store;

			_view.Model = _store;

			createTable ();
			formatTable ();
		}

		private void createTable ()
		{
			colFileName.PackStart		( cellFileName, true );
			colCheck.PackEnd			( cellCheck,	true);

			colFileName.AddAttribute 	( cellFileName, "text", 0);
			colCheck.AddAttribute		( cellCheck, "active", 1);
			cellCheck.Toggled += onCellCheckToggled;

			_view.AppendColumn ( colFileName );
			_view.AppendColumn ( colCheck );

			colFileName.Expand = true;
		}

		private void formatTable ()
		{
			colFileName.Title = "Име на файл";
			colCheck.Title = "Включи в БД";
		}

		void onCellCheckToggled(object o, ToggledArgs args) {
			TreeIter iter;
			
			if ( _store.GetIter (out iter, new TreePath(args.Path)) ) {
				bool old = (bool) _store.GetValue(iter, 1);
				_store.SetValue(iter, 1, !old);
			}
		}

		public void markAll ()
		{
			try {
				TreeIter _it;
				bool allIsMark = true;

				for (int i = 0; i < _store.IterNChildren (); i++ )
				{
					_store.IterNthChild ( out _it, i );
					allIsMark = allIsMark && (bool) _store.GetValue ( _it, 1 );
				}

				for (int i = 0; i < _store.IterNChildren (); i++ )
				{
					_store.IterNthChild ( out _it, i );
					_store.SetValue ( _it, 1, !allIsMark );
				}

			} catch {
				Console.WriteLine ("Грешка при маркиране / размаркиране");
			}
		}

		public void loadFiles (string _baseDir)
		{
			try {
				if ( System.IO.Directory.Exists ( _baseDir ) )
				{
					_store.Clear ();

					string[] file = System.IO.Directory.GetFiles ( _baseDir, "*.efo" );

					if ( file.Length > 0 )
					{
						for ( int i = 0; i < file.Length; i++ )
						{
							_store.AppendValues ( 
								System.IO.Path.GetFileName ( file[i] ),
							    false
							);

						}
					}
				}

			} catch {
			}
		}

		public bool readFiles (ListStore _dataStore, string _baseDir, ListStore _storeTrd, ListStore _storeMtr, ListStore _storeMhn)
		{
			try {
				_dataStore.Clear ();
				_storeTrd.Clear ();
				_storeMtr.Clear ();
				_storeMhn.Clear ();

				TreeIter _it;

				for ( int i = 0; i < _store.IterNChildren (); i++ )
				{
					if ( _store.IterNthChild ( out _it, i ) && (bool) _store.GetValue ( _it, 1 ) )
					{
						collectRowsFromFile (
							System.IO.Path.Combine ( _baseDir, (string) _store.GetValue ( _it, 0 ) ),
							_dataStore,
							_storeTrd, _storeMtr, _storeMhn
						);
					}
				}

				Console.WriteLine ( "Видове работи: " + _dataStore.IterNChildren ().ToString () );

				Console.WriteLine ( "Часови ставки: " + _storeTrd.IterNChildren ().ToString () );
				Console.WriteLine ( "Материали: " + _storeMtr.IterNChildren ().ToString () );
				Console.WriteLine ( "Машиносмени: " + _storeMhn.IterNChildren ().ToString () );

				return true;
			} catch {
				Console.WriteLine ("Грешка при разчитането на спистка с файлове");
			}

			return false;
		}

		public string collectPeFromFile (string _file)
		{
			try {
				string startTag = "[печалба]";
				string finishTag = "[\\печалба]";

				if ( _file.Contains (startTag ) )
				{
					int start = _file.IndexOf ( startTag ) + startTag.Length;
					int finish = _file.IndexOf (finishTag );

					string _pe = _file.Substring ( start, finish - start ).Replace ( "\r", "" ).Replace ( "\n", "" );

					return _pe.Split ( '@' )[1];
				}

			} catch {
				Console.WriteLine ( "TFiles.collectPeFromFile: Грешка при разчитане на процента на печалбата от файла" );
			}

			return "10";
		}

		public bool collectRowsFromFile (string _filePath, ListStore _dataStore, ListStore _storeTrd, ListStore _storeMtr, ListStore _storeMhn)
		{
			try {

				string textfile = System.IO.File.ReadAllText ( _filePath );

				string pe = collectPeFromFile ( textfile );

				string startTag = "[основни данни]";
				string finishTag = "[\\основни данни]";




				if ( textfile.Contains ( startTag )  )
				{
					int start = textfile.IndexOf ( startTag ) + startTag.Length;
					int finish = textfile.IndexOf ( finishTag );


					string[] row = ( textfile.Substring ( start, finish - start ) ).Replace ("\r", "").Split ('\n');

					;

					if ( row.Length > 0 )
					{

						string description = "";
						string dimension = "";
						string anCode = "";

						string fileName = System.IO.Path.GetFileName ( _filePath );

						for (int i = 0; i < row.Length; i++ )
						{
							if ( row[i] != "" && row[i].Contains ( "\t") )
							{
								string[] value = row[i].Split ('\t');
								description = value[0];
								dimension = value[1];
								anCode = value[3];

								if ( dimension != "" )
								{

									FileSearch.FSAn an = new FSAn ( anCode, pe );


									collectResources ( an, _storeTrd, _storeMtr, _storeMhn );

									_dataStore.AppendValues ( fileName, an.osn, description, dimension,
									                         String.Format ( "{0:0.0000}", Convert.ToDouble( an.nvr ) ),
									                         an.sum,
									                       	 an.trd,
									                         an.mtr,
									                         an.mhn,
									                         an.usl,
									                         an.dop,
									                         an.pe4,
									                         anCode,
									                         pe
									                         );
								}
							}
						}

					}
				}

				return true;
			} catch {
				Console.WriteLine ( "TFiles.collectRowsFromFile: Грешка при разчитането на файл: " + _filePath );
			}

			return false;
		}

		private bool resNotExist (ListStore _storeTrd, ListStore _storeMtr, ListStore _storeMhn,
		                       string osn,
		                       string opi,
		                       string mka,
		                       string kof,
		                       string cen,
		                       string tpe
		)
		{
			try
			{
				bool _ret = true;
				string _temp = "";
				string _input =
					osn + 
					opi +
					mka + 
					kof +
					cen;
				TreeIter _iter;

				if (tpe == "trd") {
					for ( int i = 0; i < _storeTrd.IterNChildren (); i++ )
					{
						_storeTrd.IterNthChild ( out _iter, i );
						
						_temp =
							(string) _storeTrd.GetValue ( _iter, 1 ) +
								(string) _storeTrd.GetValue ( _iter, 2 ) +
								(string) _storeTrd.GetValue ( _iter, 3 ) +
								(string) _storeTrd.GetValue ( _iter, 4 ) +
								(string) _storeTrd.GetValue ( _iter, 5 );
						
						if ( _input == _temp )	return false;
					}
				}

				if (tpe == "mtr") {
					for ( int i = 0; i < _storeMtr.IterNChildren (); i++ )
					{
						_storeMtr.IterNthChild ( out _iter, i );
						
						_temp =
							(string) _storeMtr.GetValue ( _iter, 1 ) +
							(string) _storeMtr.GetValue ( _iter, 2 ) +
							(string) _storeMtr.GetValue ( _iter, 3 ) +
							(string) _storeMtr.GetValue ( _iter, 4 ) +
							(string) _storeMtr.GetValue ( _iter, 5 );
						
						if (
							_input.Replace ( " ", "" ).ToLower () ==
						    _temp.Replace ( " ", "" ).ToLower () )	return false;
					}
				}

				if (tpe == "mhn") {
					for ( int i = 0; i < _storeMhn.IterNChildren (); i++ )
					{
						_storeMhn.IterNthChild ( out _iter, i );
						
						_temp =
							(string) _storeMhn.GetValue ( _iter, 1 ) +
							(string) _storeMhn.GetValue ( _iter, 2 ) +
							(string) _storeMhn.GetValue ( _iter, 3 ) +
							(string) _storeMhn.GetValue ( _iter, 4 ) +
							(string) _storeMhn.GetValue ( _iter, 5 );
						
						if ( 
						    _input.Replace ( " ", "" ).ToLower () ==
						    _temp.Replace ( " ", "" ).ToLower () )	return false;
					}
				}

			}catch{}

			return true;
		}

		public void collectResources (FileSearch.FSAn _an, ListStore _storeTrd, ListStore _storeMtr, ListStore _storeMhn)
		{
			try {

				for (int i = 0; i < _an.rows.Count; i++ )
				{

					if ( _an.rows[i].tpe == "trd" )
						if ( resNotExist ( _storeTrd, _storeMtr, _storeMhn,
						                  _an.rows[i].osn,
						                  _an.rows[i].opi,
						                  _an.rows[i].mka,
						                  _an.rows[i].kof,
						                  String.Format ( "{0:0.00}", Convert.ToDouble( _an.rows[i].cen ) ),
						                  "trd"
						                  ) )
						{
							_storeTrd.AppendValues (
								_an.rows[i].osn,
								_an.rows[i].osn,
								_an.rows[i].opi,
								_an.rows[i].mka,
								_an.rows[i].kof,
								String.Format ( "{0:0.00}", Convert.ToDouble( _an.rows[i].cen ) )
								);
						}

					if ( _an.rows[i].tpe == "mtr" )
						if ( resNotExist ( _storeTrd, _storeMtr, _storeMhn,
						                  _an.rows[i].osn,
						                  _an.rows[i].opi,
						                  _an.rows[i].mka,
						                  _an.rows[i].kof,
						                  String.Format ( "{0:0.00}", Convert.ToDouble( _an.rows[i].cen ) ),
						                  "mtr"
						                  ) )
						{
							_storeMtr.AppendValues (
								_an.rows[i].osn,
								_an.rows[i].osn,
								_an.rows[i].opi,
								_an.rows[i].mka,
								_an.rows[i].kof,
								String.Format ( "{0:0.00}", Convert.ToDouble( _an.rows[i].cen ) )
								);
						}

					if ( _an.rows[i].tpe == "mhn" )
						if ( resNotExist ( _storeTrd, _storeMtr, _storeMhn,
						                  _an.rows[i].osn,
						                  _an.rows[i].opi,
						                  _an.rows[i].mka,
						                  _an.rows[i].kof,
						                  String.Format ( "{0:0.00}", Convert.ToDouble( _an.rows[i].cen ) ),
						                  "mhn"
						                  ) )
						{
							_storeMhn.AppendValues (
								_an.rows[i].osn,
								_an.rows[i].osn,
								_an.rows[i].opi,
								_an.rows[i].mka,
								_an.rows[i].kof,
								String.Format ( "{0:0.00}", Convert.ToDouble( _an.rows[i].cen ) )
								);
						}
				}

			} catch {
				Console.WriteLine ( "TFiles.collectResources: Грешка при парсване на редове от анализ.");
			}
		}

		public string getSelectedFiles ()
		{
			string _ret = "";

			try {
				TreeIter _it;

				for (int i = 0; i < _store.IterNChildren (); i++ )
				{
					_store.IterNthChild ( out _it, i );

					if ( (bool) _store.GetValue ( _it, 1) )
					{
						_ret += (string) _store.GetValue ( _it, 0 ) + ",";
					}
				}

				_ret = _ret.TrimEnd ( ',' );

			} catch {
			}

			return _ret;
		}

		public void setSelectedFiles (string files)
		{
			try {
				string[] file = files.Split (',');
				TreeIter _row;

				for ( int i = 0; i < _store.IterNChildren (); i++ )	//iters
				{
					_store.IterNthChild ( out _row, i );
					string currentFile = (string) _store.GetValue ( _row, 0 );

					for (int f = 0; f < file.Length; f++ )
					{
						if ( currentFile == file[f] )	_store.SetValue ( _row, 1, true);
					}

				}
			
			} catch {
			}
		}


	}
}

