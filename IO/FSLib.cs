using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using Gtk;


namespace FileSearch
{
	public class FSLib
	{
		//Библиотеки
		private FSSettings _settings = new FSSettings ();

		//Данни

		public string rowsMain		{ get; set; }
		public string rowsTrd		{ get; set; }
		public string rowsMtr		{ get; set; }
		public string rowsMhn		{ get; set; }

		public string lastUpdate	{ get; set; }

		public ListStore	storeFiles = new ListStore (
			typeof (string),
			typeof (bool)
			);
		
		public ListStore	storeBase	= new ListStore (
			typeof (string),	//00
			typeof (string),	//01
			typeof (string),	//02
			typeof (string),	//03
			typeof (string),	//04
			typeof (string),	//05
			typeof (string),	//06
			typeof (string),	//07
			typeof (string),	//08
			typeof (string),	//09
			typeof (string),	//10
			typeof (string),	//11
			typeof (string),	//12
			typeof (string)		//13
			);
		
		public ListStore	storeTrd	= new ListStore (
			typeof (string),	//00
			typeof (string),	//01
			typeof (string),	//02
			typeof (string),	//03
			typeof (string),	//04
			typeof (string)		//05
			);
		
		public ListStore	storeMtr	= new ListStore (
			typeof (string),	//00
			typeof (string),	//01
			typeof (string),	//02
			typeof (string),	//03
			typeof (string),	//04
			typeof (string)		//05
			);
		
		public ListStore	storeMhn	= new ListStore (
			typeof (string),	//00
			typeof (string),	//01
			typeof (string),	//02
			typeof (string),	//03
			typeof (string),	//04
			typeof (string)		//05
			);

		//Други
		private string[] colName = new string[] {
			"filename",		//00
			"osnovanie",	//01
			"opisanie",		//02
			"mka",			//03
			"nvr",			//04
			"sum",			//05
			"trd",			//06
			"mtr",			//07
			"mhn",			//08
			"usl",			//09
			"dop",			//10
			"pe",			//11
			"an",			//12
			"pepr"			//13
		};

		private string[] colNameRes = new string[] {
			"filename",		//00
			"osnovanie",	//01
			"ime",			//02
			"mka",			//03
			"dsr",			//04
			"cena"			//05
		};

		public FSLib ()
		{
		}

		//01. Запис на базата данни
		public bool writeDB ()
		{
			try {

				using (XmlWriter writer = XmlWriter.Create ( _settings.PathData )) {

					writer.WriteStartDocument ();
					writer.WriteStartElement ("ОсновнаБаза");

					//01. Запис на допълнителни данни
					writer.WriteStartElement ("Актуализации");
					writer.WriteAttributeString ( "последна" , lastUpdate );
                    writer.WriteEndElement ();
                    
                    writer.WriteStartElement ("Елементи");
					writer.WriteAttributeString ( "основни",		rowsMain );
					writer.WriteAttributeString ( "труд",		rowsTrd );
					writer.WriteAttributeString ( "материали",	rowsMtr );
					writer.WriteAttributeString ( "механизация", rowsMhn );
                    writer.WriteEndElement ();

					//02. Запис на основните данни
					writer.WriteStartElement ("Данни");

					TreeIter _row;

					for (int i = 0; i < storeBase.IterNChildren (); i++ )
					{
						storeBase.IterNthChild ( out _row, i );

						writer.WriteStartElement ( "ред" );
						writer.WriteAttributeString ( colName[00],	(string) storeBase.GetValue ( _row, 00 ) );		//00
						writer.WriteAttributeString ( colName[01],	(string) storeBase.GetValue ( _row, 01 ) );		//01
						writer.WriteAttributeString ( colName[02],	(string) storeBase.GetValue ( _row, 02 ) );		//02
						writer.WriteAttributeString ( colName[03],	(string) storeBase.GetValue ( _row, 03 ) );		//03
						writer.WriteAttributeString ( colName[04],	(string) storeBase.GetValue ( _row, 04 ) );		//04
						writer.WriteAttributeString ( colName[05],	(string) storeBase.GetValue ( _row, 05 ) );		//05
						writer.WriteAttributeString ( colName[06],	(string) storeBase.GetValue ( _row, 06 ) );		//06
						writer.WriteAttributeString ( colName[07],	(string) storeBase.GetValue ( _row, 07 ) );		//07
						writer.WriteAttributeString ( colName[08],	(string) storeBase.GetValue ( _row, 08 ) );		//08
						writer.WriteAttributeString ( colName[09],	(string) storeBase.GetValue ( _row, 09 ) );		//09
						writer.WriteAttributeString ( colName[10],	(string) storeBase.GetValue ( _row, 10 ) );		//10
						writer.WriteAttributeString ( colName[11],	(string) storeBase.GetValue ( _row, 11 ) );		//11
						writer.WriteAttributeString ( colName[12],	(string) storeBase.GetValue ( _row, 12 ) );		//12
						writer.WriteAttributeString ( colName[13],	(string) storeBase.GetValue ( _row, 13 ) );		//13
						writer.WriteEndElement ();
					}

					writer.WriteEndElement ();

					//03. Запис на труд
					writeResPart ( writer,	storeTrd,	"работник",		"Труд" );

					//04. Запис на материали
					writeResPart ( writer,	storeMtr,	"материал",		"Материали" );

					//05. Запис на механизация
					writeResPart ( writer,	storeMhn,	"механизация",	"Механизация" );



					writer.WriteEndElement ();
					writer.WriteEndDocument ();
				}

				XDocument document = XDocument.Load ( _settings.PathData );
				document.Save ( _settings.PathData );

				return true;
			} catch {
			}

			return false;
		}

