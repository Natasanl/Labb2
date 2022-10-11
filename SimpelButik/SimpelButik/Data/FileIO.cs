using System.Reflection;

namespace SimpelButik.Data;

    public class FileIO
    {
        public static void AddData(string fileName, string data)
        {
            if (!File.Exists(fileName))
            {
                using StreamWriter streamWriter = new(fileName);
                streamWriter.WriteLine(data);
            }
        }

        public static void UpdateData(string fileName, string data)
        {
            if (File.Exists(fileName))
            {
                using StreamWriter streamWriter = new(fileName);
                streamWriter.WriteLine(data);
            }
        }

        public static string? ReadData(string fileName)
        {
            var data = new System.Text.StringBuilder();
            if (File.Exists(fileName))
            {
                using StreamReader streamReader = new(fileName);
                data.Append(streamReader.ReadToEnd());
            }
            return data.ToString();
        }
    }

