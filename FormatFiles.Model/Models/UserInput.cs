using System;
using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class UserInput : IUserInput
    {
        public string GetInput()
        {
            return Console.ReadLine();
        }
    }
}
