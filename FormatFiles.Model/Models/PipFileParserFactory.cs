﻿using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class PipFileParserFactory : FileParserFactory
    {
        public PipFileParserFactory(IFactory factory) : base(factory)
        {
        }

        protected override string Type { get; set; } = "Pip";
        public override void WriteRecord(Person person)
        {
            using (var pipWriter = FileParser.CreateStreamWriter())
            {
                pipWriter.WriteLine(
                $"{person.LastName}|{person.FirstName}|{person.Gender}|{person.FavoriteColor}|{person.DateofBirth:M/d/yyyy}");
            }
            
        }

        public override void Setup(FileParser fileParser)
        {
            this.FileParser = new FileParser(fileParser);
            OriData = FileParser.ParseFile(Type);
        }
    }
}
