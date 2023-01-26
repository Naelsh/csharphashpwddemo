using System.Security.Cryptography;
using System.Text;

public class App
{
    private readonly string dictFile = "example.dict.txt";
    private readonly string hashedFile = "hashadelosen.txt";

    public void Run()
    {
        EnsurePreHashedFileExists();
        var dictionary = GetHashedPasswordDictionary();
        while (true)
        {
            Console.WriteLine("Ange hash:");
            var hash = Console.ReadLine();


            if (!string.IsNullOrEmpty(hash))
            {
                var password = dictionary[hash];
                Console.WriteLine(password);
            }
        }
    }

    private Dictionary<string, string> GetHashedPasswordDictionary()
    {
        string fileName = "hashadelosen.txt";
        string projectPath = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(projectPath, fileName);

        string line;

        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        using (StreamReader sr = new StreamReader(filePath))
        {
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');
                string key = parts[0];
                string value = parts[1];
                dictionary.Add(key, value);
            }
        }
        return dictionary;
    }

    private void EnsurePreHashedFileExists()
    {
        if (File.Exists(hashedFile)) return;

        var writer = File.CreateText(hashedFile);
        foreach (var line in File.ReadLines(dictFile))
        {
            var hash = GenerateHash(line);
            writer.WriteLine(hash + " " + line);
        }
        writer.Close();
    }

    private string GenerateHash(string line)
    {
        using (var md5 = MD5.Create())
        {
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(line)))
                .Replace("-", "");
        }
    }
}