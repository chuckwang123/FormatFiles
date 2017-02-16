using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
        private List<Task> _taskList = new List<Task>();

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
        public async Task<HttpResponseMessage> Post(Person person)
        {
            if (person == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "the person is null"));
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
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "The data is incorrect Delimited"));
                }
            }

            try
            {
                var pipTask = _pipFactory.WriteRecord(person);
                var commaTask = _commaFactory.WriteRecord(person);
                var spaceTask = _spaceFactory.WriteRecord(person);

                await Task.WhenAll(commaTask, pipTask, spaceTask);
                if (pipTask.Result == 0 && commaTask.Result == 0 && spaceTask.Result ==0)
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "There is no data has been updated");
                }
                return Request.CreateResponse(HttpStatusCode.OK, "The data is updated successed");
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, e.Message));
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
