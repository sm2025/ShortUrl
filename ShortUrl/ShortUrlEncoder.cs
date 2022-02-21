using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShortUrl
{
	public static class ShortUrlEncoder
	{
		private static readonly ShortUrlSingleton ShortUrlSingletonInstance = ShortUrlSingleton.GetInstance;
		private static readonly string ShortUrlPrefix = ShortUrlSingletonInstance.GetShortUrlPrefix();
		private static readonly char[] ValidShortUrlChars = ShortUrlSingletonInstance.GetValidShortUrlCharacters();
		private static readonly int BaseInt = ValidShortUrlChars.Length; 

		/// <summary>
		/// Converts ShortUrl Suffix to Int
		/// </summary>
		/// <param name="shortUrlSuffix"></param>
		/// <returns>Int</returns>
		public static int ShortUrlSuffixToInt(string shortUrlSuffix)
		{
			List<int> shortUrlIntList = new List<int>();
			var charArray = shortUrlSuffix.ToCharArray();
			foreach (var c in charArray)
			{
				shortUrlIntList.Add(Array.IndexOf(ValidShortUrlChars, c));
			}

			int result = 0;
			for (int i = 0; i <= shortUrlIntList.Count - 1; i++)
			{
				var shortUrlIntToBase = BaseInt.IntPowerInt(shortUrlIntList.Count - 1 - i) * shortUrlIntList[i];
				result += shortUrlIntToBase;
			}

			return result;
		}

		/// <summary>
		/// Generates new ShortUrl Suffix
		/// </summary>
		/// <param name="shortUrlSuffixInt"></param>
		/// <param name="length"></param>
		/// <returns>ShortUrl suffix</returns>
		public static string GenerateNewShortUrlSuffix(out int shortUrlSuffixInt, int length = 5)
		{
			if (length > 5)
			{
				throw new Exception("Only 5 alphanumeric characters are supported at this time.");
			}
			int[] randomInts = new int[length];
			string result = "";
			for (int i = 0; i < length; i++)
			{
				Random rand = new Random();
				var randomNum = rand.Next(BaseInt);
				randomInts[i] = randomNum;
				result = result + ValidShortUrlChars[randomNum];
			}

			shortUrlSuffixInt = randomInts.ToList().IntListOnBaseIntToBaseTen(BaseInt);

			return result;
		}

		/// <summary>
		/// Converts Int to ShortUrl Suffix
		/// </summary>
		/// <param name="shortUrlInt"></param>
		/// <returns>ShortUrl Suffix</returns>
		public static string ToShortUrl(int shortUrlInt)
		{
			List<int> shortUrlSuffixCharsList = new List<int>();

			while (shortUrlInt > 0)
			{
				shortUrlInt = Math.DivRem(shortUrlInt, BaseInt, out int result);
				shortUrlSuffixCharsList.Add(result);
			}

			shortUrlSuffixCharsList.Reverse();
			string resultShortUrl = "";
			shortUrlSuffixCharsList.ForEach(letter => resultShortUrl += ValidShortUrlChars[letter]);

			return resultShortUrl;
		}

		/// <summary>
		/// Validates Short Url for length, format and valid characters
		/// </summary>
		/// <param name="shortUrl"></param>
		/// <returns></returns>
		public static bool ValidateShortUrl(string shortUrl)
		{
			if (shortUrl?.Length <= ShortUrlPrefix.Length || shortUrl?.Length > ShortUrlPrefix.Length + 5 || shortUrl?.Substring(0, ShortUrlPrefix.Length) != ShortUrlPrefix)
			{
				Console.WriteLine(
					$"ShortUrl validation failure: Please enter a valid Short URL.");
				return false; 
			}

			var shortUrlSuffix = shortUrl.Substring(ShortUrlPrefix.Length); 

			if (!Regex.IsMatch(shortUrlSuffix, "^[a-zA-Z0-9]*$"))
			{
				Console.WriteLine("ShortUrl validation failure: Only Alphanumeric characters are supported at this time.");
				return false;
			}

			return true; 
		}
	}
}
