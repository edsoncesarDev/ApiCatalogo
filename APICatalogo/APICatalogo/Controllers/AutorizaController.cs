using APICatalogo.DTOs;
using APICatalogo.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"AutorizaController :: Acessado em : {DateTime.Now.ToLongDateString()}";
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDTO>> RegistraUsuario(UsuarioDTO model)
        {
            try
            {
                if (model is null)
                    return BadRequest("Registro Inválido");

                if (model.Password != model.ConfirmPassword)
                    return BadRequest("Registro Inválido");

                var validaEmail = Funcoes.IsValidEmail(model.Email);

                if (!validaEmail)
                    return BadRequest("Email Inválido");

                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(GeraToken(model));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UsuarioDTO userInfo)
        {
            try
            {
                if (userInfo is null)
                    return BadRequest("Usuário Inválido");

                var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
                //lockoutOnFailure: false - ao tentar mais de 3 vezes não bloqueia

                if (!result.Succeeded)
                    return BadRequest("Login Inválido");

                return Ok(GeraToken(userInfo));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }

        }

        private UsuarioToken GeraToken(UsuarioDTO userInfo)
        {
            //definindo declarações do usuário
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("meuPet", "flipper"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //gerando uma chave com base em um algoritmo simetrico
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            //gerando a assinatura digital do token usando o algoritmo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //tempo de expiração do token
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            //gerando token JWT
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credenciais
            );

            //retorna os dados com o token e informacoes
            return new UsuarioToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }

    }
}
