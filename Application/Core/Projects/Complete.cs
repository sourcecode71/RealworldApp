using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace Application.Core.Projects
{
    public class Complete
    {
        public class Command : IRequest<Unit>
        {
            public bool Invoiced { get; set; }
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

                if(project != null)
                {
                    project.Invoiced = request.Invoiced;
                    project.Status = ProjectStatus.Completed;

                    await _context.SaveChangesAsync();
                }
              
                return Unit.Value;
            }
        }
    }
}