using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Exceptions
{
    public class UnauthorizedException(string message) : Exception(message);
}
