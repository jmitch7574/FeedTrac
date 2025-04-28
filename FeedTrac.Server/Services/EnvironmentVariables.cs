namespace FeedTrac.Server.Services;

/// <summary>
/// Class for loading environment variables from a .env file
///
/// Source: https://dusted.codes/dotenv-in-dotnet
/// 
/// </summary>
public class EnvironmentVariables
{
	/// <summary>
	/// Path of the .env file
	/// </summary>
	static readonly string ENVPATH = Path.Combine(".", ".env");

	public static void Load()
	{
		if (!File.Exists(ENVPATH))
			return;

		foreach (var line in File.ReadAllLines(ENVPATH))
		{
			var parts = line.Split(
				'=',
				StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length != 2)
				continue;

			Environment.SetEnvironmentVariable(parts[0], parts[1]);
		}
	}
}