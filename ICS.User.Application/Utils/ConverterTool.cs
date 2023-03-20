namespace ICS.User.Application.Utils;

public static class ConverterTool
{
    public static string ExtractDataFromBase64(string dataBase64)
    {
        byte[] dataBinary = Convert.FromBase64String(dataBase64);
        string data = System.Text.Encoding.UTF8.GetString(dataBinary);
        return data;
    }
}
