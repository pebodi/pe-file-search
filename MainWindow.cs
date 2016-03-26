using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	public FileSearch.Controller _controller;

	public MainWindow (string[] args): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		_controller = new FileSearch.Controller (
			this,			//Основен прозорец
			args,
			notebook1,		//Основно разпределение на екраните

			//00. First Time Run
			FirstDirChoise,
			btnFirstDirOpen,
			btnFristMarkAll,
			btnSetSamples,
			btnGoToMainScreen,
			btnFristTableRefresh,
			viewFirstFiles,

			//01. Търсене
			label6,
			entrySearch,
			viewSearch,
			btnSearch,
			btnUpdate,
			btnSettings,
			btnCopyRow,
			btnCopyAn,
			btnGoToRes,

			//02. Ресурси
			btnBackInSearch,
			label16,
			entrySearchResources,
			comboboxSearchInDB,
			viewResources,
			btnCopyResRow,

			//03. Актуализация
			viewFiles,
			btnFFBack,
			btnFFContainFolder,
			btnFFUpdate,
			btnFFMark,

			//04. Настройки
			btnBackSettings,
			DirChoiseSettings,
			btnRunBaseDirSettings,
			btnRunAppDataSettings,
			radioSMAnd,
			radioSMOr,
			radioSMOff,
			checkbuttonFixedColumn,
			checkColFile,
			checkColOsnovanie,
			checkColOpisanie,
			checkColMka,
			checkColNvr,
			checkColSum,
			checkColTrd,
			checkColMtr,
			checkColMhn,
			checkColUsl,
			checkColDop,
			checkColPe4,
			checkColAn,
			checkColPePr
		);

	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}




}