using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ShortUrl
{
	public static class Utilities
	{
		/// <summary>
		/// Int Exponent operation for Int 
		/// </summary>
		/// <param name="baseInt">Base Int</param>
		/// <param name="powInt">Exponent</param>
		/// <returns></returns>
		public static int IntPowerInt(this int baseInt, int powInt)
		{
			var result = 1;
			if (powInt == 0)
				return 1; 
			for (int pow = 0; pow < powInt; pow++)
			{
				result = result * baseInt;
			}

			return result; 
		}

		/// <summary>
		/// Converts int List to Base Int
		/// </summary>
		/// <param name="intList">List of Ints </param>
		/// <param name="baseInt">Base Int</param>
		/// <returns></returns>
		public static int IntListOnBaseIntToBaseTen(this List<int> intList, int baseInt )
		{
			int result = 0;
			for (int i = 0; i <= intList.Count - 1; i++)
			{
				if (intList[i] >= baseInt)
					throw new Exception($"Error Converting to from Base{baseInt} to Base10. List<int> not in Base{baseInt}");

				var intInBaseInt = baseInt.IntPowerInt(intList.Count - 1 - i) * intList[i];
				result += intInBaseInt;
			}

			return result;
		}
	}
}
