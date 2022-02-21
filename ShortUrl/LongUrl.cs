using System;
using System.Collections.Generic;
using System.Text;

namespace ShortUrl
{
	public class LongUrl
	{
		private Uri Url { get; }
		private DateTime TimeStamp { get; }
		public int RetrievalCount { get; set; }

		public LongUrl(Uri url)
		{
			Url = url;
			TimeStamp = DateTime.Now;
			RetrievalCount = 0; 
		}

		/// <summary>
		/// Prints Long Url to Console
		/// </summary>
		public void PrintLongUri()
		{
			Console.WriteLine($"The Long URI is : {this.Url}");  
		}

		/// <summary>
		/// Adds Retrieval Count
		/// </summary>
		public void AddRetrievalCount()
		{
			RetrievalCount += 1; 
		}

		/// <summary>
		/// Prints Retrieval Count to Console
		/// </summary>
		public void PrintRetrivalCount()
		{
			Console.WriteLine($"This short url has been retrieved {RetrievalCount} times.");
		}
	}
}
