﻿using Movies.Application.Models;

namespace Movies.Application.Repositories;

public interface IMovieRepository
{
    Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Movie?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
