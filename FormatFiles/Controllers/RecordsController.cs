using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FormatFiles.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FormatFiles.Controllers
{
    [Route("api/[controller]")]
    public class RecordsController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public RecordsController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        private Result GetAllResult()
        {
            var files = FileLister.ListFiles(_hostingEnvironment);
            var fileParser = new FileParser();

            var spaceFilePath = "";
            var commaFilePath = "";
            var pipFilePath = "";
            //Setup the Delimitor
            foreach (var file in files)
            {
                var result = fileParser.DetermineDelimiterType(file);
                switch (result)
                {
                    case "Space":
                        spaceFilePath = file;
                        break;
                    case "Comma":
                        commaFilePath = file;
                        break;
                    case "Pip":
                        pipFilePath = file;
                        break;
                    default:
                        throw new InvalidDataException("The data is incorrect Delimited");
                }
            }
            var dataList_space = new List<Person>();
            var dataList_comma = new List<Person>();
            var dataList_pip = new List<Person>();
            //Parse the files
            if (!string.IsNullOrEmpty(pipFilePath))
            {
                dataList_pip = fileParser.ParseFile(pipFilePath, '|');
            }
            if (!string.IsNullOrEmpty(commaFilePath))
            {
                dataList_comma = fileParser.ParseFile(commaFilePath, ',');
            }
            if (!string.IsNullOrEmpty(spaceFilePath))
            {
                dataList_space = fileParser.ParseFile(spaceFilePath, ' ');
            }

            return new Result
            {
                commaResult = dataList_comma,
                pipResult = dataList_pip,
                spaceResult = dataList_space
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
            var fileParser = new FileParser();

            var spaceFilePath = "";
            var commaFilePath = "";
            var pipFilePath = "";
            //Setup the Delimitor
            foreach (var file in files)
            {
                var result = fileParser.DetermineDelimiterType(file);
                switch (result)
                {
                    case "Space":
                        spaceFilePath = file;
                        break;
                    case "Comma":
                        commaFilePath = file;
                        break;
                    case "Pip":
                        pipFilePath = file;
                        break;
                    default:
                        throw new InvalidDataException("The data is incorrect Delimited");
                }
            }

            if (!string.IsNullOrEmpty(pipFilePath))
            {
                using (var pipWriter = fileParser.CreateStreamWriter(pipFilePath))
                {
                    pipWriter.WriteLine(
                    $"{person.LastName}|{person.FirstName}|{person.Gender}|{person.FavoriteColor}|{person.DateofBirth:M/d/yyyy}");
                }
            }
            if (!string.IsNullOrEmpty(commaFilePath))
            {
                using (var commaWriter = fileParser.CreateStreamWriter(commaFilePath))
                {
                    commaWriter.WriteLine(
                    $"{person.LastName},{person.FirstName},{person.Gender},{person.FavoriteColor},{person.DateofBirth:M/d/yyyy}");
                }
            }
            if (!string.IsNullOrEmpty(spaceFilePath))
            {
                using (var spaceWriter = fileParser.CreateStreamWriter(spaceFilePath))
                {
                    spaceWriter.WriteLine(
                   $"{person.LastName} {person.FirstName} {person.Gender} {person.FavoriteColor} {person.DateofBirth:M/d/yyyy}");
                }
               
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