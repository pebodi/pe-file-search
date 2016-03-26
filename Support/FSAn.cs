using System;
using System.Collections.Generic;

namespace FileSearch
{
	public class FSAn
	{
		public string osn { get; set; }
		public string sum { get; set; }		//3
		public string nvr { get; set; }		//4
		public string trd { get; set; }		//5
		public string mtr { get; set; }		//6
		public string mhn { get; set; }		//7
		public string usl { get; set; }		//8
		public string dop { get; set; }		//9
		public string pe4 { get; set; }		//10

		public string preki { get; set; }
		public string pd { get; set; }

		private	string[]	_row;
		public	string[] 	dopPrArray;
		public	string[]	dopArray = new string[] { "", "", "", "" };
		public	string		_pe;

		public List<FSAnRows> rows = new List<FSAnRows>();

		private FileSearch.FSSupport _support = new FSSupport ();
	

		public FSAn (string an, string pe)
		{
			try
			{
				string[] temp = an.Split ('@');

				osn = temp[0].Split ('|')[0];
				_row = temp[1].Split ('$');
				dopPrArray = temp[2].Split ('|');
				_pe = pe;

				if ( temp[1].Length > 0 )
				{
					if ( !parseRows ( _row ) )
					{
						Console.WriteLine ( "Грешка при парсването на анали" );
						Console.WriteLine ( "[код] " + osn + " [анализ] " + temp[1] );
					}
				}

				nvr = collectByCode ( "nvr" );
				trd = collectByCode ( "trd" );
				mtr = collectByCode ( "mtr" );
				mhn = collectByCode ( "mhn" );
				usl = collectByCode ( "usl" );

				dop = collectDop ();

				preki = String.Format ( "{0:0.00}",
					         	_support.getDouble ( trd, 0 ) +
					            _support.getDouble ( mtr, 0 ) +
					            _support.getDouble ( mhn, 0 ) +
					            _support.getDouble ( usl, 0 )
				  			);
				pd = String.Format ( "{0:0.00}",
				                    _support.getDouble ( dop, 0 ) +
				                    _support.getDouble ( preki, 0 )
				                    );



				pe4 = collectPe4 ();

				sum = collectSum ();


			}catch{
				Console.WriteLine ( "FSAn (init): Грешка при инициализиране.");
			}
		}

		private bool parseRows (string[] row)
		{
			string _retError = "";
			try {


				for (int i = 0; i < row.Length; i++ )
				{
					_retError = row[i];
					string[] value = row[i].Split ('^');
					rows.Add ( new FSAnRows (
						value[0], value[1], value[2], value[3],
						value[4], value[5], value[6], value[7]
					) );
				}

				return true;
			} catch {
				Console.WriteLine ( "FSAn.parseRows: Грешка при парсване на редове от анализ. ( " + _retError + " )");
			}

			return false;
		}

		private string collectByCode (string code)	//trd, mtr, mhn, usl
		{
			try {
				string _formatString = "{0:0.00}";
				double _result = 0;

				for ( int i = 0; i < _row.Length; i++ )
				{
					string[] value = _row[i].Split ('^');

					if ( code != "nvr" && value[7] == code )
					{
//						_result += Math.Round (
//							_support.getDouble ( value[3], 0 ) *
//							_support.getDouble ( value[4], 0 ) *
//							_support.getDouble ( value[5], 0 )
//							, 2);
						_result += Math.Round ( 
							_support.getDouble ( value[6], 0 )
							, 2);
					}

					if ( code == "nvr" && value[7] == "trd" )
					{
						_formatString = "{0:0.0000}";
						_result += Math.Round ( _support.getDouble ( value[3], 0 ), 2 );
					}
				}

				return String.Format ( _formatString , _result );
			} catch {
			}

			return String.Format ( "{0:0.00}", 0 );
		}

		private string collectDop ()
		{
			try {
				double result = 0;
				double[] _tempDop = new double[] { 0, 0, 0, 0 };

				_tempDop[0] = _support.getDouble ( trd, 0 ) * _support.getDouble ( dopPrArray[0], 0 ) / 100;
				_tempDop[1] = _support.getDouble ( mtr, 0 ) * _support.getDouble ( dopPrArray[1], 0 ) / 100;
				_tempDop[2] = _support.getDouble ( mhn, 0 ) * _support.getDouble ( dopPrArray[2], 0 ) / 100;
				_tempDop[3] = _support.getDouble ( usl, 0 ) * _support.getDouble ( dopPrArray[3], 0 ) / 100;

				result = _tempDop[0] + _tempDop[1] + _tempDop[2] + _tempDop[3];

				dopArray[0] = String.Format ( "{0:0.00}", _tempDop[0] );
				dopArray[1] = String.Format ( "{0:0.00}", _tempDop[1] );
				dopArray[2] = String.Format ( "{0:0.00}", _tempDop[2] );
				dopArray[3] = String.Format ( "{0:0.00}", _tempDop[3] );


				return String.Format ( "{0:0.00}", result );
			} catch {
				Console.WriteLine ( "FSAn.CollectDop: Грешка при събирането на допълнителните разходи" );
			}

			return String.Format ( "{0:0.00}", 0 );
		}

		private string collectPe4 ()
		{
			try {
				double result = 0;
				
				result = 
					_support.getDouble ( preki, 0 ) +
					_support.getDouble ( dop, 0 );

				result *= _support.getDouble ( _pe, 0 ) / 100;

				return String.Format ( "{0:0.00}", result );
			} catch {
				Console.WriteLine ( "FSAn.collectPe4: Грешка при пресмятането на печалба" );
			}
			
			return _support.getString ( "0", "0") ;
		}

		private string collectSum ()
		{
			try {
				double result = 0;
				
				result = 
					_support.getDouble ( trd, 0 ) +
					_support.getDouble ( mtr, 0 ) +
					_support.getDouble ( mhn, 0 ) +
					_support.getDouble ( usl, 0 ) +
					_support.getDouble ( dop, 0 ) + 
					_support.getDouble ( pe4, 0 );
				
				_support.getString ( result, "0" );

				return String.Format ( "{0:0.00}", result );
			} catch {
			}
			
			return String.Format ( "{0:0.00}", 0 ) ;
		}
	}

	public class FSAnRows
	{
		public string osn { get; set; }
		public string opi { get; set; }
		public string mka { get; set; }
		public string nvr { get; set; }
		public string kof { get; set; }
		public string cen { get; set; }
		public string sum { get; set; }
		public string tpe { get; set; }

		public FSAnRows (
			string _osn,
			string _opi,
			string _mka,
			string _nvr,
			string _kof,
			string _cen,
			string _sum,
			string _tpe
		)
		{
							osn = _osn;
							opi = _opi;
							mka = _mka;
							nvr = _nvr;
							kof = _kof;
							cen = _cen;
							sum = _sum;
							tpe = _tpe;
		}

	}
}

