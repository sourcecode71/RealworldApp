using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace Application.Core.Projects
{
    public class AddDelayedComment
    {
        public class Command : IRequest<Unit>
        {
            public int SelfProjectId { get; set; }
            public string Delayed { get; set; }
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
                var project = await _context.Projects.Include(a => a.Activities).FirstOrDefaultAsync(x => x.SelfProjectId == request.SelfProjectId);

                if (project != null)
                {
                    project.AdminDelayedComment = request.Delayed;

                    var result = await _context.SaveChangesAsync() > 0;

                    if (!result) return SystemException("Error adding delayed comment!");
                }

                return Unit.Value;
            }

            private Unit SystemException(string v)
            {
                throw new NotImplementedException();
            }
        }
    }
}