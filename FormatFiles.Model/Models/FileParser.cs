using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class FileParser
    {
        private readonly IStreamReader StreamReader;
        private string _filePath;
        public FileParser(IFactory factory)
        {
            if (factory == null) { throw new ArgumentNullException(nameof(factory)); }
            StreamReader = factory.CustomStreamReader;
        }

        public FileParser(FileParser fileParser)
        {
            StreamReader = fileParser.StreamReader;
            _filePath = fileParser._filePath;
        }

        public void SetupPath(string filepath)
        {
            _filePath = filepath;
        }
        public List<Person> ParseFile(string type)
        {
            var delimitor = '@';
            switch (type)
            {
                case "Space":
                    delimitor = ' ';
                    break;
                case "Pip":
                    delimitor = '|';
                    break;
                case "Comma":
                    delimitor = ',';
                    break;
            }

            StreamReader.SetPath(_filePath);
            StreamReader.SetupStreamReaderWrapper();
            var listOfObject = new List<Person>();
            var provider = CultureInfo.InvariantCulture;
            try
            {   // Open the text file using a stream reader.
                StreamReader.ReadLine();
                string line;
                while ((line = StreamReader.ReadLine()) != null)
                {
                    var words = line.Split(delimitor);
                    listOfObject.Add(new Person
                    {
                        LastName = words[0],
                        FirstName = words[1],
                        Gender = words[2],
                        FavoriteColor = words[3],
                        DateofBirth = DateTime.ParseExact(words[4], "M/d/yyyy", provider)
                    });
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"The file {_filePath}could not be read:");
                Console.WriteLine(e.Message);
            }
            StreamReader.Dispose();
            return listOfObject;
        }

        private string ReadFile()
        {
            try
            {   // Open the text file using a stream reader.
                StreamReader.SetPath(_filePath);
                return StreamReader.ReadtoEnd();
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"The file {_filePath}could not be read:");
                Console.WriteLine(e.Message);
            }

            return "";
        }

        public string DetermineDelimiterType()
        {
            var delimiters = new List<char> { ' ', ',', '|' };
            var line = ReadFile();
            StreamReader.Dispose();
            foreach (var c in delimiters)
            {
                var num = line.Count(t => t == c);
                if (num <= 0) continue;

                switch (c)
                {
                    case ' ':
                        return "Space";
                    case ',':
                        return "Comma";
                    case '|':
                        return "Pip";

                }
            }
            return "Error";
        }

        public StreamWriter CreateStreamWriter()
        {
            return File.AppendText(_filePath);
        }
    }
}
