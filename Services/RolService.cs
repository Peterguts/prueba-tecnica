using System.Security.Claims;

namespace Api.Services
{
	public class RolService : IRolService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

		public RolService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public string GetRol()
		{
			var result = string.Empty;
			if (_httpContextAccessor.HttpContext is not null)
			{
				result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
			}

			return result;
		}
	}
}
