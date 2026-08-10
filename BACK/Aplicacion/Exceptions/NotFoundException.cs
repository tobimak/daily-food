using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Exceptions
{
    public class NotFoundException(string message) : Exception(message);
}
