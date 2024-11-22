using AutoMapper;
using CrudDapper.Dto;
using CrudDapper.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace CrudDapper.Services
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UsuarioService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [Obsolete]
        public async Task<ResponseModel<UsuarioListarDto>> BuscarUsuarioPorId(int usuarioId)
        {
            ResponseModel<UsuarioListarDto> response = new ResponseModel<UsuarioListarDto>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.QueryFirstOrDefaultAsync<Usuario>("select * from Usuarios where id = @Id", new { Id = usuarioId });

                if (usuarioBanco == null)
                {
                    response.Mensagen = "Nenhim usuário localizado";
                    response.Status = false;
                    return response;
                }

                var usuarioMapeado = _mapper.Map<UsuarioListarDto>(usuarioBanco);

                response.Dados = usuarioMapeado;
                response.Mensagen = "Usuário lcalizado com sucesso";
            }

            return response;
        }

        [Obsolete]
        public async Task<ResponseModel<List<UsuarioListarDto>>> BuscarUsuarios()
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var UsuariosBanco = await connection.QueryAsync<Usuario>("select * from Usuarios");

                if (UsuariosBanco.Count() == 0)
                {
                    response.Mensagen = "Nenhum usuário localizado";
                    response.Status = false;
                    return response;
                }

                //Transformacao em Mapper
                var usuarioMapeado = _mapper.Map<List<UsuarioListarDto>>(UsuariosBanco);

                response.Dados = usuarioMapeado;
                response.Mensagen = "Usário Localizados com sucesso";
            }

            return response;
        }

        [Obsolete]
        public async Task<ResponseModel<List<UsuarioListarDto>>> CriarUsuario(CriarUsuarioDto usuarioCriarDto)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var UsuarioBanco = await connection.ExecuteAsync("Insert into Usuarios (NomeCompleto, Email, Cargo, Salario, CPF, Senha, Situacao) values (@NomeCompleto, @Email, @Cargo, @Salario, @CPF, @Senha, @Situacao)", usuarioCriarDto);

                if (UsuarioBanco == 0)
                {
                    response.Mensagen = "Ocorreu um erro ao realizar um registro!";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados = usuariosMapeados;
                response.Mensagen = "usuários listados com sucesso!";
            }
            return response;
        }

        [Obsolete]
        private static async Task<IEnumerable<Usuario>> ListarUsuarios(SqlConnection connection)
        {
            return await connection.QueryAsync<Usuario>("select * from Usuarios");
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> EditarUsuario(UsuarioEditarDto usuarioEditarDto)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var UsuarioBanco = await connection.ExecuteAsync("update Usuarios set NomeCompleto = @NomeCompleto, Email = @Email, Cargo = @Cargo, Salario = @Salario, CPF = @CPF, Situacao = @Situacao WHERE Id = @Id", usuarioEditarDto);

                if (UsuarioBanco == 0)
                {
                    response.Mensagen = "Ocurreu um erro ao realizar a edição";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados = usuariosMapeados;
                response.Mensagen = "Usuários listados com sucesso";
            }
            return response;
        }

        [Obsolete]
        public async Task<ResponseModel<List<UsuarioListarDto>>> RemoverUsuario(int usuarioId)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var UsuariosBanco = await connection.ExecuteAsync("delete from Usuarios where id = @Id", new { Id = usuarioId });

                if (UsuariosBanco == 0)
                {
                    response.Mensagen = "Ocurreu um erro ao realizar ao remover o usuário";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados = usuariosMapeados;
                response.Mensagen = "Usuários lstados com sucesso";
            }
            return response;
        }
    }
}