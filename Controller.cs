using System;
using System.Threading;

using Gtk;

namespace FileSearch
{
	public class Controller
	{
		//Навигация
		public enum tabPage {
			UserWellcome = 0,
			DataBase,
			DataBaseResources,
			Update,
			Settings
		}

		//Настройки в кода
		private bool threadStop = false;

		//Файлова структура и данни
		FSLib _data = new FSLib ();
		FSSamples _samples = new FSSamples ();

		public ListStore	_storeFilesFirst = new ListStore (
			typeof (string),
			typeof (bool)
			);

		//Филтри
		TreeModelFilter _filter;
		TreeModelFilter _filterTrd;
		TreeModelFilter _filterMtr;
		TreeModelFilter _filterMhn;

		//Библиотеки
		private TFiles		_tableFiles;
		private TFiles		_tableFilesFirst;
		private TBase		_tableBase;
		private TResources	_tableResoures;
		private FSSettings	_settings;
		private FSInfo		_info = new FSInfo ();

		//Компоненти
		private Window		_mainWindow;
		private string[]	args;
		private Notebook	_mainNotebook;	//Основно разпределение на екраните


		//00. First Time Run
		private FileChooserButton _firstDirChoise;
		private Button		_btnFristDirOpen;
		private Button		_btnFristMarkAll;
		private Button		_btnFirstSetSamples;
		private Button		_btnGoToMainScreen;
		private Button		_btnFristTableRefresh;
		private NodeView	_viewFirstScreen;


		//01. Търсене
		private Label		_searchLebal;
		private Entry		_entrySearch;
		private NodeView	_viewSearch;
		private Button		_btnSearch;
		private Button		_btnUpdate;
		private Button		_btnSettings;
		private Button		_btnCopyRow;
		private Button		_btnCopyAn;
		private Button		_btnGoToRes;

		//02. Ресурси
		private Button		_btnBackRes;
		private Label		_searchLabelRes;
		private Entry		_entrySearchRes;
		private ComboBox	_comboResources;
		private NodeView	_viewResources;
		private Button		_btnCopyResRow;

		//02. Актуализация
		private NodeView	_viewFiles;
		private Button		_btnFFBack;
		private Button		_btnFFContainFolder;
		private Button		_btnFFUpdate;
		private Button		_btnFFMark;

		//03. Настройки
		private Button				_btnBackSettings;
		private FileChooserButton	_DirChoiseSettings;
		private Button				_btnRunBaseDataSettings;
		private Button				_btnRunAppDataSettings;

		private RadioButton			_radioAnd;
		private RadioButton			_radioOr;
		private RadioButton			_radioOff;
		private CheckButton			_checkFixedColumn;

		private CheckButton			_checkColFileName;
		private CheckButton			_checkColOsnovanie;
		private CheckButton			_checkColOpisanie;
		private CheckButton			_checkColMka;
		private CheckButton			_checkColNvr;
		private CheckButton			_checkColSum;
		private CheckButton			_checkColTrd;
		private CheckButton			_checkColMtr;
		private CheckButton			_checkColMhn;
		private CheckButton			_checkColUsl;
		private CheckButton			_checkColDop;
		private CheckButton			_checkColPe;
		private CheckButton			_checkColAn;
		private CheckButton			_checkColPePr;

