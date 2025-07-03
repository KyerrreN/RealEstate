namespace ChatService.API.Constants
{
    public class ApiConstants
    {
        public const string JsonContentType = "application/json";

        private const string Api = "api/";

        public const string RouteSaveMessage = Api + "messages";
        public const string RouteGetMessages = Api + "messages/realestate/{realEstateId:guid}";
        public const string RouteGetUserDialogs = Api + "dialogs";
    }
}
