using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticWeb.Data
{
    public class Session : ComponentBase
    {
        public readonly string SS_TOKEN = "SS_TOKEN";

        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Token
        {
            get
            {
                try
                {
                    return _httpContextAccessor.HttpContext.Session.GetString(SS_TOKEN);
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                _httpContextAccessor.HttpContext.Session.SetString(SS_TOKEN, value);
            }
        }
    }
}
