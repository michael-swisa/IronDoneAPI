﻿using Microsoft.AspNetCore.Mvc;

namespace IronDoneAPI.Utils
{
    public static class HttpUtils
    {
        public static object Response(int status, object message)
        {
            bool success = status >= 200 && status < 300;
            return new { success = success, message = message };
        }
    }
}
