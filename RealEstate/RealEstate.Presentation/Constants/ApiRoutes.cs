namespace RealEstate.Presentation.Constants
{
    public class ApiRoutes
    {
        private const string Api = "api";
        public const string UsersEndpoint = $"{Api}/users";
        public const string RealEstateEndpoint = $"{Api}/estates";
        public const string HistoryEndpoint = Api + "/users/{userId:guid}/history";
    }
}
