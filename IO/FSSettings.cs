using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace FileSearch
{
	public class FSSettings
	{
		public enum SearchMode { modeAnd = 0, modeOr, modeOff }

		//Дефиниране на пропъртита
		public string Path { 
			get {
				return System.IO.Path.Combine ( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ), "PEFileSearch" );
			}
		}

		public string PathSettings {
			get {
				return System.IO.Path.Combine ( Path, "settings.xml" );
			}
		}

		public string PathData {
			get {
				return System.IO.Path.Combine ( Path, "data.xml" );
			}
		}
		
		public string		BaseDir				{ get; set; }
		public string		CheckedFiles		{ get; set; }	//Сепаратор ","

		public visibleCols	visibleColumns = 	new visibleCols ();

		public bool			fixedColumnLenght	{ get; set; }
		public int			searchMode			{ get; set; }		

		//Инициализация
		public FSSettings ()
		{
			if ( !loadSettings () )	restoreSettings ();
		}

		//Писане и четене
		public bool loadSettings ()
		{
			try {
				XmlReaderSettings _settings = new XmlReaderSettings ();
				
				using (XmlReader reader = XmlReader.Create ( PathSettings ) )
				{
					while ( reader.Read () )
					{
						
						if ( reader.IsStartElement () )
						{
							
							switch ( reader.Name )
							{
							case "БД":
								BaseDir			= reader["Адрес"];
								
								break;
								
							case "Избрани":
								CheckedFiles	= reader["Файлове"];
								
								break;
							
							//Проюитане на колони
							case "Търсене":			//00
								searchMode = Convert.ToInt16 ( reader["режим"] );
								break;

							//Проюитане на колони
							case "дължина":			//00
								fixedColumnLenght = Convert.ToBoolean ( reader["фиксирана"] );;
								break;

							//Проюитане на колони
							case "Файл":			//00
								visibleColumns.FileName = Convert.ToBoolean ( reader["включена"] );
								break;

							case "Основание":		//01
								visibleColumns.Osnovanie = Convert.ToBoolean ( reader["включена"] );
								break;

							case "Описание":		//02
								visibleColumns.Opisanie = Convert.ToBoolean ( reader["включена"] );
								break;

							case "м-ка":			//03
								visibleColumns.mka = Convert.ToBoolean ( reader["включена"] );
								break;

							case "Нвр":				//04
								visibleColumns.Nvr = Convert.ToBoolean ( reader["включена"] );
								break;

							case "Общо":			//05
								visibleColumns.Sum = Convert.ToBoolean ( reader["включена"] );
								break;

							case "Труд":			//06
								visibleColumns.Trud = Convert.ToBoolean ( reader["включена"] );
								break;

							case "Материали":		//07
								visibleColumns.Materiali = Convert.ToBoolean ( reader["включена"] );
								break;

							case "Механизация":		//08
								visibleColumns.Mehanizacia = Convert.ToBoolean ( reader["включена"] );
								break;

							case "Услуги":			//09
								visibleColumns.Uslugi = Convert.ToBoolean ( reader["включена"] );
								break;

							case "Допълнителни":	//10
								visibleColumns.Dopalnitelni = Convert.ToBoolean ( reader["включена"] );
								break;

							case "Печалба":			//11
								visibleColumns.Pe4alba = Convert.ToBoolean ( reader["включена"] );
								break;

							case "Анализ":			//12
								visibleColumns.Analiz = Convert.ToBoolean ( reader["включена"] );
								break;

							case "ПечалбаПр":		//13
								visibleColumns.PePr = Convert.ToBoolean ( reader["включена"] );
								break;
							}
						
						}
					}
					
				}

				return true;
			} catch {
			}

			return false;
		}

		public void saveSettings ()
		{
			try {
				using (XmlWriter writer = XmlWriter.Create ( PathSettings )) {
					writer.WriteStartDocument ();
					writer.WriteStartElement ("Настройки");


					writer.WriteStartElement ("БД");
					writer.WriteAttributeString ( "Адрес", BaseDir );
					writer.WriteEndElement ();
					
					writer.WriteStartElement ("Избрани");
					writer.WriteAttributeString ("Файлове", CheckedFiles);
					writer.WriteEndElement ();

					writer.WriteStartElement ("Търсене");
					writer.WriteAttributeString ( "режим", searchMode.ToString () );
					writer.WriteEndElement ();


					writer.WriteStartElement ("Колони");
					
					writeColumnSettings ( writer, "Файл",			visibleColumns.FileName		);	//00
					writeColumnSettings ( writer, "Основание",		visibleColumns.Osnovanie	);	//01
					writeColumnSettings ( writer, "Описание",		visibleColumns.Opisanie		);	//02
					writeColumnSettings ( writer, "м-ка",			visibleColumns.mka			);	//03
					writeColumnSettings ( writer, "Нвр",			visibleColumns.Nvr			);	//04
					writeColumnSettings ( writer, "Общо",			visibleColumns.Sum			);	//05
					writeColumnSettings ( writer, "Труд",			visibleColumns.Trud			);	//06
					writeColumnSettings ( writer, "Материали",		visibleColumns.Materiali	);	//07
					writeColumnSettings ( writer, "Механизация",	visibleColumns.Mehanizacia	);	//08
					writeColumnSettings ( writer, "Услуги",			visibleColumns.Uslugi		);	//09
					writeColumnSettings ( writer, "Допълнителни",	visibleColumns.Dopalnitelni	);	//10
					writeColumnSettings ( writer, "Печалба",		visibleColumns.Pe4alba		);	//11
					writeColumnSettings ( writer, "Анализ",			visibleColumns.Analiz		);	//12
					writeColumnSettings ( writer, "ПечалбаПр",		visibleColumns.PePr			);	//13


					writer.WriteStartElement ( "дължина" );
					writer.WriteAttributeString ( "фиксирана", fixedColumnLenght.ToString () );
					writer.WriteEndElement ();

					writer.WriteEndElement ();
					
					writer.WriteEndElement ();
					writer.WriteEndDocument ();
					
				}
				
				XDocument document = XDocument.Load (PathSettings);
				document.Save (PathSettings);
			} catch {
			}
		}

		public void writeColumnSettings (XmlWriter writer, string column, bool visible)
		{
			writer.WriteStartElement ( column );
			writer.WriteAttributeString ( "включена", visible.ToString () );
			writer.WriteEndElement ();
		}

		public void restoreSettings ()
		{
			try {
				//Проверка и създаване на директория
				if ( ! Directory.Exists ( Path ) )
				{
					Directory.CreateDirectory ( Path );
				}

				searchMode = (int) SearchMode.modeAnd;
				fixedColumnLenght = true;
			
				//Проверка и създаване на xml файл
				if ( ! File.Exists ( PathSettings ) )
				{
					saveSettings ();
				}


			} catch {
			}
		}

		//Помощни методи
		public void runAppData ()
		{
			try {
				Process.Start (Path);
			} catch {
				Console.WriteLine ( "Грешка при стартиране на: " + Path );
			}
		}

		public void runBaseDir ()
		{
			try {
				Process.Start (BaseDir);
			} catch {
				Console.WriteLine ( "Грешка при стартиране на: " + BaseDir );
			}
		}
	}

	public class visibleCols
	{
		public bool FileName		{ get; set; }	//00
		public bool Osnovanie		{ get; set; }	//01
		public bool Opisanie		{ get; set; }	//02
		public bool mka				{ get; set; }	//03
		public bool Nvr				{ get; set; }	//04
		public bool Sum				{ get; set; }	//05
		public bool Trud			{ get; set; }	//06
		public bool Materiali		{ get; set; }	//07
		public bool Mehanizacia		{ get; set; }	//08
		public bool Uslugi			{ get; set; }	//09
		public bool Dopalnitelni	{ get; set; }	//10
		public bool Pe4alba			{ get; set; }	//11
		public bool Analiz			{ get; set; }	//12
		public bool PePr			{ get; set; }	//13

		public visibleCols ()
		{
			FileName		= false;	//00
			Osnovanie		= false;	//01
			Opisanie		= true;		//02
			mka				= true;		//03
			Nvr				= true;		//04
			Sum				= true;		//05
			Trud			= false;	//06
			Materiali		= false;	//07
			Mehanizacia		= false;	//08
			Uslugi			= false;	//09
			Dopalnitelni	= false;	//10
			Pe4alba			= false;	//11
			Analiz			= false;	//12
			PePr			= false;	//13
		}
	}
}

