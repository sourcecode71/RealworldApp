using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Model
{
    public static class EnumConverter
    {
       

        public static string ProjectStatusString(ProjectStatus status)
        {
            switch (status)
            {
                case ProjectStatus.Budgeted:
                    return "Budget phase";
                case ProjectStatus.Active:
                    return "Active";
                case ProjectStatus.Completed:
                    return "Completed";
                case ProjectStatus.Archived:
                    return "Archived";
                case ProjectStatus.Delayed:
                    return "Delayed";
                case ProjectStatus.Modified:
                    return "Modified";
                case ProjectStatus.Canceled:
                    return "Cancel";
                default:
                    return "On Time";
            }
        }
    }
}
