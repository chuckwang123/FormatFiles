using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using FormatFiles.Model.Exception;
using FormatFiles.Model.Interfaces;
using FormatFiles.Model.Models;

namespace FormatFiles.API.Controllers
{
    [RoutePrefix("api/Records")]
    public class RecordsController : ApiController
    {
        private readonly FileParser _tempParser;
        private readonly SpaceFileParserFactory _spaceFactory;
        private readonly CommaFileParserFactory _commaFactory;
        private readonly PipFileParserFactory _pipFactory;
        private readonly FileLister _fileLister;

        public RecordsController() : this(new FormatFileFactory()) { }
        public RecordsController(IFactory factory)
        {
            if (factory == null) { throw new ArgumentNullException(nameof(factory)); }
            _tempParser = new FileParser(factory);
            _spaceFactory = new SpaceFileParserFactory(factory);
            _commaFactory = new CommaFileParserFactory(factory);
            _pipFactory = new PipFileParserFactory(factory);
            _fileLister = new FileLister(factory);
        }

        private Result GetAllResult()
        {
            var files = _fileLister.ListWebFiles();
            
            //Setup the Delimitor
            foreach (var file in files)
            {
                _tempParser.SetupPath(file);
                var result = _tempParser.DetermineDelimiterType();
                switch (result)
                {
                    case "Space":
                        _spaceFactory.Setup(_tempParser);
                        break;
                    case "Comma":
                        _commaFactory.Setup(_tempParser);
                        break;
                    case "Pip":
                        _pipFactory.Setup(_tempParser);
                        break;
                    default:
                        throw new InvalidDataException("The data is incorrect Delimited");
                }
            }

            return new Result
            {
                commaResult = _commaFactory.OriData,
                pipResult = _pipFactory.OriData,
                spaceResult = _spaceFactory.OriData
            };
        }

        [Route("")]
        public void Post(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            var files = _fileLister.ListWebFiles();

            //Setup the Delimitor
            foreach (var file in files)
            {
                _tempParser.SetupPath(file);
                var result = _tempParser.DetermineDelimiterType();
                switch (result)
                {
                    case "Space":
                        _spaceFactory.Setup(_tempParser);
                        break;
                    case "Comma":
                        _commaFactory.Setup(_tempParser);
                        break;
                    case "Pip":
                        _pipFactory.Setup(_tempParser);
                        break;
                    default:
                        throw new InvalidDataException("The data is incorrect Delimited");
                }
            }

            try
            {
                _pipFactory.WriteRecord(person);
                _commaFactory.WriteRecord(person);
                _spaceFactory.WriteRecord(person);
            }
            catch (Exception e)
            {
                throw new FormatFilesException(e.Message);
            }
            
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
