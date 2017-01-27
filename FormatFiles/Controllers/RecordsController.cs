using System;
using System.IO;
using System.Linq;
using FormatFiles.Interfaces;
using FormatFiles.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FormatFiles.Controllers
{
    [Route("api/[controller]")]
    public class RecordsController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public RecordsController() : this(new FormatFileFactory()) { }
        public RecordsController(IFactory factory)
        {
            if (factory == null) { throw new ArgumentNullException(nameof(factory)); }
            _hostingEnvironment = factory.HostingEnvironment;
        }

        private Result GetAllResult()
        {
            var files = FileLister.ListFiles(_hostingEnvironment);
            var spaceFactory = new SpaceFileParserFactory();
            var commaFactory = new CommaFileParserFactory();
            var pipFactory = new PipFileParserFactory();

            //Setup the Delimitor
            foreach (var file in files)
            {
                var tempParser = new FileParser(file);
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

        [HttpPost]
        public void Post([FromBody]Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            var files = FileLister.ListFiles(_hostingEnvironment);
            var spaceFactory = new SpaceFileParserFactory();
            var commaFactory = new CommaFileParserFactory();
            var pipFactory = new PipFileParserFactory();

            //Setup the Delimitor
            foreach (var file in files)
            {
                var tempParser = new FileParser(file);
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