﻿using Jynx.Api.Models.Responses;
using Jynx.Common.AspNetCore.ErrorHandling;
using Jynx.Common.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers
{
    [Route("[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        public BaseController(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }

        public IActionResult NotFound(string message)
            => NotFound(new GenericErrorModel
            {
                Message = message,
                RequestId = Request.HttpContext.GetRequestId()
            });

        public IActionResult BadRequest(string message)
            => BadRequest(new GenericErrorModel
            {
                Message = message,
                RequestId = Request.HttpContext.GetRequestId()
            });

        public IActionResult MultiError(IEnumerable<string> errors)
            => BadRequest(new MultiErrorResponse
            {
                Errors = errors,
                RequestId = Request.HttpContext.GetRequestId()
            });

        public IActionResult NullRequestError()
            => BadRequest("Empty or invalid body of request");
    }
}
