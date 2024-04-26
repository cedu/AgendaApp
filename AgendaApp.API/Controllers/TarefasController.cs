using AgendaApp.API.Models;
using AgendaApp.Data.Entities;
using AgendaApp.Data.Entities.Enums;
using AgendaApp.Data.Repositories;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace AgendaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly IMapper _mapper;

        public TarefasController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Serviço da API para cadastro de tarefas
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CriarTarefaResponseModel), 201)]
        public IActionResult Post(CriarTarefaRequestModel model)
        {
            try
            {
                //capturando os dados da tarefa
                var tarefa = _mapper.Map<Tarefa>(model);

                //complementando as informações da entidade 'tarefa'
                tarefa.Id = Guid.NewGuid();
                tarefa.DataHoraCadastro = DateTime.Now;
                tarefa.DataHoraUltimaAtualizacao = DateTime.Now;
                tarefa.Status = 1;

                //gravando no banco de dados
                var tarefaRepository = new TarefaRepository();
                tarefaRepository.Add(tarefa);

                //retornar a consulta da tarefa
                var response = _mapper.Map<CriarTarefaResponseModel>(tarefa);

                return StatusCode(201, response);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }

        /// <summary>
        /// Serviço da API para edição de tarefas
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(EditarTarefaResponseModel), 200)]
        public IActionResult Put(EditarTarefaRequestModel model)
        {
            try
            {
                var tarefaRepository = new TarefaRepository();
                var tarefa = tarefaRepository.GetById(model.Id.Value);

                //verificando se a tarefa foi encontrado
                if (tarefa != null)
                {
                    tarefa.Nome = model.Nome;
                    tarefa.Descricao = model.Descricao;
                    tarefa.DataHora = model.DataHora;
                    tarefa.Prioridade = (PrioridadeTarefa)model.Prioridade;
                    tarefa.DataHoraUltimaAtualizacao = DateTime.Now;

                    ///atualizar no banco de dados
                    tarefaRepository.Update(tarefa);

                    var response = _mapper.Map<EditarTarefaResponseModel>(tarefa);

                    return StatusCode(200, response);
                }
                else
                {
                    return StatusCode(400, new { message = "O Id da tarefa é inválido" }); //Erro BADREQUEST
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }

        /// <summary>
        /// Serviço da API para exclusão de tarefas
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ExcluirTarefaResponseModel), 200)]
        public IActionResult Delete(Guid? id)
        {
            try
            {
                var tarefaRepository = new TarefaRepository();
                var tarefa = tarefaRepository.GetById(id.Value);

                if (tarefa != null)
                {
                    tarefaRepository.Delete(tarefa);

                    var response = _mapper.Map<ExcluirTarefaResponseModel>(tarefa);
                    response.DataHoraExclusao = DateTime.Now;

                    return StatusCode(200, response);
                }
                else
                {
                    return StatusCode(400, new { message = "O Id da tarefa é inválido" }); //Erro BADREQUEST
                }

            }
            catch (Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }

        /// <summary>
        /// Serviço da API para consulta de tarefas
        /// </summary>
        [HttpGet("{dataInicio}/{dataFim}")]
        [ProducesResponseType(typeof(List<ConsultarTarefaResponseModel>), 200)]
        public IActionResult Get(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var tarefaRepository = new TarefaRepository();
                var tarefa = tarefaRepository.Get(dataInicio, dataFim);

                var response = _mapper.Map<List<ConsultarTarefaResponseModel>>(tarefa);
                return StatusCode(200, response);
            }
            catch(Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }

        /// <summary>
        /// Serviço da API para consultar 1 tarefa baseado no ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ConsultarTarefaResponseModel), 200)]
        public IActionResult GetById(Guid? id)
        {
            try
            {
                var tarefaRepository = new TarefaRepository();
                var tarefa = tarefaRepository.GetById(id.Value);

                if (tarefa != null)
                {
                    var response = _mapper.Map<ConsultarTarefaResponseModel>(tarefa);
                    return StatusCode(200, response);
                }
                else
                {
                    return StatusCode(204);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { e.Message });

            }
        }
    }
}
