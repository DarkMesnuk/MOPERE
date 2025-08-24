using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Base.API.Controllers;

[ApiController]
public abstract class BaseApiController(
    ILogger<BaseApiController> logger, 
    IMapper mapper, 
    IMediator mediator
) : ControllerBase
{
    protected readonly ILogger<BaseApiController> Logger = logger;
    protected readonly IMapper Mapper = mapper;
    protected readonly IMediator Mediator = mediator;
}