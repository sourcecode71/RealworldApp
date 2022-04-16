using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Employees
{
    public class DeleteEmployee
    {
        public class Command : IRequest
        {
            public string Email { get; set; }
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
                Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Email == request.Email);

                _context.Remove(employee);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) throw new System.Exception("Problem deleting employee!");

                return Unit.Value;
            }
        }
    }
}