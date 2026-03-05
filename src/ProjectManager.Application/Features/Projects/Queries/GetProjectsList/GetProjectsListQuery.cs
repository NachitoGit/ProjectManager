using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Queries.GetProjectsList
{
    public class GetProjectsListQuery : IRequest<IEnumerable<ProjectListItemDto>>
    {
    }
}
