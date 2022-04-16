using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Employees
{
    public class GetAllEmployees
    {
        public class Query : IRequest<List<Employee>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Employee>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Employee>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Employees.ToListAsync();
            }
        }
    }
}