		public Controller (
			Window			mainWindow,
			string[]		_args,
			Notebook		mainNotebook,

			//00. First Time Run
			FileChooserButton firstDirChoise,
			Button			btnFristDirOpen,
			Button			btnFristMarkAll,
			Button			btnFirstSetSamples,
			Button			btnGoToMainScreen,
			Button			btnFristTableRefresh,
			NodeView		viewFirstScreen,
			
			//01. Търсене
			Label			searchLabel,
			Entry			entrySearch,
			NodeView		viewSearch,
			Button			btnSearch,
			Button			btnUpdate,
			Button			btnSettings,
			Button			btnCopyRow,
			Button			btnCopyAn,
			Button			btnGoToRes,


			//02. База данни ресурси
			Button			btnBackRes,
			Label			searchLabelRes,
			Entry			entrySearchRes,
			ComboBox		comboResources,
			NodeView		viewResources,
			Button			btnCopyResRow,
			
			//03. Актуализация
			NodeView		viewFiles,
			Button			btnFFBack,
			Button			btnFFContainFolder,
			Button			btnFFUpdate,
			Button			btnFFMark,
			
			//04. Настройки
			Button				btnBackSettings,
			FileChooserButton	DirChoiseSettings,
			Button				btnRunBaseDirSettings,
			Button				btnRunAppDataSettings,

			RadioButton			radioAnd,
			RadioButton			radioOr,
			RadioButton			radioOff,
			CheckButton			checkFixedColumn,

			CheckButton			checkColFileName,
			CheckButton			checkColOsnovanie,
			CheckButton			checkColOpisanie,
			CheckButton			checkColMka,
			CheckButton			checkColNvr,
			CheckButton			checkColSum,
			CheckButton			checkColTrd,
			CheckButton			checkColMtr,
			CheckButton			checkColMhn,
			CheckButton			checkColUsl,
			CheckButton			checkColDop,
			CheckButton			checkColPe,
			CheckButton			checkColAn,
			CheckButton			checkColPePr


		)
		{
			_mainWindow = mainWindow;
			_mainWindow.Title = _info.shortName + " [" + _info.progVersion + "]";
			_mainNotebook = mainNotebook;

			_mainNotebook.ShowBorder = false;
			_mainNotebook.ShowTabs = false;

			args = _args;


			//00. First Time Run
			_firstDirChoise = firstDirChoise;
			_firstDirChoise.SelectionChanged += OnFistDirChoiseSelectionChanged;

			_btnFristDirOpen = btnFristDirOpen;
			_btnFristDirOpen.Clicked += OnBtnFirstDirOpen;
			_btnFristMarkAll = btnFristMarkAll;
			_btnFristMarkAll.Clicked += OnFirstMarkAll;
			_btnFirstSetSamples = btnFirstSetSamples;
			_btnFirstSetSamples.Clicked += OnBtnFirstSetSamples;
			_btnGoToMainScreen = btnGoToMainScreen;
			_btnFristTableRefresh = btnFristTableRefresh;
			_btnFristTableRefresh.Clicked += OnBtnFristTableRefreshClick;
			_btnGoToMainScreen.Clicked += OnBtnGoToMainScreen;
			_viewFirstScreen = viewFirstScreen;


			//01. Търсене
			_searchLebal = searchLabel;
			_entrySearch = entrySearch;

			_viewSearch = viewSearch;
			_viewSearch.RowActivated += OnViewSearchRowActivated;
			_btnSearch = btnSearch;
			_btnSearch.Visible = false;
			_btnUpdate = btnUpdate;
			_btnUpdate.Clicked += OnBtnUpdateClicked;
			_btnSettings = btnSettings;
			_btnSettings.Clicked += OnBtnSettingsClicked;
			_btnCopyRow = btnCopyRow;
			_btnCopyRow.Clicked += OnBtnCopyRow;
			_btnCopyAn = btnCopyAn;
			_btnCopyAn.Clicked += OnBtnCopyAn;
			_btnGoToRes = btnGoToRes;
			_btnGoToRes.Clicked += OnBtnGoToRes;

			//02. Ресурси
			_btnBackRes = btnBackRes;
			_btnBackRes.Clicked += OnBtnBackRes;
			_searchLabelRes = searchLabelRes;
			_entrySearchRes = entrySearchRes;
			_entrySearchRes.Changed += OnFilterEntryResTextChanged;
			_comboResources = comboResources;
			_comboResources.Changed += OnComboboxSearchInDBChanged;
			setupComboBox ();
			_viewResources = viewResources;
			_btnCopyResRow = btnCopyResRow;
			_btnCopyResRow.Clicked += OnBtnCopyRes;

			//03. Актуализация
			_viewFiles = viewFiles;
			_btnFFBack = btnFFBack;
			_btnFFBack.Clicked += OnBtnFFBackClicked;
			_btnFFContainFolder = btnFFContainFolder;
			_btnFFContainFolder.Clicked += OnBtnOpenFolderClicked;
			_btnFFUpdate = btnFFUpdate;
			_btnFFUpdate.Clicked += OnBtnUpdateFilesClicked;
			_btnFFMark = btnFFMark;
			_btnFFMark.Clicked += OnBtnFFMarkClicked;

			//04. Настройки
			_btnBackSettings = btnBackSettings;
			_btnBackSettings.Clicked += OnBtnBackSettingsClicked;
			_DirChoiseSettings = DirChoiseSettings;
			_DirChoiseSettings.SelectionChanged += OnDirChoiseSettingsSelectionChanged;
			_btnRunBaseDataSettings = btnRunBaseDirSettings;
			_btnRunBaseDataSettings.Clicked += OnBtnRunBaseDirSettingsClicked;
			_btnRunAppDataSettings = btnRunAppDataSettings;
			_btnRunAppDataSettings.Clicked += OnBtnRunAppDataSettingsClicked;

			_radioAnd = radioAnd;
			_radioOr = radioOr;
			_radioOff = radioOff;
			_checkFixedColumn = checkFixedColumn;

			_checkColFileName = checkColFileName;
			_checkColFileName.Toggled += OnCheckColToggled;
			_checkColOsnovanie = checkColOsnovanie;
			_checkColOsnovanie.Toggled += OnCheckColToggled; 
			_checkColOpisanie = checkColOpisanie;
			_checkColOpisanie.Toggled += OnCheckColToggled;
			_checkColMka = checkColMka;
			_checkColMka.Toggled += OnCheckColToggled;
			_checkColNvr = checkColNvr;
			_checkColNvr.Toggled += OnCheckColToggled;
			_checkColSum = checkColSum;
			_checkColSum.Toggled += OnCheckColToggled;
			_checkColTrd = checkColTrd;
			_checkColTrd.Toggled += OnCheckColToggled;
			_checkColMtr = checkColMtr;
			_checkColMtr.Toggled += OnCheckColToggled;
			_checkColMhn = checkColMhn;
			_checkColMhn.Toggled += OnCheckColToggled;
			_checkColUsl = checkColUsl;
			_checkColUsl.Toggled += OnCheckColToggled;
			_checkColDop = checkColDop;
			_checkColDop.Toggled += OnCheckColToggled;
			_checkColPe = checkColPe;
			_checkColPe.Toggled += OnCheckColToggled;
			_checkColAn = checkColAn;
			_checkColAn.Toggled += OnCheckColToggled;
			_checkColPePr = checkColPePr;
			_checkColPePr.Toggled += OnCheckColToggled;




			//Библиотеки
			_tableFiles = new TFiles (_viewFiles, _data.storeFiles);
			_tableBase = new TBase (_viewSearch, _data.storeBase);


			_tableResoures = new TResources (_viewResources);

			_settings = new FSSettings ();

			if ( _settings.BaseDir != "" ) {
				_mainNotebook.Page = (int)tabPage.DataBase;
			
				_tableFiles.loadFiles (_settings.BaseDir);
				_tableFiles.setSelectedFiles (_settings.CheckedFiles);




				if ( threadStop ) {

					if (!_data.readDB ()) {
									_tableFiles.readFiles (
										_data.storeBase, _settings.BaseDir,
										_data.storeTrd,
										_data.storeMtr,
										_data.storeMhn
									);
					}


					_entrySearch.Sensitive = true;
					_entrySearch.Text = "";

					_entrySearch.Changed += OnFilterEntryTextChanged;
					_entrySearch.GrabFocus ();
				
					_viewSearch.Visible = true;
				
					_DirChoiseSettings.SetUri (_settings.BaseDir);
				
					_checkColFileName.Active = _settings.visibleColumns.FileName;		//00
					_checkColOsnovanie.Active = _settings.visibleColumns.Osnovanie;		//01
					_checkColOpisanie.Active = _settings.visibleColumns.Opisanie;		//02
					_checkColMka.Active = _settings.visibleColumns.mka;					//03
					_checkColNvr.Active = _settings.visibleColumns.Nvr;					//04
					_checkColSum.Active = _settings.visibleColumns.Sum;					//05
					_checkColTrd.Active = _settings.visibleColumns.Trud;				//06
					_checkColMtr.Active = _settings.visibleColumns.Materiali;			//07
					_checkColMhn.Active = _settings.visibleColumns.Mehanizacia;			//08
					_checkColUsl.Active = _settings.visibleColumns.Uslugi;				//09
					_checkColDop.Active = _settings.visibleColumns.Dopalnitelni;		//10
					_checkColPe.Active = _settings.visibleColumns.Pe4alba;				//11
					_checkColAn.Active = _settings.visibleColumns.Analiz;				//12
					_checkColPePr.Active = _settings.visibleColumns.PePr;				//13
				
					if (_settings.searchMode == 0)
						_radioAnd.Active = true;
					if (_settings.searchMode == 1)
						_radioOr.Active = true;
					if (_settings.searchMode == 2)
						_radioOff.Active = true;
				
					_checkFixedColumn.Active = _settings.fixedColumnLenght;
					_tableBase.fixedSize (_settings.fixedColumnLenght);
				
					_filter = new TreeModelFilter (_data.storeBase, null);
					_filter.VisibleFunc = new TreeModelFilterVisibleFunc (FilterTree);
					_viewSearch.Model = _filter;
				
					_filterTrd = new TreeModelFilter (_data.storeTrd, null);
					_filterTrd.VisibleFunc = new TreeModelFilterVisibleFunc (FilterTreeRes);
				
					_filterMtr = new TreeModelFilter (_data.storeMtr, null);
					_filterMtr.VisibleFunc = new TreeModelFilterVisibleFunc (FilterTreeRes);
				
					_filterMhn = new TreeModelFilter (_data.storeMhn, null);
					_filterMhn.VisibleFunc = new TreeModelFilterVisibleFunc (FilterTreeRes);
				
				
					_viewResources.Model = _filterMtr;

				} else {

					System.Threading.Thread thr1 = new System.Threading.Thread (new ThreadStart (ThreadRoutine1));
					thr1.Start ();

				}



			} else {
				_mainNotebook.Page = (int)tabPage.UserWellcome;
			}




		}

