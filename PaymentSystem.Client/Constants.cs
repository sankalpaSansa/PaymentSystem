using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Client
{
    public class Constants
    {
        public const string BASE_URL = "https://localhost:5001/";
        public const string CONTENT_TYPE = "application/json";
        public const string LOGIN_URL = "api/Login/Authenticate/authenticate?UserName=";
        public const string BALANCE_URL = "api/Pay/";
        public const string TOPUP_URL = "api/Topup";
        public const string TRANSFER_URL = "api/Pay/Transfer";
    }
}
