using AutoMapper;
using MediatR;
using TaskManager.Application.Commons.Bases;
using TaskManager.Domain.DTOs;
using TaskManager.Infra.Interfaces.Repositories;

namespace TaskManager.Application.UseCases.Task.v1.GetAllTasks
{
    public class GetAllTasksHandler : IRequestHandler<GetAllTasksQuery, BaseResponse<GetAllTaskskResult>>
    {
        private readonly IMapper _mapper;
        private readonly ITasksRepository _tasksRepository;

        public GetAllTasksHandler(IMapper mapper, ITasksRepository tasksRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _tasksRepository = tasksRepository ?? throw new ArgumentNullException(nameof(tasksRepository));
        }

        public async Task<BaseResponse<GetAllTaskskResult>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetAllTaskskResult>() { Succcess = true };

            try
            {
                var result = await _tasksRepository.GetAllAsync(default);
                response.Data = _mapper.Map<GetAllTaskskResult>(Adapter(result.ToList()));
                response.Message = "Get successfully!";
            }
            catch (Exception ex)
            {
                response.Succcess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private GetAllTaskskResult Adapter(List<Domain.Entities.Tasks> entity)
        {
            var listTasks = new List<TaskDTO>();
            foreach (var item in entity)
            {
                var task = new TaskDTO
                { 
                                TaskId = item.Id, 
                                ProjectId = item.ProjectId, 
                                Name = item.Name, 
                                Priority = item.Priority, 
                                Status = item.Status, 
                                Comments = item.Comments
                            };
                listTasks.Add(task);
            }

            return new GetAllTaskskResult() { Tasks = listTasks };
        }
    }
}
