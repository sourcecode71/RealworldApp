using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Projects
{
    public class GetById
    {
         public class Query : IRequest<Project>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Project>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            _context = context;
            }

            public async Task<Project> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id);
            }
        }
    }
}