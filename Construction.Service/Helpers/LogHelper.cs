namespace Construction.Service.Helpers
{
    public static class LogHelper
    {
        public static string SuccessRemove(string entityName, int entityId) => $"{entityName} with id:{entityId}, successfully deleted";
        public static string SuccessUpdate(string entityName, int entityId) => $"{entityName} with id:{entityId}, successfully updated";
        public static string NotFoundMessage(string entity, int entityId) => $"Unable to find {entity} with id:{entityId}";
        public static string BadRequest(string entity, int entityId) => $"Something went wrong. {entity} with id:{entityId}";

        public static string SuccessGet(string entityNames) => $"Successful receipt of {entityNames}";
        public static string BadGet(string entityNames) => $"Failed to receive {entityNames}";
    }
}
