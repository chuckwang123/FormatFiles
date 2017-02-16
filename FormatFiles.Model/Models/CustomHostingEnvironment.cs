using System;
using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class CustomHostingEnvironment : IHostingEnvironment
    {
        public string MapPath(string path)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data"); //todo I am doing here
        }
    }
}
