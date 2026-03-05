using Microsoft.AspNetCore.Http;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Services
{
    
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId
        {
            get
            {
                var claims = _httpContextAccessor.HttpContext?.User?.Claims;

                return claims?.FirstOrDefault(c =>
                    (c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")
                    && Guid.TryParse(c.Value, out _))?.Value;
            }
        }
    }
}
