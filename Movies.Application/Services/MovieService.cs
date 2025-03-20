using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _movieValidator;

    public MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator)
    {
        _movieRepository = movieRepository;
        _movieValidator = movieValidator;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken)
    {
        await _movieValidator.ValidateAndThrowAsync(movie);
        return await _movieRepository.CreateAsync(movie);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _movieRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _movieRepository.GetAllAsync();
    }

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _movieRepository.GetByIdAsync(id);
    }

    public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await _movieRepository.GetBySlugAsync(slug);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken cancellationToken)
    {
        await _movieValidator.ValidateAndThrowAsync(movie);
        var movieExists = await _movieRepository.ExistByIdAsync(movie.Id);
        if (!movieExists)
        {
            return null;
        }

        await _movieRepository.UpdateAsync(movie);

        return movie;
    }
}