		//Изчистване на лагове
		void ThreadRoutine1 ()	//Зареждане на индекс
		{
			try {
				//_progress.Pulse ();
				
				//_entrySearch.Text = "Зареждане на основните бази данни ...";
				_entrySearch.Sensitive = false;
				_viewSearch.Visible = false;
				
				if ( !_data.readDB () )
					_tableFiles.readFiles (
						_data.storeBase, _settings.BaseDir,
						_data.storeTrd,
						_data.storeMtr,
						_data.storeMhn
						);
				
				
				Gtk.Application.Invoke (delegate {

					if ( _data.storeBase.IterNChildren () > 0 )
					{
						_mainWindow.Title = _info.shortName + " [актуализирани данни от: " + _data.lastUpdate + "]";
						_searchLebal.Text = "<b>Търси</b> в " + _data.rowsMain + " реда:";
						_searchLebal.UseMarkup = true;

					}else{
						_mainWindow.Title = _info.shortName + " [" + _info.progVersion + "]";
					}

					_entrySearch.Sensitive = true;
					_entrySearch.Text = "";
					
					_entrySearch.Changed += OnFilterEntryTextChanged;
					_entrySearch.GrabFocus ();
					//_progress.Visible = false;

					_viewSearch.Visible = true;

					_DirChoiseSettings.SetUri ( _settings.BaseDir );
					
					_checkColFileName.Active	= _settings.visibleColumns.FileName;		//00
					_checkColOsnovanie.Active	= _settings.visibleColumns.Osnovanie;		//01
					_checkColOpisanie.Active	= _settings.visibleColumns.Opisanie;		//02
					_checkColMka.Active			= _settings.visibleColumns.mka;				//03
					_checkColNvr.Active			= _settings.visibleColumns.Nvr;				//04
					_checkColSum.Active			= _settings.visibleColumns.Sum;				//05
					_checkColTrd.Active			= _settings.visibleColumns.Trud;			//06
					_checkColMtr.Active			= _settings.visibleColumns.Materiali;		//07
					_checkColMhn.Active			= _settings.visibleColumns.Mehanizacia;		//08
					_checkColUsl.Active			= _settings.visibleColumns.Uslugi;			//09
					_checkColDop.Active			= _settings.visibleColumns.Dopalnitelni;	//10
					_checkColPe.Active			= _settings.visibleColumns.Pe4alba;			//11
					_checkColAn.Active			= _settings.visibleColumns.Analiz;			//12
					_checkColPePr.Active		= _settings.visibleColumns.PePr;			//13
					
					if ( _settings.searchMode == 0 )	_radioAnd.Active = true;
					if ( _settings.searchMode == 1 )	_radioOr.Active = true;
					if ( _settings.searchMode == 2 )	_radioOff.Active = true;
					
					_checkFixedColumn.Active = _settings.fixedColumnLenght;
					_tableBase.fixedSize ( _settings.fixedColumnLenght );
					
					_filter = new TreeModelFilter ( _data.storeBase, null );
					_filter.VisibleFunc = new TreeModelFilterVisibleFunc ( FilterTree );
					_viewSearch.Model = _filter;
					
					_filterTrd = new TreeModelFilter ( _data.storeTrd, null );
					_filterTrd.VisibleFunc = new TreeModelFilterVisibleFunc ( FilterTreeRes );
					
					_filterMtr = new TreeModelFilter ( _data.storeMtr, null );
					_filterMtr.VisibleFunc = new TreeModelFilterVisibleFunc ( FilterTreeRes );
					
					_filterMhn = new TreeModelFilter ( _data.storeMhn, null );
					_filterMhn.VisibleFunc = new TreeModelFilterVisibleFunc ( FilterTreeRes );
					
					
					_viewResources.Model = _filterMtr;

					if ( _data.storeMtr.IterNChildren () > 0 ){
						_searchLabelRes.Text = "<b>Търси</b> в " + _data.rowsMtr + " реда:";
						_searchLabelRes.UseMarkup = true;
					}

					//Специални параметри
					if ( args.Length > 0 )
					{
						try
						{
							if ( args[0] == "trd" )
							{
								_mainNotebook.Page = (int)tabPage.DataBaseResources;
								_comboResources.Active  = 0;
								
								setSelectedStore ();
							}
							
							if ( args[0] == "mtr" )
							{
								_mainNotebook.Page = (int)tabPage.DataBaseResources;
								_comboResources.Active  = 1;
							}
							
							if ( args[0] == "mhn" )
							{
								_mainNotebook.Page = (int)tabPage.DataBaseResources;
								_comboResources.Active  = 2;
							}
							
							if ( args[0] == "set" )
							{
								_mainNotebook.Page = (int)tabPage.Settings;
							}
						}catch{}
					}

					//Подготовка за второ лагване

					//System.Threading.Thread thr2 = new System.Threading.Thread (new ThreadStart (ThreadRoutine2));	thr2.Start ();
					
				});
			} catch {
			}
		}

