using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace Application.Core.Employees
{
    public class ListEmployees
    {
        public class Query : IRequest<List<EmployeeDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<EmployeeDto>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<EmployeeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var employeesDto = new List<EmployeeDto>();
                var employees = await _context.Employees.ToListAsync();
                foreach (var item in employees)
                {
                    var itemDto = new EmployeeDto
                    {
                        Name = item.Name,
                        Email = item.Email
                    };
                    employeesDto.Add(itemDto);
                }
                return employeesDto;
            }
        }
    }
}