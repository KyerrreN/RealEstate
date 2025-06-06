﻿namespace NotificationService.Consumers.Constants
{
    public class TemplateConstants
    {
        private const string BasePath = "Templates/";

        public const string UserRegistered = BasePath + "UserRegistered.cshtml";
        public const string RealEstateAdded = BasePath + "RealEstateAdded.cshtml";
        public const string RealEstateDeleted = BasePath + "RealEstateDeleted.cshtml";
        public const string ReviewAdded = BasePath + "ReviewAdded.cshtml";
    }
}