		void ThreadRoutine2 ()	//Зареждане на лист
		{
			try {
				
				//Лагване
				
				Gtk.Application.Invoke (delegate {
					
					//Код след лагване
					
				});
			} catch {
			}
		}



		//Системни функции
		private bool FilterTree (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			string searchString =
				(string)model.GetValue (iter, (int)TBase.columns.osnovanie) +
				(string)model.GetValue (iter, (int)TBase.columns.opisanie) +
				(string)model.GetValue (iter, (int)TBase.columns.mka) +
				(string)model.GetValue (iter, (int)TBase.columns.an);
				
			
			if (_entrySearch.Text == "")
				return true;

			if (_settings.searchMode == 0 || _settings.searchMode == 1) {
				return compare ( _entrySearch.Text, searchString);
			}
			
			if ( _settings.searchMode == 2 ) 
				return searchString.ToLower().Contains ( _entrySearch.Text.ToLower() );
			else
				return false;


		}

		private bool compare (string search, string source)
		{
			bool _ret = false; if ( _settings.searchMode == 0 ) _ret = true;

			try {

				if ( search.Contains ( " " ) )
				{
					string[] value = search.Replace ("  ", " ").Split (' '); 

					for (int i = 0; i < value.Length; i++)
					{
						if ( _settings.searchMode == 0 )
							_ret = _ret && source.ToLower().Contains ( value[i].ToLower ());

						if ( _settings.searchMode == 1 )
							_ret = _ret || source.ToLower().Contains ( value[i].ToLower ());
					}
				}else{
					_ret = source.ToLower().Contains ( search.ToLower() );
				}

			} catch {
			}

			return _ret;
		}

