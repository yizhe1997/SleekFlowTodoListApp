﻿using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace SleekFlowTodoListAPI.Controllers.ControllerTypes
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiSecureController : ApiBaseController
    {
        public ApiSecureController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }
    }
}
