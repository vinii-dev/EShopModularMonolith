﻿using MediatR;

namespace Shared.CQRS;

public interface ICommand : IRequest<Unit>
{

}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    
}
