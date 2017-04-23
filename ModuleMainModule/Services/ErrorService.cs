using ModuleMainModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ModuleMainModule.Services
{
    class ErrorService : IErrorService
    {
        public bool ValidateInput(string input, out ICollection<string> validationErrors)
        {
            validationErrors = new List<string>();
            
            if (input.Length > 50 || input.Length < 2)
                validationErrors.Add("Количество введенных символов должно быть от 2 до 50");
           
            if (!Regex.IsMatch(input, @"^[a-zA-Z]+$"))
                validationErrors.Add("В строке могут быть только символы (a-z, A-Z).");

            return validationErrors.Count == 0;
        }
    }
}
