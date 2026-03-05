using MediatR;
using ProjectManager.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Queries.GetTasksList
{
    public class GetTasksListQuery : IRequest<PaginatedResponse<TaskItemListItemDto>>
    {
        public int ProjectId { get; set; }

        //Paginación
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        //Filtros
        public bool? IsCompleted { get; set; }
        public string? SearchTerm { get; set; }
    }
}
