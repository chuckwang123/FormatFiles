using System;
using System.IO;
using System.Linq;
using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class BootStrapper
    {
        private FileParser tempParser;
        private SpaceFileParserFactory spaceFactory;
        private CommaFileParserFactory commaFactory;
        private PipFileParserFactory pipFactory;

        public BootStrapper() : this(new FormatFileFactory()) { }
        public BootStrapper(IFactory factory)
        {
            if (factory == null) { throw new ArgumentNullException(nameof(factory)); }
            tempParser = new FileParser(factory);
            spaceFactory = new SpaceFileParserFactory(factory);
            commaFactory = new CommaFileParserFactory(factory);
            pipFactory = new PipFileParserFactory(factory);
        }

        public void Sort(string sortWay)
        {
            var files = FileLister.ListFiles();
            foreach (var file in files)
            {
                Console.WriteLine($"The file path is {file}");
            }

            //Setup the Delimitor and parse
            foreach (var file in files)
            {
                tempParser.SetupPath(file);
                var result = tempParser.DetermineDelimiterType();
                switch (result)
                {
                    case "Space":
                        spaceFactory.Setup(tempParser);
                        break;
                    case "Comma":
                        commaFactory.Setup(tempParser);
                        break;
                    case "Pip":
                        pipFactory.Setup(tempParser);
                        break;
                    default:
                        throw new InvalidDataException("The data is incorrect Delimited");
                }
            }

            //output

            var enterValue = sortWay.ToUpper();

            switch (enterValue)
            {
                case "GENDER":
                    pipFactory.ResultData = Helper.SortByGender(pipFactory.OriData).ToList();
                    commaFactory.ResultData = Helper.SortByGender(commaFactory.OriData).ToList();
                    spaceFactory.ResultData = Helper.SortByGender(spaceFactory.OriData).ToList();
                    break;
                case "BIRTH":
                    pipFactory.ResultData = Helper.SortByBod(pipFactory.OriData).ToList();
                    commaFactory.ResultData = Helper.SortByBod(pipFactory.OriData).ToList();
                    spaceFactory.ResultData = Helper.SortByBod(spaceFactory.OriData).ToList();
                    break;
                case "LASTNAME":
                    pipFactory.ResultData = Helper.SortByLastName(pipFactory.OriData).ToList();
                    commaFactory.ResultData = Helper.SortByLastName(pipFactory.OriData).ToList();
                    spaceFactory.ResultData = Helper.SortByLastName(spaceFactory.OriData).ToList();
                    break;
                default:
                    Console.WriteLine("The value you entered is wrong, see you!!!!");
                    return;
            }
            Console.WriteLine("###################### Here is the result for PIP delimitor #######################");
            Helper.OutputData(pipFactory.ResultData);
            Console.WriteLine("###################### Here is the result for COMMA delimitor #######################");
            Helper.OutputData(commaFactory.ResultData);
            Console.WriteLine("###################### Here is the result for SPACE delimitor #######################");
            Helper.OutputData(spaceFactory.ResultData);
        }
    }
}
