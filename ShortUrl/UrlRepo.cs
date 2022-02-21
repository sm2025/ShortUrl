using System;
using System.Collections.Generic;
using System.Text;

namespace ShortUrl
{
	public static class UrlRepo
	{
		private static readonly ShortUrlSingleton ShortUrlSingletonInstance = ShortUrlSingleton.GetInstance;
		private static readonly string ShortUrlPrefix = ShortUrlSingletonInstance.GetShortUrlPrefix();

		/// <summary>
		/// Dictionary to record ShortUrlSuffix to LongUrl object mapping
		/// </summary>
		public static Dictionary<int, LongUrl> UrlDictionary = new Dictionary<int, LongUrl>();

		/// <summary>
		/// Adds New Long Url object to UrlDictinoary
		/// </summary>
		/// <param name="urlDictionary">Url Dictionary</param>
		/// <param name="longUrl">LongUrl string</param>
		/// <param name="customShortUrl">Optional Custom Short Url</param>
		/// <returns></returns>
		public static string AddNewUrl(this Dictionary<int, LongUrl> urlDictionary, Uri longUrl, string customShortUrl = "")
		{
			var longUrlObject = new LongUrl(longUrl);
			string shortUrlSuffix;
			int shortUrlInt;
			var keyAlreadyExists = true; 
			if (customShortUrl.Length == 0)
			{
				shortUrlSuffix = ShortUrlEncoder.GenerateNewShortUrlSuffix(out shortUrlInt);
				while (keyAlreadyExists)
				{
					shortUrlSuffix = ShortUrlEncoder.GenerateNewShortUrlSuffix(out shortUrlInt);
					keyAlreadyExists = urlDictionary.ContainsKey(shortUrlInt);
				}
			}
			else
			{
				shortUrlSuffix = customShortUrl.Substring(ShortUrlPrefix.Length);
				shortUrlInt = ShortUrlEncoder.ShortUrlSuffixToInt(shortUrlSuffix);
				keyAlreadyExists = urlDictionary.ContainsKey(shortUrlInt);
				if (keyAlreadyExists)
				{
					throw new Exception("Error: Custom Short Url suffix conflict. Please try again.");
				}
			}

			var urlAdded = urlDictionary.TryAdd(shortUrlInt, longUrlObject);
			if (!urlAdded)
				throw new Exception("Error: Unable to add LongUrlObject to UrlDictionary.");
			return shortUrlSuffix;
		}

		/// <summary>
		/// Tries to Get LongUrl Object from UrlDictionary
		/// </summary>
		/// <param name="shortUrl">ShortUrl</param>
		/// <param name="longUrl">LongUrl Object if found</param>
		/// <returns>True if Found</returns>
		public static bool TryGetLongUrl(string shortUrl, out LongUrl longUrl)
		{
			var shortUrlSuffix = shortUrl.Substring(ShortUrlPrefix.Length);
			var shortUrlInt = ShortUrlEncoder.ShortUrlSuffixToInt(shortUrlSuffix);
			var found = UrlDictionary.TryGetValue(shortUrlInt, out longUrl);
			if (!found)
			{
				return false;
			}

			return true;
		}
	}
}
