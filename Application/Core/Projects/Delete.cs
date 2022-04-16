using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Projects
{
    public class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
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
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id);

                _context.Remove(project);

                await _context.SaveChangesAsync();

                return Unit.Value;

            }
        }
    }
}