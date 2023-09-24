using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Quanlyrapchieuphim.Controllers
{
    public abstract class BaseController<TEntity, TEntityUpdateDto, TEntityInsertDto> : ControllerBase
    {
        protected readonly string? _connectionString;
        protected readonly IConfiguration? _configuration;
        public BaseController(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings"];
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<List<TEntity>> GetAsync()
        {
            var tableName = typeof(TEntity).Name;

            MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);

            await mySqlConnection.OpenAsync();

            var result = await mySqlConnection.QueryAsync<TEntity>($"CALL Proc_{tableName}_GetAll()");

            await mySqlConnection.CloseAsync();

            return (List<TEntity>)result;
        }

        [HttpGet]
        [Route("FilterByName")]
        public async Task<List<TEntity>> GetFilterAsync(string? name)
        {
            var tableName = typeof(TEntity).Name;
            var parameters = new DynamicParameters();
            parameters.Add("p_Name", name);

            using (MySqlConnection mySqlConnection = new MySqlConnection(_connectionString))
            {
                await mySqlConnection.OpenAsync();
                var result = await mySqlConnection.QueryAsync<TEntity>($"CALL Proc_{tableName}_Filter(@p_Name)", parameters);
                return (List<TEntity>)result;
            }
        }

        [HttpGet(template: "{id}")]
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            var tableName = typeof(TEntity).Name;

            MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);

            await mySqlConnection.OpenAsync();

            var parameters = new DynamicParameters();

            parameters.Add("id", id);

            var result = await mySqlConnection.QueryFirstOrDefaultAsync<TEntity>(sql: $"CALL Proc_{tableName}_GetById(@id)", parameters);

            await mySqlConnection.CloseAsync();

            return result;
        }
        [HttpDelete]
        public async Task<int> DeleteMultiAsync([FromBody] List<string> ids)
        {
            var tableName = typeof(TEntity).Name;

            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {
                await mySqlConnection.OpenAsync();

                using (var transaction = await mySqlConnection.BeginTransactionAsync())
                {
                    try
                    {
                        var idList = string.Join(",", ids.Select(x => "\"" + x + "\""));

                        var parameters = new DynamicParameters();
                        parameters.Add("idList", idList);

                        var res = await mySqlConnection.ExecuteAsync(sql: $"CALL Proc_{tableName}_MultiDelete(@idList)", parameters, transaction);

                        await transaction.CommitAsync();

                        return res;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        [HttpPut("{id}")]

        public async Task<int> UpdateAsync(Guid id, TEntityUpdateDto entity)
        {
            using (MySqlConnection? connection = new MySqlConnection(_connectionString))
            {
                var name = typeof(TEntity).Name;
                var parameters = new DynamicParameters();
                parameters.Add($"@p_{name}Id", id);
                foreach (var prop in entity.GetType().GetProperties())
                {
                    if (string.Equals(prop.Name, $"{name}Id"))
                    {
                        continue;
                    }
                    else
                    {
                        parameters.Add("@p_" + prop.Name, prop.GetValue(entity, null));
                    }
                }

                string storedProcedureName = $"Proc_{name}_Update";

                int result = await connection.ExecuteAsync(sql: storedProcedureName, parameters, commandType: CommandType.StoredProcedure);

                return result;
            }
        }
        [HttpPost]
        public virtual async Task<ActionResult> InsertAsync(TEntityInsertDto entity)
        {
            using (var connection = new MySqlConnection(connectionString: _connectionString))
            {
                var name = typeof(TEntity).Name;
                var parameters = new DynamicParameters();
                var guid = Guid.NewGuid();

                foreach (var prop in entity.GetType().GetProperties())
                {
                    if (string.Equals(prop.Name, $"{name}Id"))
                    {
                        parameters.Add($"@p_{name}Id", guid);
                    }
                    else
                    {
                        parameters.Add("@p_" + prop.Name, prop.GetValue(entity, null));
                    }
                }

                string storedProcedureName = $"Proc_{name}_AddNew";

                int result = await connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);

                return Ok(result);
            }
        }
        [HttpDelete("{id}")]
        public async Task<int> DeleteAsync(string id)
        {
            using (MySqlConnection? connection = new MySqlConnection(_connectionString))
            {
                var tableName = typeof(TEntity).Name;

                var parameters = new DynamicParameters();

                parameters.Add($"p_{tableName}Id", id);
                var sql = $"CALL Proc_{tableName}_Delete(@p_{tableName}Id)";
                int res = await connection.ExecuteAsync(sql: sql, param: parameters);

                return res;
            }
        }
    }
}
