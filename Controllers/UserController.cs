using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Data;
using RestaurantApp.DTOs;
using RestaurantApp.Models;
using RestaurantApp.Repository.IRepository;

namespace RestaurantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly IUserRepository _userRepository;

        public UserController(DataContextEf ef, IUserRepository userRepository)
        {
            _ef = ef;
            _userRepository = userRepository;
        }


        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            IEnumerable<User> userDb = _userRepository.GetAll();
            return Ok(userDb);
        }

        [HttpGet("GetSingleUser")]
        public IActionResult GetSingleUser(int userId)
        {
            User? userDb = _userRepository.Get(userId);
            return Ok(userDb);
        }

        [HttpPost("AddUsers")]
        public IActionResult AddUsers(UserDto userDto)
        {
            /*User? userDbEmailExists = _ef.Users.FirstOrDefault(u => u.Email == userDto.Email);*/
            User? userDbEmailExists = _userRepository.GetByEmail(userDto.Email);

            if (userDbEmailExists != null)
            {
                throw new Exception("User with the specified email already exists.");
            }

            User newUser = new User()
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber,
            };

            _userRepository.Add(newUser);
            _userRepository.Save();

            return Ok(newUser);
        }


        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser(UserDto userDto, int userId)
        {
            User? userDbExists = _userRepository.Get(userId);

            userDbExists.FirstName = userDto.FirstName;
            userDbExists.LastName = userDto.LastName;
            userDbExists.PhoneNumber = userDto.PhoneNumber;
            userDbExists.Email = userDto.Email;


            _userRepository.Update(userDbExists);
            _userRepository.Save();
            return Ok(userDbExists);
        }

        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser(int userId)
        {
            User? userDbExists = _userRepository.Get(userId);

            if (userDbExists != null)
            {
                _userRepository.Remove(userDbExists);
                _userRepository.Save();
                throw new Exception($"The user ${userDbExists} dosen`t exist");
            }
            return Ok(userDbExists);
        }
    }
}