		private bool writeResPart (XmlWriter writer, ListStore _store, string _type, string _part)
		{
			try {
				writer.WriteStartElement (_part);
				
				TreeIter _row;
				
				for (int i = 0; i < _store.IterNChildren (); i++ )
				{
					_store.IterNthChild ( out _row, i );
					
					writer.WriteStartElement ( _type );
					writer.WriteAttributeString ( colNameRes[00],	(string) _store.GetValue ( _row, 00 ) );		//00
					writer.WriteAttributeString ( colNameRes[01],	(string) _store.GetValue ( _row, 01 ) );		//01
					writer.WriteAttributeString ( colNameRes[02],	(string) _store.GetValue ( _row, 02 ) );		//02
					writer.WriteAttributeString ( colNameRes[03],	(string) _store.GetValue ( _row, 03 ) );		//03
					writer.WriteAttributeString ( colNameRes[04],	(string) _store.GetValue ( _row, 04 ) );		//04
					writer.WriteAttributeString ( colNameRes[05],	(string) _store.GetValue ( _row, 05 ) );		//05
					writer.WriteEndElement ();
				}
				
				writer.WriteEndElement ();

				return true;
			} catch {
			}

			return false;
		}

		//02. Четене на базата данни
		public bool readDB ( )
		{
			try {
				//Изход при липса на данни
				if ( !System.IO.File.Exists ( _settings.PathData ) )	return false;

				//Буферни масиви
				string[] value		= new string[] { "", "", "", "", "",	"", "", "", "", "",		"", "", "", "" };
				string[] valueRes	= new string[] { "", "", "", "", "",	"" };


				/*
//01. Запис на допълнителни данни
					writer.WriteStartElement ("Актуализации");
					writer.WriteAttributeString ( "последна" , lastUpdate );
                    writer.WriteEndElement ();
                    
                    writer.WriteStartElement ("Елементи");
					writer.WriteAttributeString ( "основни",		rowsMain );
					writer.WriteAttributeString ( "труд",		rowsTrd );
					writer.WriteAttributeString ( "материали",	rowsMtr );
					writer.WriteAttributeString ( "механизация", rowsMhn );
                    writer.WriteEndElement ();
				*/

				//Почистванена на хранилища
				storeBase.Clear ();
				storeTrd.Clear ();	storeMtr.Clear ();	storeMhn.Clear ();

				using (XmlReader reader = XmlReader.Create ( _settings.PathData ) )
				{
					while ( reader.Read () )
					{
						if ( reader.IsStartElement () )
						{
							switch ( reader.Name )
							{
							case "Актуализации":
								lastUpdate = Convert.ToString ( reader[ "последна" ] );
								break;
							
							case "Елементи":
								rowsMain	= Convert.ToString ( reader[ "основни"		]);
								rowsTrd		= Convert.ToString ( reader[ "труд"			]);
								rowsMtr		= Convert.ToString ( reader[ "материали"	]);
								rowsMhn		= Convert.ToString ( reader[ "механизация"	]);
								break;



								//Разчитане
							case "ред":
								value[00] = Convert.ToString ( reader[ colName[00] ] );
								value[01] = Convert.ToString ( reader[ colName[01] ] );
								value[02] = Convert.ToString ( reader[ colName[02] ] );
								value[03] = Convert.ToString ( reader[ colName[03] ] );
								value[04] = Convert.ToString ( reader[ colName[04] ] );
								value[05] = Convert.ToString ( reader[ colName[05] ] );
								value[06] = Convert.ToString ( reader[ colName[06] ] );
								value[07] = Convert.ToString ( reader[ colName[07] ] );
								value[08] = Convert.ToString ( reader[ colName[08] ] );
								value[09] = Convert.ToString ( reader[ colName[09] ] );
								value[10] = Convert.ToString ( reader[ colName[10] ] );
								value[11] = Convert.ToString ( reader[ colName[11] ] );
								value[12] = Convert.ToString ( reader[ colName[12] ] );
								value[13] = Convert.ToString ( reader[ colName[13] ] );

								storeBase.AppendValues (
									value [00],
									value [01],
									value [02],
									value [03],
									value [04],
									value [05],
									value [06],
									value [07],
									value [08],
									value [09],
									value [10],
									value [11],
									value [12],
									value [13]
									);

								break;

							case "работник":
								valueRes[00] = Convert.ToString ( reader[ colNameRes[00] ] );
								valueRes[01] = Convert.ToString ( reader[ colNameRes[01] ] );
								valueRes[02] = Convert.ToString ( reader[ colNameRes[02] ] );
								valueRes[03] = Convert.ToString ( reader[ colNameRes[03] ] );
								valueRes[04] = Convert.ToString ( reader[ colNameRes[04] ] );
								valueRes[05] = Convert.ToString ( reader[ colNameRes[05] ] );
								
								storeTrd.AppendValues (
									valueRes [00],
									valueRes [01],
									valueRes [02],
									valueRes [03],
									valueRes [04],
									valueRes [05]
									);
								
								break;

							case "материал":
								valueRes[00] = Convert.ToString ( reader[ colNameRes[00] ] );
								valueRes[01] = Convert.ToString ( reader[ colNameRes[01] ] );
								valueRes[02] = Convert.ToString ( reader[ colNameRes[02] ] );
								valueRes[03] = Convert.ToString ( reader[ colNameRes[03] ] );
								valueRes[04] = Convert.ToString ( reader[ colNameRes[04] ] );
								valueRes[05] = Convert.ToString ( reader[ colNameRes[05] ] );
								
								storeMtr.AppendValues (
									valueRes [00],
									valueRes [01],
									valueRes [02],
									valueRes [03],
									valueRes [04],
									valueRes [05]
									);
								
								break;

							case "механизация":
								valueRes[00] = Convert.ToString ( reader[ colNameRes[00] ] );
								valueRes[01] = Convert.ToString ( reader[ colNameRes[01] ] );
								valueRes[02] = Convert.ToString ( reader[ colNameRes[02] ] );
								valueRes[03] = Convert.ToString ( reader[ colNameRes[03] ] );
								valueRes[04] = Convert.ToString ( reader[ colNameRes[04] ] );
								valueRes[05] = Convert.ToString ( reader[ colNameRes[05] ] );
								
								storeMhn.AppendValues (
									valueRes [00],
									valueRes [01],
									valueRes [02],
									valueRes [03],
									valueRes [04],
									valueRes [05]
									);
								
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


	}
}

