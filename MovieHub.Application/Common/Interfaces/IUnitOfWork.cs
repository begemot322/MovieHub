﻿using MovieHub.Application.Common.Interfaces.Repositories;

namespace MovieHub.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IMovieRepository Movies { get; }
    IActorRepository Actors { get; }
    IUserRepository Users { get; }
    
    Task<int> SaveChangesAsync();
}