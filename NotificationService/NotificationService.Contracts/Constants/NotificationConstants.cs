﻿namespace NotificationService.Contracts.Constants
{
    public class NotificationConstants
    {
        public const string Exchange = "notification-exchange";
        public const string ExchangeType = "direct";

        public const string UserQueue = "user-queue";
        public const string RealEstateQueue = "real-estate-queue";
        public const string ReviewQueue = "review-queue";

        public const string UserRoutingKey = "user";
        public const string RealEstateAddedRoutingKey = "real-estate-added";
        public const string RealEstateDeletedRoutingKey = "real-estate-deleted";
        public const string ReviewAddedRoutingKey = "review-added";
    }
}
