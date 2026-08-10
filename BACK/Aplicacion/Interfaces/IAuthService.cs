using Aplicacion.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegistrarAsync(RegistroRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
