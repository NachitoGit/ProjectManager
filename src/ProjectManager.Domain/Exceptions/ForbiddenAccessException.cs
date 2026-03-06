using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base("No tienes permisos para realizar esta acción.") { }

        public ForbiddenAccessException(string message) : base(message) { }
    }
}
