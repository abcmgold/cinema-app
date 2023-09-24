using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Quanlyrapchieuphim.Dtos;
using Quanlyrapchieuphim.Models;
using StackExchange.Redis;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using static Dapper.SqlMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quanlyrapchieuphim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController<Account, AccountUpdateDto, Account>
    {
        public AccountController(IConfiguration configuration) : base(configuration)
        {
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);

            await mySqlConnection.OpenAsync();

            var parameters = new DynamicParameters();

            parameters.Add("p_Username", username);

            var account = await mySqlConnection.QueryFirstOrDefaultAsync<Account>(
                        "Proc_Account_GetByUsername",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

            if (account != null && account.Password == password)
            {
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes
                (_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("UserId", account.UserId.ToString()),
                        new Claim("AccountId", account.AccountId.ToString()),
                        new Claim(ClaimTypes.Role, account.Role),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                // Đăng nhập thành công
                return Ok(new { jwtToken, account.Username });
            }
            else
            {
                // Đăng nhập thất bại
                return BadRequest("Thông tin đăng nhập không hợp lệ");
            }
        }
        [HttpGet]
        [Route("GetInfo")]
        public async Task<User> GetInfo(Guid accountId)
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            var cacheDb = redis.GetDatabase();
            var value = cacheDb.StringGet("accountInfo").ToString();

            if (!value.IsNullOrEmpty())
            {
                return JsonSerializer.Deserialize<User>(value);
            }

            else
            {
                MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);

                await mySqlConnection.OpenAsync();
                DynamicParameters? parameters = new DynamicParameters();
                parameters.Add("@accountId", accountId);
                var sql = "Proc_Account_GetInfo";
                var result = await mySqlConnection.QueryFirstOrDefaultAsync<User>(sql, parameters, commandType: CommandType.StoredProcedure);

                await mySqlConnection.CloseAsync();

                cacheDb.StringSet("accountInfo", JsonSerializer.Serialize(result));

                return result;
            }


        }
        [HttpPost]
        [Route("Signup")]
        public async Task<ActionResult> Signup(AccountInsertDto account)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(_connectionString))
            {
                await mySqlConnection.OpenAsync();

                using (var transaction = await mySqlConnection.BeginTransactionAsync())
                {
                    try
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@userName", account.UserName);
                        parameters.Add("@email", account.Email);
                        parameters.Add("@phoneNumber", account.PhoneNumber);
                        var sql = "Proc_Account_CheckInfo";
                        var result = await mySqlConnection.QueryFirstOrDefaultAsync<AccountInsertDto>(sql, parameters, transaction, commandType: CommandType.StoredProcedure);

                        if (result != null)
                        {
                            if (result.Email == account.Email)
                            {
                                return BadRequest("Email đã được đăng ký");
                            }
                            else if (result.PhoneNumber == account.PhoneNumber)
                            {
                                return BadRequest("Số điện thoại đã được đăng ký");
                            }
                            else if (result.UserName == account.UserName)
                            {
                                return BadRequest("Tên tài khoản đã tồn tại");
                            }
                        }
                        else
                        {
                            var accountId = Guid.NewGuid();
                            var userId = Guid.NewGuid();
                            parameters.Add("@p_AccountId", accountId);
                            parameters.Add("@p_UserName", account.UserName);
                            parameters.Add("@p_Password", account.Password);

                            if (account.Role == null)
                            {
                                parameters.Add("@p_Role", value: "User");
                            }
                            else
                            {
                                parameters.Add("@p_Role", value: account.Role);

                            }
                            parameters.Add("@p_UserId", userId);
                            parameters.Add("@p_FullName", account.FullName);
                            parameters.Add("@p_Image", account.Image);
                            parameters.Add("@p_Email", account.Email);
                            parameters.Add("@p_PhoneNumber", account.PhoneNumber);

                            await mySqlConnection.ExecuteAsync("Proc_Account_AddNew", parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
                            await mySqlConnection.ExecuteAsync("Proc_User_AddNew", parameters, transaction: transaction, commandType: CommandType.StoredProcedure);

                        }

                        await transaction.CommitAsync();

                        return Ok("Đăng ký thành công");
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest("Đăng ký thất bại");
                    }
                }
            }
        }

    }
}
