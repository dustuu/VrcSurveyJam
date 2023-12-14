using System.Runtime.CompilerServices;

namespace VrcSurveyJam;

public static class EnvironmentVariables
{
    public const string GET = "get";
    public const string TABLE_NAME = "MyTable";

    // Storage
    public static string AzureWebJobsStorage => Env();

    // Returns the Environment Variable matching the name of the member which called this method
    private static string Env([CallerMemberName] string variableName = null) =>
        Environment.GetEnvironmentVariable(variableName);
}
