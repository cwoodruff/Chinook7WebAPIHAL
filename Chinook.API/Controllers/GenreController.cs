using System.Net;
using Chinook.Domain.ApiModels;
using Chinook.Domain.Supervisor;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableCors("CorsPolicy")]
public class GenreController(IChinookSupervisor chinookSupervisor, ILogger<GenreController> logger)
    : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    public ActionResult<List<GenreApiModel>> Get()
    {
        try
        {
            var genres = chinookSupervisor.GetAllGenre();

            if (genres.Any())
            {
                return Ok(genres);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotFound, "No Genres Could Be Found");
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside the GenreController Get action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Get All Genres");
        }
    }

    [HttpGet("{id}", Name = "GetGenreById")]
    [Produces("application/json")]
    public ActionResult<GenreApiModel> Get(int id)
    {
        try
        {
            var genre = chinookSupervisor.GetGenreById(id);

            if (genre != null)
            {
                return Ok(genre);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotFound, "Genre Not Found");
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside the GenreController GetById action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Get Genre By Id");
        }
    }

    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    public ActionResult<GenreApiModel> Post([FromBody] GenreApiModel input)
    {
        try
        {
            if (input == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Given Genre is null");
            }
            else
            {
                return Ok(chinookSupervisor.AddGenre(input));
            }
        }
        catch (ValidationException ex)
        {
            logger.LogError($"Something went wrong inside the GenreController Add Genre action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError, "Error occurred while executing Add Genres");
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside the GenreController Add Genre action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError, "Error occurred while executing Add Genres");
        }
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public ActionResult<GenreApiModel> Put(int id, [FromBody] GenreApiModel input)
    {
        try
        {
            if (input == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Given Genre is null");
            }
            else
            {
                return Ok(chinookSupervisor.UpdateGenre(input));
            }
        }
        catch (ValidationException ex)
        {
            logger.LogError($"Something went wrong inside the GenreController Update Genre action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Update Genres");
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside the GenreController Update Genre action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing AUpdatedd Genres");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            return Ok(chinookSupervisor.DeleteGenre(id));
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside the GenreController Delete action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Delete Genre");
        }
    }
}