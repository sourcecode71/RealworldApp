using Domain.Enums;

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
                case ProjectStatus.Invoiced:
                    return "Invoiced";
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
