using System;
using System.Collections.Generic;
using System.Text;

namespace ShortUrl
{
	public sealed class ShortUrlSingleton
	{
		private static readonly char[] ValidShortUrlCharacters = new char[62];
		public const string ShortUrlPrefix = "https://surl.com/";
		private static ShortUrlSingleton _instance = null;


		public static ShortUrlSingleton GetInstance
		{
			get
			{
				if (_instance == null)
					_instance = new ShortUrlSingleton();
				return _instance;
			}
		}
		private ShortUrlSingleton()
		{
			InitializeShortUrlCharactersArray();
		}

		/// <summary>
		/// Initializes ShortUrl valid characters
		/// </summary>
		private void InitializeShortUrlCharactersArray()
		{
			for (int i = 0; i < 26; i++)
			{
				ValidShortUrlCharacters[i] = (char)(i + (int)('A')); // ASCII of A is 65
				ValidShortUrlCharacters[i + 26] = (char)(i + (int)('a')); // ASCII of a is 97
			}

			for (int i = 52; i < ValidShortUrlCharacters.Length; i++)
			{
				ValidShortUrlCharacters[i] = (char)(i - 52 + (int)('0'));
			}
		}

		public char[] GetValidShortUrlCharacters()
		{
			return ValidShortUrlCharacters; 
		}

		public string GetShortUrlPrefix()
		{
			return ShortUrlPrefix; 
		}

	}

}
