using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Models;
using static Api.Services.JwtService;
using Microsoft.Extensions.Configuration;

namespace Api.Services
{
	public class JwtService
	{
		private readonly IConfiguration _configuration;
		private readonly IRolService _rolService;

		public JwtService(IConfiguration configuration, IRolService rolService)
		{
			_configuration = configuration;
			_rolService = rolService;
		}

			public string GenerateToken(Usuario user, string rol)
			{
				List<Claim> claims = new List<Claim> {
					new Claim(ClaimTypes.Name, user.NombreUsuario),
					new Claim(ClaimTypes.Role, rol)
				};

			string? clave = _configuration["AppSettings:Token"];
			if (string.IsNullOrEmpty(clave))
			{
				throw new InvalidOperationException("La clave del token no está configurada en appsettings.json");
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));

				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

				var token = new JwtSecurityToken(
						claims: claims,
						expires: DateTime.Now.AddDays(1),
						signingCredentials: creds
					);

				var jwt = new JwtSecurityTokenHandler().WriteToken(token);

				return jwt;
			}
		}

		//public string GenerateToken(string role)
		//{
		//	// Definir las reclamaciones (claims) basadas en el rol
		//	var claims = new[]
		//	{
		//		new Claim(ClaimTypes.Role, role),
		//		new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Identificador único para el token
		//	};

		//	// Crear la clave de seguridad utilizando la clave secreta definida en _jwtSettings
		//	var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

		//	// Definir las credenciales de firma usando HMAC y SHA256
		//	var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		//	// Crear el descriptor del token, con información como el emisor, audiencia, expiración y las credenciales de firma
		//	var tokenDescriptor = new SecurityTokenDescriptor
		//	{
		//		Subject = new ClaimsIdentity(claims),
		//		Expires = DateTime.UtcNow.AddMinutes(30), // Expiración del token a los 30 minutos
		//		SigningCredentials = creds,
		//		Issuer = _jwtSettings.Issuer,
		//		Audience = _jwtSettings.Audience
		//	};

		//	// Generar el token usando el descriptor
		//	var tokenHandler = new JwtSecurityTokenHandler();
		//	var token = tokenHandler.CreateToken(tokenDescriptor);

		//	// Retornar el token generado
		//	return tokenHandler.WriteToken(token);
		//}

		//public string GenerateRefreshToken()
		//{
		//	return Guid.NewGuid().ToString().Replace("-", "");
		//}

		//public (int? userId, string role)? ValidateJwtToken(string token)
		//{
		//	if (string.IsNullOrWhiteSpace(token))
		//		return null;

		//	var tokenHandler = new JwtSecurityTokenHandler();
		//	var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey); // Clave secreta del JWTSettings

		//	try
		//	{
		//		// Parámetros para validar el token
		//		tokenHandler.ValidateToken(token, new TokenValidationParameters
		//		{
		//			ValidateIssuerSigningKey = true,
		//			IssuerSigningKey = new SymmetricSecurityKey(key), // Usa la clave secreta para verificar la firma del token
		//			ValidateIssuer = false, // Si deseas validar el emisor, cambia a true y proporciona el valor correcto
		//			ValidateAudience = false, // Si deseas validar la audiencia, cambia a true y proporciona el valor correcto
		//			ClockSkew = TimeSpan.Zero // Esto hace que el token expire exactamente en su tiempo de expiración
		//		}, out SecurityToken validatedToken);

		//		// Convertir el token a JwtSecurityToken para poder extraer los claims
		//		var jwtToken = (JwtSecurityToken)validatedToken;

		//		// Extraer el userId del claim 'id'
		//		var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

		//		// Extraer el rol del claim 'role'
		//		var role = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

		//		// Retornar userId y role si la validación es exitosa
		//		return (userId, role);
		//	}
		//	catch
		//	{
		//		// Retornar null si la validación falla
		//		return null;
		//	}
		//}
}