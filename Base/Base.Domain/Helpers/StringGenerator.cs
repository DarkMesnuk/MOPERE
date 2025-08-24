namespace Base.Domain.Helpers;

public static class StringGenerator
{
	private static readonly Random Random = new();
	
	public static string RandomString(int length)
	{
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[Random.Next(s.Length)]).ToArray());
	}

	public static string GeneratePassword()
	{
		const string numbers = "0123456789";
		const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
		const string symbols = "!@#$%^&*&()_+=-";

		var password = numbers.Shuffle().Substring(0, 5) +
		               upperCase.Shuffle().Substring(0, 5) +
		               lowerCase.Shuffle().Substring(0, 5) +
		               symbols.Shuffle().Substring(0, 5);

		return password.Shuffle();

	}

	public static string RandomStringNumber(int length)
	{
		const string chars = "0123456789";
		
		return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[Random.Next(s.Length)]).ToArray());
	}


	private static string Shuffle(this string str)
	{
		var array = str.ToCharArray();
		var rng = new Random();
		var n = array.Length;
		
		while (n > 1)
		{
			n--;
			var k = rng.Next(n + 1);
			(array[k], array[n]) = (array[n], array[k]);
		}
		
		return new string(array);
	}
}