using System;

namespace WebAPI.Models
{
    public class FailedChargeResponse : ChargeResponse
    {
        public FailedChargeResponse(Exception exception)
        {
            Error = exception;
        }
    }
}