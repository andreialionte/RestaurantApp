using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestaurantApp.Data;
using RestaurantApp.DTOs;
using RestaurantApp.Models;
using RestaurantApp.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantApp.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly IConfiguration _config;
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        public AuthController(DataContextEf ef, IConfiguration config, IAuthRepository authRepository, IUserRepository userRepository)
        {
            _ef = ef;
            _config = config;
            _authRepository = authRepository;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            /*            User? userDb = _ef.Users.Where(u => u.Email == registerDto.Email).FirstOrDefault();*/

            byte[] passwordSalt = new byte[128 / 8];
            if (registerDto == null || registerDto.Password == null)
            {
                return BadRequest("Invalid registration data.");
            }


            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(passwordSalt);
            }

            string passwordHash = PasswordHasher(passwordSalt, registerDto.Password);

            Auth newUserAuth = new Auth();
            newUserAuth.Email = registerDto.Email;
            newUserAuth.PasswordHash = Encoding.UTF8.GetBytes(passwordHash);
            newUserAuth.PasswordSalt = passwordSalt;

            User newUser = new User()
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,

            };

            _authRepository.Add(newUserAuth);
            _userRepository.Add(newUser);
            _authRepository.Save();


            return Ok(newUser);

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            Auth? userAuth = _authRepository.UserExists(loginDto.Email);

            if (userAuth == null)
            {
                throw new Exception("User dosent exist");
            }
            /*
                        byte[] passwordSalt = new byte[128 / 8];

                        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                        {
                            rng.GetBytes(passwordSalt);
                        }*/
            byte[] passwordHashUser = userAuth.PasswordHash;
            string passwordHash = PasswordHasher(userAuth.PasswordSalt, loginDto.Password);
            byte[] byteasswordHash = Encoding.UTF8.GetBytes(passwordHash);

            //
            if (!passwordHashUser.SequenceEqual(byteasswordHash))
            {
                return Unauthorized("Password Incorrect");
            }
            //
            var user = _authRepository.SelectUserId(loginDto.Email);

            var token = CreateToken(user);

            return Ok(new { Message = "Login succesful", Token = $"{token}" });
        }

        private string PasswordHasher(byte[] saltPassword, string password)
        {
            var hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltPassword,
                iterationCount: 100000,
                prf: KeyDerivationPrf.HMACSHA512,
                numBytesRequested: 256 / 8
                );

            return Convert.ToBase64String(hash);
        }

        private string CreateToken(int userId)
        {
            Claim[] claims = new Claim[]{
                new Claim("userId", userId.ToString())
            };

            string? tokenKey = _config.GetSection("TokenKey").Value;

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var writeToken = tokenHandler.WriteToken(token);
            return writeToken;
        }
    }
}
