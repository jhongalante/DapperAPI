using API.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private DbSession _db;
        public TarefaRepository(DbSession dbSession)
        {
            _db = dbSession;
        }

        public async Task<List<Tarefa>> GetTarefasAsync()
        {
            using (var conn = _db.Connection)
            {
                //string query = "SELECT * FROM Tarefas";
                //List<Tarefa> tarefas = (await conn.QueryAsync<Tarefa>(sql: query)).ToList();
                var tarefas = (await conn.GetAllAsync<Tarefa>()).ToList();
                return tarefas;
            }
        }

        public async Task<Tarefa> GetTarefaByIdAsync(int id)
        {
            using (var conn = _db.Connection)
            {
                //string query = "SELECT * FROM Tarefas WHERE Id = @id";
                //Tarefa tarefa = await conn.QueryFirstOrDefaultAsync<Tarefa>
                //    (sql: query, param: new { id });

                var tarefa = await conn.GetAsync<Tarefa>(id);
                return tarefa;
            }
        }
        public async Task<TarefaContainer> GetTarefasEContadorAsync()
        {
            using (var conn = _db.Connection)
            {
                string query =
                    @"SELECT COUNT(*) FROM Tarefas
    				SELECT * FROM Tarefas";

                var reader = await conn.QueryMultipleAsync(sql: query);

                return new TarefaContainer
                {
                    Contador = (await reader.ReadAsync<int>()).FirstOrDefault(),
                    Tarefas = (await reader.ReadAsync<Tarefa>()).ToList()
                };
            }
        }
        public async Task<int> SaveAsync(Tarefa novaTarefa)
        {
            using (var conn = _db.Connection)
            {
                //        string command = @"
                //INSERT INTO Tarefas(Descricao, IsCompleta)
                //VALUES(@Descricao, @IsCompleta)";

                //        var result = await conn.ExecuteAsync(sql: command, param: novaTarefa);
                var result = await conn.InsertAsync(novaTarefa);
                return result;
            }
        }
        public async Task<bool> UpdateTarefaStatusAsync(Tarefa atualizaTarefa)
        {
            using (var conn = _db.Connection)
            {
                //     string command = @"
                //UPDATE Tarefas SET IsCompleta = @IsCompleta WHERE Id = @Id";

                //     var result = await conn.ExecuteAsync(sql: command, param: atualizaTarefa);
                var result = await conn.UpdateAsync(atualizaTarefa);
                return result;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            using (var conn = _db.Connection)
            {
                //string command = @"DELETE FROM Tarefas WHERE Id = @id";
                //var resultado = await conn.ExecuteAsync(sql: command, param: new { id });
                var resultado = await conn.DeleteAsync(new Tarefa { Id = id });
                return resultado;
            }
        }
    }
}
