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
    public class AddModifiedComment
    {
        public class Command : IRequest<Unit>
        {
            public int SelfProjectId { get; set; }
            public string Modified { get; set; }
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

                project.AdminModifiedComment = request.Modified;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return SystemException("Error adding modified comment!");
                
                return Unit.Value;
            }

            private Unit SystemException(string v)
            {
                throw new NotImplementedException();
            }
        }
    }
}