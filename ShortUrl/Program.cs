using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;

namespace ShortUrl
{
	class Program
	{
		static void Main(string[] args)
		{
			ShortUrlSingleton shortUrlSingleton = ShortUrlSingleton.GetInstance;
			string shortUrlPrefix = shortUrlSingleton.GetShortUrlPrefix();

			Console.WriteLine("Welcome to ShortUrl!!! ");
			while (true)
			{
				Console.WriteLine();
				Console.WriteLine("*************************************************");
				Console.WriteLine("Enter 1 to create a ShortUrl from a Long Url");
				Console.WriteLine("Enter 2 to Retrieve a Long Url from a ShortUrl");
				Console.WriteLine("Enter 3 to View ShortUrl retrieval statistics");
				Console.WriteLine("Enter 4 to Delete a ShortUrl");
				Console.WriteLine("Enter 0 to Quit");

				string userSelection = Console.ReadLine();
				try
				{
					if (int.TryParse(userSelection, out int userSelectionInt))
					{
						//Purchase
						if (userSelectionInt == 1)
						{
							Console.WriteLine();
							Console.WriteLine("Enter a long URL or type \"menu\" to go back to Main Menu");
							var inputLongUrl = Console.ReadLine();
							if (inputLongUrl.ToLower() == "menu")
								continue;
							Uri longUri; 
							//PendingValidation
							try
							{
								longUri = new Uri(inputLongUrl);
							}
							catch (Exception)
							{
								throw new Exception($"Invalid Input. Please enter a valid Long URL in the format of: scheme://server/path/resource. Example: https://www.google.com/");
							}
							while (true)
							{
								Console.WriteLine();
								Console.WriteLine("Enter 1 to use a Custom Short Url Suffix");
								Console.WriteLine("Enter 2 to use system generated Custom Short Url");
								Console.WriteLine("Type \"menu\" to go back to Main Menu");

								var shortUrlSelection = Console.ReadLine();
								if (shortUrlSelection.ToLower() == "menu")
									break;
								if (int.TryParse(shortUrlSelection, out int shortUrlSelectionInt))
								{
									//Purchase
									if (shortUrlSelectionInt == 1)
									{
										while (true)
										{
											Console.WriteLine("Please Enter a Custom suffix (up to 5 alphanumeric characters):");
											var customShortUrl = Console.ReadLine();
											customShortUrl = shortUrlPrefix + customShortUrl;
											var isShortUrlValid = ShortUrlEncoder.ValidateShortUrl(customShortUrl);
											if (!isShortUrlValid)
												continue;
											var shortUrlExistsAlready = UrlRepo.TryGetLongUrl(customShortUrl, out var longUrl);
											if (shortUrlExistsAlready)
											{
												Console.WriteLine($"{customShortUrl} already exists. Please enter a unique short url suffix.");
												continue;
											}

											var shortUrlSuffix = UrlRepo.UrlDictionary.AddNewUrl(longUri,  customShortUrl);
											Console.WriteLine($"The Short URl is: {shortUrlPrefix + shortUrlSuffix}");

											break;
										}

										break; 
									}

									if (shortUrlSelectionInt == 2)
									{
										var shortUrlSuffix = UrlRepo.UrlDictionary.AddNewUrl(new Uri(inputLongUrl));
										Console.WriteLine($"The Short URl is: {shortUrlPrefix}{shortUrlSuffix}");
										break;
									}
									Console.WriteLine("Invalid Input. Please try again.");
									continue;
								}
								Console.WriteLine($"Invalid Input. Please try again.");
							}

							continue;

						}

						if (userSelectionInt == 2) 
						{
							while (true)
							{
								Console.WriteLine();
								Console.WriteLine("Enter a Short URL in the format of https://surl.com/xxxxx or type \"menu\" to go back to Main Menu");
								var inputShortUrl = Console.ReadLine();
								if (inputShortUrl.ToLower() == "menu")
									break;
								var isShortUrlValid = ShortUrlEncoder.ValidateShortUrl(inputShortUrl);
								if (!isShortUrlValid)
									continue;

								var shortUrlExists = UrlRepo.TryGetLongUrl(inputShortUrl, out var longUrlObject);
								if (shortUrlExists)
								{
									longUrlObject.PrintLongUri();
									longUrlObject.AddRetrievalCount();
									break;
								}
								Console.WriteLine("Short URI not found or is expired. Please enter a valid Short URI or type \"menu\" to go to Main Menu");
							}
							continue;
						}

						if (userSelectionInt == 3)
						{
							while (true)
							{
								Console.WriteLine();
								Console.WriteLine("Enter ShortUrl in the format of https://surl.com/xxxxx to retrieve stats.");
								Console.WriteLine("Type \"menu\" to go back to Main Menu");
								var shortUrlInput = Console.ReadLine();
								if (shortUrlInput.ToLower() == "menu")
									break;
								var isShortUrlValid = ShortUrlEncoder.ValidateShortUrl(shortUrlInput);
								if (!isShortUrlValid)
									continue;

								var shortUrlExists = UrlRepo.TryGetLongUrl(shortUrlInput, out var longUrlObject);
								if (shortUrlExists)
								{
									longUrlObject.PrintLongUri();
									longUrlObject.PrintRetrivalCount();
									break;
								}
								Console.WriteLine("Short URI not found or is expired. Please enter a valid Short URI.");
							}
							continue;
						}

						if (userSelectionInt == 4)
						{
							while (true)
							{
								Console.WriteLine();
								Console.WriteLine("Enter ShortUrl in the format of https://surl.com/xxxxx to Delete.");
								Console.WriteLine("Type \"menu\" to go back to Main Menu");
								var shortUrlInput = Console.ReadLine();
								if (shortUrlInput.ToLower() == "menu")
									break;
								var shortUrlIsValid = ShortUrlEncoder.ValidateShortUrl(shortUrlInput);
								if (!shortUrlIsValid)
									continue;

								var shortUrlExists = UrlRepo.TryGetLongUrl(shortUrlInput, out var longUrlObject);
								if (shortUrlExists)
								{
									UrlRepo.UrlDictionary.Remove(ShortUrlEncoder.ShortUrlSuffixToInt(shortUrlInput.Substring(shortUrlPrefix.Length)));
									Console.WriteLine($"{shortUrlInput} has been deleted from records.");
									break;
								}
								Console.WriteLine("Short URI not found or is expired. Please enter a valid Short URI.");
							}
							continue;
						}

						if (userSelectionInt == 0)
						{
							Console.WriteLine("Exiting the system.");
							break;
						}
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					continue;
				}
				Console.WriteLine("Input Not Valid. Please Enter a valid input");
			}

		}

	}

}