		private bool FilterTreeRes (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			bool _searchResult = false;

			string searchString =
				(string)model.GetValue (iter, (int)TResources.columns.osnovanie) +
				(string)model.GetValue (iter, (int)TResources.columns.ime) +
				(string)model.GetValue (iter, (int)TResources.columns.mka) +
				(string)model.GetValue (iter, (int)TResources.columns.dsr) +
				(string)model.GetValue (iter, (int)TResources.columns.cena);
			
			
			if (_entrySearchRes.Text == "")
				return true;
			
			if (_settings.searchMode == 0 || _settings.searchMode == 1) {
				return  compare ( _entrySearchRes.Text, searchString);
			}
			
			if ( _settings.searchMode == 2 ) 
				return searchString.ToLower().Contains ( _entrySearchRes.Text.ToLower() );


			return _searchResult;
		}


		//00. First Time Run
		private void OnBtnFirstDirOpen (object sender, EventArgs e)
		{
			try {
				_settings.runBaseDir ();
			} catch {
			}
		}

		private void OnFirstMarkAll (object sender, EventArgs e)
		{
			try {
				_tableFilesFirst.markAll ();
			} catch {
			}
		}

		private void OnBtnFirstSetSamples (object sender, EventArgs e)
		{
			try {
				if ( ! _samples.writeSampleFile (_settings) )
				{
					//Съобщение, че файловете не могат да бъдат записани

					//Предложени да се запишат на декстопа
				}else{
					_tableFilesFirst.loadFiles ( _settings.BaseDir );
					_tableFilesFirst.markAll ();

					bool isTableNotEmpty = _storeFilesFirst.IterNChildren () > 0;
					
					_btnFristMarkAll.Sensitive = isTableNotEmpty;
					_btnGoToMainScreen.Sensitive = isTableNotEmpty;
					_btnFristTableRefresh.Sensitive = isTableNotEmpty;
				}
			} catch {
			}
		}

		private void OnBtnGoToMainScreen (object sender, EventArgs e)
		{
			try {
				_settings.saveSettings ();
				_mainNotebook.Page = (int) tabPage.DataBase;


				if ( _tableFilesFirst.readFiles (
					_data.storeBase, _settings.BaseDir,
					_data.storeTrd,
					_data.storeMtr,
					_data.storeMhn
					)	)
				{
					_settings.CheckedFiles = _tableFilesFirst.getSelectedFiles ();
					_tableFiles.setSelectedFiles ( _settings.CheckedFiles );

					_data.lastUpdate =
						DateTime.Now.Year.ToString () + "-" + 
							forN ( DateTime.Now.Month.ToString () ) + "-" +
							forN ( DateTime.Now.Day.ToString () );
					
					_data.rowsMain	= _data.storeBase.IterNChildren	().ToString ();
					_data.rowsTrd	= _data.storeTrd.IterNChildren	().ToString ();
					_data.rowsMtr	= _data.storeMtr.IterNChildren	().ToString ();
					_data.rowsMhn	= _data.storeMhn.IterNChildren	().ToString ();
					
					_mainWindow.Title = _info.shortName + " [актуализирани данни от: " + _data.lastUpdate + "]";
					
					_searchLebal.Text = "<b>Търси</b> в " + _data.rowsMain + " реда:";
					_searchLebal.UseMarkup = true;
					


					_data.writeDB ();
				}
				
				_entrySearch.Sensitive = true;
				_entrySearch.Text = "";
				
				_entrySearch.Changed += OnFilterEntryTextChanged;
				_entrySearch.GrabFocus ();
				
				_viewSearch.Visible = true;
				
				_DirChoiseSettings.SetUri (_settings.BaseDir);
				
				_checkColFileName.Active = _settings.visibleColumns.FileName;		//00
				_checkColOsnovanie.Active = _settings.visibleColumns.Osnovanie;		//01
				_checkColOpisanie.Active = _settings.visibleColumns.Opisanie;		//02
				_checkColMka.Active = _settings.visibleColumns.mka;					//03
				_checkColNvr.Active = _settings.visibleColumns.Nvr;					//04
				_checkColSum.Active = _settings.visibleColumns.Sum;					//05
				_checkColTrd.Active = _settings.visibleColumns.Trud;				//06
				_checkColMtr.Active = _settings.visibleColumns.Materiali;			//07
				_checkColMhn.Active = _settings.visibleColumns.Mehanizacia;			//08
				_checkColUsl.Active = _settings.visibleColumns.Uslugi;				//09
				_checkColDop.Active = _settings.visibleColumns.Dopalnitelni;		//10
				_checkColPe.Active = _settings.visibleColumns.Pe4alba;				//11
				_checkColAn.Active = _settings.visibleColumns.Analiz;				//12
				_checkColPePr.Active = _settings.visibleColumns.PePr;				//13
				
				if (_settings.searchMode == 0)
					_radioAnd.Active = true;
				if (_settings.searchMode == 1)
					_radioOr.Active = true;
				if (_settings.searchMode == 2)
					_radioOff.Active = true;
				
				_checkFixedColumn.Active = _settings.fixedColumnLenght;
				_tableBase.fixedSize (_settings.fixedColumnLenght);
				
				_filter = new TreeModelFilter (_data.storeBase, null);
				_filter.VisibleFunc = new TreeModelFilterVisibleFunc (FilterTree);
				_viewSearch.Model = _filter;
				
				_filterTrd = new TreeModelFilter (_data.storeTrd, null);
				_filterTrd.VisibleFunc = new TreeModelFilterVisibleFunc (FilterTreeRes);
				
				_filterMtr = new TreeModelFilter (_data.storeMtr, null);
				_filterMtr.VisibleFunc = new TreeModelFilterVisibleFunc (FilterTreeRes);
				
				_filterMhn = new TreeModelFilter (_data.storeMhn, null);
				_filterMhn.VisibleFunc = new TreeModelFilterVisibleFunc (FilterTreeRes);
				
				
				_viewResources.Model = _filterMtr;
			} catch {
			}
		}

