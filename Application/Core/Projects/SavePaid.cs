using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Projects
{
    public class SavePaid
    {
        public class Command : IRequest<Unit>
        {
            public double Paid { get; set; }
            public int SelfProjectId { get; set; }
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
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.SelfProjectId == request.SelfProjectId);

                if (project != null)
                {
                    project.Paid = request.Paid;

                    await _context.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}