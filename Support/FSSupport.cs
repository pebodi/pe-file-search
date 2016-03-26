using System;
using System.Globalization;

namespace FileSearch
{
	public class FSSupport
	{
		public FSSupport ()
		{
		}

		public double getDouble ( string _number, double _escape )
		{
			
			double defaultValue = _escape;
			
			double result;
			
			//Try parsing in the current culture
			if (!double.TryParse(_number, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
			    //Then try in US english
			    !double.TryParse(_number, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
			    //Then in neutral language
			    !double.TryParse(_number, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				result = defaultValue;
			}
			return result;
		}
		
		public string getString (double _number, string _escape)
		{
			try {
				
				//Прецизиране на закръглението
				_number = Math.Round ( _number, 2 );
				
				
				return  _number.ToString (  ); //String.Format ( "D2", _number);
			} catch {
			}
			
			return _escape;
		}
		
		public string getString ( string _number, string _escape )
		{
			try {
				double _result = getDouble ( _number, getDouble( _escape, 0 ) );

				
				//Прецизиране на закръглението
				_result = Math.Round ( _result, 2 );
				
				
				return  _result.ToString (  ); //String.Format ( "D2", _result );
				
			} catch {
			}
			
			return _escape;
		}
	}
}

