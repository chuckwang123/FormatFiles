using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FormatFiles.Interfaces;

namespace FormatFiles.Models
{
    public class FileParser
    {
        public IFileOpener fileOpener;
        
        public FileParser()
        { 
            fileOpener = new FIleOpener();
        }

        public List<Person> ParseFile(string filePath, char delimitor)
        {
            var listOfObject = new List<Person>();
            var provider = CultureInfo.InvariantCulture;
            try
            {   // Open the text file using a stream reader.
                using ( var _textReader = fileOpener.OpenFile(filePath))
                {
                    // Skip the header
                    _textReader.ReadLine();
                    string line;
                    while ((line = _textReader.ReadLine()) != null)
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
            }
            catch (Exception e)
            {
                throw e;
            }

            return listOfObject;
        }

        private string ReadFile(string filePath)
        {
            try
            {   // Open the text file using a stream reader.
                using (var _textReader = fileOpener.OpenFile(filePath))
                {
                    // Read the stream to a string, and write the string to the console.
                    return _textReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string DetermineDelimiterType(string filePath)
        {
            var delimiters = new List<char> { ' ', ',', '|' };
            foreach (var c in delimiters)
            {
                var num = ReadFile(filePath).Count(t => t == c);
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

        public StreamWriter CreateStreamWriter(string filePath)
        {
            return File.AppendText(filePath);
        }
    }
}