		private void OnBtnFristTableRefreshClick (object sender, EventArgs e)
		{
			try {
				_tableFilesFirst.loadFiles (_settings.BaseDir);
				_tableFilesFirst.markAll ();
			} catch {
			}
		}

		protected void OnFistDirChoiseSelectionChanged (object sender, EventArgs e)
		{
			try {
				_settings.BaseDir = _firstDirChoise.Filename;
				Console.WriteLine (_firstDirChoise.Filename);

				if (_tableFilesFirst == null) {
					_viewFirstScreen.Sensitive = true;
					_btnFirstSetSamples.Sensitive = true;

					_tableFilesFirst = new TFiles (_viewFirstScreen, _storeFilesFirst);
				}

				_tableFilesFirst.loadFiles (_settings.BaseDir);
				_tableFilesFirst.markAll ();

				bool isTableNotEmpty = _storeFilesFirst.IterNChildren () > 0;

				_btnFristMarkAll.Sensitive = isTableNotEmpty;
				_btnGoToMainScreen.Sensitive = isTableNotEmpty;
				_btnFristTableRefresh.Sensitive = isTableNotEmpty;


				_btnFristDirOpen.Sensitive = true;
			} catch {
			}
		}


		//01. Търсене
		private void OnBtnUpdateClicked (object sender, EventArgs e)
		{
			_mainNotebook.Page = (int) tabPage.Update;
			_tableFiles.loadFiles ( _settings.BaseDir );
			_tableFiles.setSelectedFiles ( _settings.CheckedFiles );
		}

		private void OnFilterEntryTextChanged (object o, System.EventArgs args)
		{
			_filter.Refilter ();
		}

		private void OnBtnSettingsClicked (object sender, EventArgs e)
		{
			_mainNotebook.Page = (int) tabPage.Settings;
		}

