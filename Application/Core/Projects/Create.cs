using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Persistance.Context;

namespace Application.Core.Projects
{
    public class Create
    {
        public class Command : IRequest<Unit>
        {
            public Project Project { get; set; }
            public List<string> Employees { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Projects.Add(request.Project);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return SystemException("Greska!");

                var project = _context.Projects.Include(p => p.Employees).FirstOrDefault(x => x.Id == request.Project.Id);

                if (project == null) return SystemException("Greska pri nalazenju projekta iz baze!");

                if (request.Employees.Count != 0)
                {
                    foreach (var employee in request.Employees)
                    {
                        Employee emp = _context.Employees.FirstOrDefault(x => x.Email == employee);

                        if (emp == null) return SystemException("Greska pri nalazenju zaposlenog iz baze!");

                        project.Employees.Add(emp);
                    }
                    result = await _context.SaveChangesAsync() > 0;
                }

                if (!result) return SystemException("Greska pri dodavanju zaposlenih!");

                return Unit.Value;

            }

            private Unit SystemException(string v)
            {
                throw new NotImplementedException(v);
            }
        }
    }
}