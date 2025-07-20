namespace CubosFinance.Application.Common;

public static class Helper
{
    public static string GetOnlyDigits(string text)
    {
        return new string(text.Where(char.IsDigit).ToArray());
    }
}