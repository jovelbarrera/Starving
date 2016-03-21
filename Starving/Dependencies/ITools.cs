using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Starving.Dependencies
{
	public interface ITools
	{
		byte[] DownloadImageFromURL (string address);

		Task<Dictionary<string, double>> GetCurrentLocation ();

		void OpenWaze (double latitude, double longitude);
	}
}

