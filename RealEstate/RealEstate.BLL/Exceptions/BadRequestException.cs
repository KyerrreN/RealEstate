﻿namespace RealEstate.BLL.Exceptions
{
    public class BadRequestException(string message)
        : Exception(message)
    {
    }
}
