﻿using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace Vts.Api.Security
{
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
        public IReadOnlyList<string> ApiKeys { get; set; }

        public ApiKeyRequirement(IEnumerable<string> apiKeys)
        {
            ApiKeys = apiKeys?.ToList() ?? new List<string>();
        }
    }
}
