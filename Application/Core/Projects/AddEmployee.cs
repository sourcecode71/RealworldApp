using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace Application.Core.Projects
{
    public class AddEmployee
    {
        public class Command : IRequest<Unit>
        {
            public int SelfProjectId { get; set; }
            public string EmployeeEmail { get; set; }
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
                var project = await _context.Projects.Include(e => e.Employees).Include(a => a.Activities).FirstOrDefaultAsync(x => x.SelfProjectId == request.SelfProjectId);

                Employee employee = _context.Employees.FirstOrDefault(x => x.Email == request.EmployeeEmail);

                if (employee == null) return SystemException("Error finding employee in database!!");

                project.Employees.Add(employee);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return SystemException("Error adding employee!");

                return Unit.Value;
            }

            private Unit SystemException(string v)
            {
                throw new System.NotImplementedException(v);
            }
        }
    }
}