using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using FormatFiles.Model.Interfaces;
using FormatFiles.Model.Models;

namespace FormatFiles.API.Controllers
{
    [RoutePrefix("api/Records")]
    public class RecordsController : ApiController
    {
        private FileParser tempParser;
        private SpaceFileParserFactory spaceFactory;
        private CommaFileParserFactory commaFactory;
        private PipFileParserFactory pipFactory;

        public RecordsController() : this(new FormatFileFactory()) { }
        public RecordsController(IFactory factory)
        {
            if (factory == null) { throw new ArgumentNullException(nameof(factory)); }
            tempParser = new FileParser(factory);
            spaceFactory = new SpaceFileParserFactory(factory);
            commaFactory = new CommaFileParserFactory(factory);
            pipFactory = new PipFileParserFactory(factory);
        }

        private Result GetAllResult()
        {
            var files = FileLister.ListWebFiles();
            
            //Setup the Delimitor
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

            return new Result
            {
                commaResult = commaFactory.OriData,
                pipResult = pipFactory.OriData,
                spaceResult = spaceFactory.OriData
            };
        }

        [Route("")]
        public void Post(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            var files = FileLister.ListWebFiles();

            //Setup the Delimitor
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

            pipFactory.WriteRecord(person);
            commaFactory.WriteRecord(person);
            spaceFactory.WriteRecord(person);
        }

        [HttpGet, Route("gender")]
        public Result SortByGender()
        {
            var result = GetAllResult();

            return new Result
            {
                pipResult = PeopleSorter.SortByGender(result.pipResult).ToList(),
                commaResult = PeopleSorter.SortByGender(result.commaResult).ToList(),
                spaceResult = PeopleSorter.SortByGender(result.spaceResult).ToList()
            };
        }

        [HttpGet, Route("birthdate")]
        public Result SortByBirth()
        {
            var result = GetAllResult();

            return new Result
            {
                pipResult = PeopleSorter.SortByBod(result.pipResult).ToList(),
                commaResult = PeopleSorter.SortByBod(result.commaResult).ToList(),
                spaceResult = PeopleSorter.SortByBod(result.spaceResult).ToList()
            };
        }

        [HttpGet, Route("name")]
        public Result SortByName()
        {

            var result = GetAllResult();
            return new Result
            {
                pipResult = PeopleSorter.SortByName(result.pipResult).ToList(),
                commaResult = PeopleSorter.SortByName(result.commaResult).ToList(),
                spaceResult = PeopleSorter.SortByName(result.spaceResult).ToList()
            };
        }
    }
}
