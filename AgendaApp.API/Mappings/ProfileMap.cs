using AgendaApp.API.Models;
using AgendaApp.Data.Entities;
using AutoMapper;

namespace AgendaApp.API.Mappings
{
    public class ProfileMap : Profile
    {
        public ProfileMap()
        {
            //CriarTarefaRequestModel => Tarefa
            CreateMap<CriarTarefaRequestModel, Tarefa>();


            //Tarefa => CriarTarefaResponseModel
            CreateMap<Tarefa, CriarTarefaResponseModel>();

            //Tarefa => EditarTarefaResponseModel
            CreateMap<Tarefa, EditarTarefaResponseModel>();

            //Tarefa => ExcluirTarefaResponseModel
            CreateMap<Tarefa, ExcluirTarefaResponseModel>();

            //Tarefa => ConsultarTarefaResponseModel
            CreateMap<Tarefa, ConsultarTarefaResponseModel>();
        }
    }
}
