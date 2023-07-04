﻿using Jynx.Common.Entities;

namespace Jynx.Api.Models.Responses
{
    public class ReadUserResponse
    {
        public ReadUserResponse(User user)
        {
            Username = user.Username;
            Created = user.Created ?? DateTime.MinValue;
        }

        public string Username { get; set; } = "";

        public DateTime Created { get; set; }
    }
}