		private void OnBtnCopyRow (object sender, EventArgs e)
		{
			try {
				TreeIter _sel;
				
				if ( _viewSearch.Selection.GetSelected ( out _sel ) )
				{
					Clipboard clippy = Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false));	clippy.Clear ();
					clippy.Text = "[row]"
						+ (string) _viewSearch.Model.GetValue ( _sel, (int) TBase.columns.opisanie )
						+ "#" + (string) _viewSearch.Model.GetValue ( _sel, (int) TBase.columns.mka )
						+ "#1"
						+ "#" + (string) _viewSearch.Model.GetValue ( _sel, (int) TBase.columns.an );
				}
			} catch {
			};
		}

		private void OnBtnCopyAn (object sender, EventArgs e)
		{
			try {
				TreeIter _sel;

				if ( _viewSearch.Selection.GetSelected ( out _sel ) )
				{
					Clipboard clippy = Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false));	clippy.Clear ();
					clippy.Text = "[anpe]" + (string) _viewSearch.Model.GetValue ( _sel, (int) TBase.columns.an );
				}
			} catch {
			}
		}

		private void OnBtnGoToRes (object sender, EventArgs e)
		{
			try {
				_mainNotebook.Page = (int) tabPage.DataBaseResources;
			} catch {
			}
		}

		protected void OnViewSearchRowActivated (object o, RowActivatedArgs args)
		{
			try {
				TreeIter _row;

				if ( _viewSearch.Selection.GetSelected ( out _row ) )
				{
					anDialog _an = new anDialog (
							(string) _viewSearch.Model.GetValue ( _row, (int) TBase.columns.an ),
							(string) _viewSearch.Model.GetValue ( _row, (int) TBase.columns.pepr),
							(string) _viewSearch.Model.GetValue ( _row, (int) TBase.columns.osnovanie ),
							(string) _viewSearch.Model.GetValue ( _row, (int) TBase.columns.opisanie ),
							(string) _viewSearch.Model.GetValue ( _row, (int) TBase.columns.mka ),
							(string) _viewSearch.Model.GetValue ( _row, (int) TBase.columns.filename )
						);

					_an.Run ();
					_an.Destroy ();
				}

				 
			} catch {
			}
		}


		//02. Ресурси
		private void setupComboBox ()
		{
			ListStore _tempStore = new ListStore ( typeof (string) );
			_tempStore.AppendValues ( "Труд" );
			_tempStore.AppendValues ( "Материали" );
			_tempStore.AppendValues ( "Механизация" );


			_comboResources.Model = _tempStore;
			_comboResources.Active = 1;

			setSelectedStore ();
		}

		private void OnFilterEntryResTextChanged (object o, System.EventArgs args)
		{
			if ( _comboResources.Active == 0 )	_filterTrd.Refilter ();
			if ( _comboResources.Active == 1 )	_filterMtr.Refilter ();
			if ( _comboResources.Active == 2 )	_filterMhn.Refilter ();
		}

		private void setSelectedStore ()
		{
			try {
				_filterTrd = new TreeModelFilter ( _data.storeTrd, null );
				_filterTrd.VisibleFunc = new TreeModelFilterVisibleFunc ( FilterTreeRes );

				_filterMtr = new TreeModelFilter ( _data.storeMtr, null );
				_filterMtr.VisibleFunc = new TreeModelFilterVisibleFunc ( FilterTreeRes );

				_filterMhn = new TreeModelFilter ( _data.storeMhn, null );
				_filterMhn.VisibleFunc = new TreeModelFilterVisibleFunc ( FilterTreeRes );

				if ( _comboResources.Active == 0 )
				{
					if ( _data.storeTrd.IterNChildren () > 0 )
					{
						_searchLabelRes.Text = "<b>Търси</b> в " + _data.rowsTrd + " реда:";
						_searchLabelRes.UseMarkup = true;
					}
					_viewResources.Model = _filterTrd;

				}

				if ( _comboResources.Active == 1 )
				{
					if ( _data.storeMtr.IterNChildren () > 0 )
					{
						_searchLabelRes.Text = "<b>Търси</b> в " + _data.rowsMtr + " реда:";
						_searchLabelRes.UseMarkup = true;
					}
					_viewResources.Model = _filterMtr;
				}

				if ( _comboResources.Active == 2 )
				{
					if ( _data.storeMhn.IterNChildren () > 0 )
					{
						_searchLabelRes.Text = "<b>Търси</b> в " + _data.rowsMhn + " реда:";
						_searchLabelRes.UseMarkup = true;
					}
					_viewResources.Model = _filterMhn;
				}


			} catch {
			}

		}

		protected void OnComboboxSearchInDBChanged (object sender, EventArgs e)
		{
			try {

				setSelectedStore ();

			} catch {
			}
		}


		private void OnBtnCopyRes (object sender, EventArgs e)
		{
			try {
				TreeIter _it;

				if ( _viewResources.Selection.GetSelected ( out _it ) )
				{
					string _type = "trd";

					_type = ( _comboResources.Active == 0 ) ? "trd" : _type;
					_type = ( _comboResources.Active == 1 ) ? "mtr" : _type;
					_type = ( _comboResources.Active == 2 ) ? "mhn" : _type;
			
					Clipboard clippy = Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false));	clippy.Clear ();
					clippy.Text = "[an:row]"
							  + (string) _viewResources.Model.GetValue ( _it, (int) TResources.columns.osnovanie )
						+ "#" + (string) _viewResources.Model.GetValue ( _it, (int) TResources.columns.ime )
						+ "#" + (string) _viewResources.Model.GetValue ( _it, (int) TResources.columns.mka )
						+ "#" + String.Format ( "{0:0.0000}", 0 )
						+ "#" + (string) _viewResources.Model.GetValue ( _it, (int) TResources.columns.dsr )
						+ "#" + (string) _viewResources.Model.GetValue ( _it, (int) TResources.columns.cena )
						+ "#" + String.Format ( "{0:0.00}", 0 ) + "#" + _type ;
					

				}

			} catch {
			}
		}

		private void OnBtnBackRes (object sender, EventArgs e)
		{
			_mainNotebook.Page = (int) tabPage.DataBase;
		}

		//03. Актуализация
		private void OnBtnFFBackClicked (object sender, EventArgs e)
		{
			_mainNotebook.Page = (int) tabPage.DataBase;
			_settings.CheckedFiles = _tableFiles.getSelectedFiles ();
			_settings.saveSettings ();

			_tableFiles.readFiles ( _data.storeBase, _settings.BaseDir, _data.storeTrd, _data.storeMtr, _data.storeMhn );

			_data.lastUpdate =
				DateTime.Now.Year.ToString () + "-" + 
				forN ( DateTime.Now.Month.ToString () ) + "-" +
				forN ( DateTime.Now.Day.ToString () );

			_data.rowsMain	= _data.storeBase.IterNChildren	().ToString ();
			_data.rowsTrd	= _data.storeTrd.IterNChildren	().ToString ();
			_data.rowsMtr	= _data.storeMtr.IterNChildren	().ToString ();
			_data.rowsMhn	= _data.storeMhn.IterNChildren	().ToString ();

			_mainWindow.Title = _info.shortName + " [актуализирани данни от: " + _data.lastUpdate + "]";

			_searchLebal.Text = "<b>Търси</b> в " + _data.rowsMain + " реда:";
			_searchLebal.UseMarkup = true;

			_data.writeDB ();

		}

		private string forN (string _inp)
		{
			try {
				int a = Convert.ToInt32 ( _inp );
				if ( a < 10 )	return "0" + _inp;
			} catch {
			}

			return _inp;
		}

		private void OnBtnOpenFolderClicked (object sender, EventArgs e)
		{
			_settings.runBaseDir ();
			//_tableFiles.readFiles ( _storeBase , _settings.BaseDir );
		}

		private void OnBtnUpdateFilesClicked (object sender, EventArgs e)
		{
			_tableFiles.loadFiles ( _settings.BaseDir );
			//_tableFiles.readFiles ( _storeBase , _settings.BaseDir );
		}

		private void OnBtnFFMarkClicked (object sender, EventArgs e)
		{
			_tableFiles.markAll ();
		}

		//04. Настройки
		private void OnBtnBackSettingsClicked (object sender, EventArgs e)
		{
			_settings.visibleColumns.FileName		= _checkColFileName.Active;			//00
			_settings.visibleColumns.Osnovanie		= _checkColOsnovanie.Active;		//01
			_settings.visibleColumns.Opisanie		= _checkColOpisanie.Active;			//02
			_settings.visibleColumns.mka			= _checkColMka.Active;				//03
			_settings.visibleColumns.Nvr			= _checkColNvr.Active;				//04
			_settings.visibleColumns.Sum			= _checkColSum.Active;				//05
			_settings.visibleColumns.Trud			= _checkColTrd.Active;				//06
			_settings.visibleColumns.Materiali		= _checkColMtr.Active;				//07
			_settings.visibleColumns.Mehanizacia	= _checkColMhn.Active;				//08
			_settings.visibleColumns.Uslugi			= _checkColUsl.Active;				//09
			_settings.visibleColumns.Dopalnitelni	= _checkColDop.Active;				//10
			_settings.visibleColumns.Pe4alba		= _checkColPe.Active;				//11
			_settings.visibleColumns.Analiz			= _checkColAn.Active;				//12
			_settings.visibleColumns.PePr			= _checkColPePr.Active;				//13

			if ( _radioAnd.Active )		_settings.searchMode = (int) FSSettings.SearchMode.modeAnd;
			if ( _radioOr.Active )		_settings.searchMode = (int) FSSettings.SearchMode.modeOr;
			if ( _radioOff.Active )		_settings.searchMode = (int) FSSettings.SearchMode.modeOff;

			_settings.fixedColumnLenght = _checkFixedColumn.Active;
			_tableBase.fixedSize ( _settings.fixedColumnLenght );


			_settings.saveSettings ();
			
			_mainNotebook.Page = (int) tabPage.DataBase;
		}

		private void OnBtnRunAppDataSettingsClicked (object sender, EventArgs e)
		{
			_settings.runAppData ();
		}

		private void OnBtnRunBaseDirSettingsClicked (object sender, EventArgs e)
		{
			_settings.runBaseDir ();
		}

		protected void OnDirChoiseSettingsSelectionChanged (object sender, EventArgs e)
		{
			_settings.BaseDir = _DirChoiseSettings.Filename;
		}

		protected void OnCheckColToggled (object sender, EventArgs e)
		{
			_tableBase.colFileName.Visible		= _checkColFileName.Active;			//00
			_tableBase.colOsnovanie.Visible		= _checkColOsnovanie.Active;		//01
			_tableBase.colOpisanie.Visible		= _checkColOpisanie.Active;			//02
			_tableBase.colMka.Visible			= _checkColMka.Active;				//03
			_tableBase.colNvr.Visible			= _checkColNvr.Active;				//04
			_tableBase.colSum.Visible			= _checkColSum.Active;				//05
			_tableBase.colTrd.Visible			= _checkColTrd.Active;				//06
			_tableBase.colMtr.Visible			= _checkColMtr.Active;				//07
			_tableBase.colMhn.Visible			= _checkColMhn.Active;				//08
			_tableBase.colUsl.Visible			= _checkColUsl.Active;				//09
			_tableBase.colDop.Visible			= _checkColDop.Active;				//10
			_tableBase.colPe.Visible			= _checkColPe.Active;				//11
			_tableBase.colAn.Visible			= _checkColAn.Active;				//12
			_tableBase.colPePr.Visible			= _checkColPePr.Active;				//13
		}

	}
}

