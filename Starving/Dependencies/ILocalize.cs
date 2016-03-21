using System;
using System.Globalization;

namespace Starving.Dependencies
{
	public interface ILocalize
	{
		CultureInfo GetCurrentCultureInfo ();
	}
}

