using System.Collections.Generic;

namespace ModuleMainModule.Interfaces
{
    public interface IErrorService
    {
        bool ValidateInput(string input, out ICollection<string> validationErrors);
    }
}
