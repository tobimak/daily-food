using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Exceptions
{
    public class BusinessException(string message) : Exception(message);
}
