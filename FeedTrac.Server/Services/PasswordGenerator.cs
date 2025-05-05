namespace FeedTrac.Server.Services;

public class PasswordGenerator
{
	public static string GeneratePassword()
	{
		string lowercase = "abcdefghijklmnopqrstuvwxyz";
		string digits = "0123456789";
		string symbol = "!@#$%^&*";

		string lowercasePass = new string(lowercase.OrderBy(x => Guid.NewGuid()).ToArray()[0..5]);
		string uppercasePass = new string(lowercase.ToUpper().OrderBy(x => Guid.NewGuid()).ToArray()[0..5]);
		string digitsPass = new string(digits.OrderBy(x => Guid.NewGuid()).ToArray()[0..5]);
		string symbols = new string(symbol.OrderBy(x => Guid.NewGuid()).ToArray()[0..2]);

		return new string((lowercasePass + uppercasePass + digitsPass + symbols).OrderBy(x => Guid.NewGuid()).ToArray());
	}
}