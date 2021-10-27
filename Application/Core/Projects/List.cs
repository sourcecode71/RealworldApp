using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Persistance.Context;

namespace Application.Core.Projects
{
    public class List
    {
        public class Query : IRequest<List<ProjectDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<ProjectDto>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<ProjectDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var projectsDto = new List<ProjectDto>();
                var projects = await _context.Projects.ToListAsync();
                foreach (var item in projects)
                {
                    var itemDto = new ProjectDto
                    {
                        Name = item.Name,
                        Description = item.Description,
                        SelfProjectId = item.SelfProjectId
                    };
                    projectsDto.Add(itemDto);
                }
                return projectsDto;
            }
        }
    }
}