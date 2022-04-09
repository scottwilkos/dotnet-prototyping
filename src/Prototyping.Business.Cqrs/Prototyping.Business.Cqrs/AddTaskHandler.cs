using MediatR;
using Prototyping.Domain;
using Prototyping.Domain.Models;

namespace Prototyping.Business.Cqrs
{
    public class AddTaskHandler : IRequestHandler<AddTaskCommand, TaskItem>
    {
        private readonly TaskContext _context;

        public AddTaskHandler(TaskContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> Handle(AddTaskCommand command, CancellationToken cancellationToken)
        {
            TaskItem taskItem = new TaskItem
            {
                TaskBody = command.Taskbody,
            };

            await _context.TaskItems.AddAsync(taskItem, cancellationToken);

            return taskItem;
        }
    }
}