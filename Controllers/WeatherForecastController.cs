using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supabase;
using Supabase.Interfaces;
using WebApplication7;

namespace WebApplication7.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly SupaBaseContext _supabaseContext;

        public WeatherForecastController(Supabase.Client supabaseClient, SupaBaseContext supabaseContext)
        {
            _supabaseClient = supabaseClient;
            _supabaseContext = supabaseContext;
        }

        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        public async Task<string> GetAllUsers()
        {
            var result = await _supabaseContext.GetUsers(_supabaseClient);
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        [HttpPost("InsertUsers", Name = "InsertUsers")]
        public async Task<ActionResult> InsertUser([FromBody] UserData userData)
        {
            if (string.IsNullOrEmpty(userData.Login) || string.IsNullOrEmpty(userData.Password))
            {
                return BadRequest("��� ����� ��� ������ ������");
            }

            User newUser = new User
            {
                Id = userData.Id,
                Login = userData.Login,
                Password = userData.Password,
                Name = userData.NewName
            };

            bool result = await _supabaseContext.InsertUsers(_supabaseClient, newUser);
            if (result)
            {
                return Ok("����������� ������ �������");
            }
            else
            {
                return BadRequest("�� ������� �������� ������������ � ��");
            }
        }

        [HttpPut("UpdateUserName", Name = "UpdateUserName")]
        public async Task<ActionResult> UpdateUserName([FromBody] UserData userData)
        {
            try
            {
                if (userData.Id <= 0 || string.IsNullOrEmpty(userData.NewName))
                {
                    return BadRequest("������������� ������������ ������ ���� �������������, � ����� ��� �� ������ ���� ������.");
                }

                User updatedUser = new User
                {
                    Id = userData.Id,
                    Login = userData.Login,
                    Password = userData.Password,
                    Name = userData.NewName 
                };

                bool result = await _supabaseContext.UpdateUserName(_supabaseClient,updatedUser.Id, updatedUser.Name);

                if (result)
                {
                    return Ok("��� ������������ ������� ���������");
                }
                else
                {
                    return BadRequest("�� ������� �������� ��� ������������ � ��");
                }
            }
            catch (Exception)
            {
                return BadRequest("����������� ������");
            }
        }
    }

    public class UserData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("newName")]
        public string NewName { get; set; }
    }

}