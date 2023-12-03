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
public class AlbumController(IChinookSupervisor chinookSupervisor, ILogger<AlbumController> logger)
    : ControllerBase
{
    [HttpGet]
    [Produces(typeof(List<AlbumApiModel>))]
    public ActionResult<List<AlbumApiModel>> Get()
    {
        try  
        {  
            var albums = chinookSupervisor.GetAllAlbum();  

            if (albums.Any())  
            {  
                return Ok(albums);  
            }  
            else  
            {  
                return StatusCode((int)HttpStatusCode.NotFound, "No Albums Could Be Found");  
            }  
        }  
        catch (Exception ex)  
        {  
            logger.LogError($"Something went wrong inside the AlbumController Get action: {ex}");  
            return StatusCode((int)HttpStatusCode.InternalServerError, "Error occurred while executing Get All Albums");  
        }  
    }

    [HttpGet("{id}", Name = "GetAlbumById")]
    public ActionResult<AlbumApiModel> Get(int id)
    {
        try  
        {  
            var album = chinookSupervisor.GetAlbumById(id);  

            if (album != null)  
            {  
                return Ok(album);  
            }  
            else  
            {  
                return StatusCode((int)HttpStatusCode.NotFound, "Album Not Found");  
            }  
        }  
        catch (Exception ex)  
        {  
            logger.LogError($"Something went wrong inside the AlbumController GetById action: {ex}");  
            return StatusCode((int)HttpStatusCode.InternalServerError, "Error occurred while executing Get Album By Id");  
        }
    }

    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    public ActionResult<AlbumApiModel> Post([FromBody] AlbumApiModel input)
    {
        try
        {
            if (input == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Given Album is null");
            }
            else
            {
                return Ok(chinookSupervisor.AddAlbum(input));
            }
        }
        catch (ValidationException  ex)  
        {  
            logger.LogError($"The Album could not validated: Add Album action: {ex}");  
            return StatusCode((int)HttpStatusCode.InternalServerError, "Validation error while executing Add Albums");  
        }
        catch (Exception ex)  
        {  
            logger.LogError($"Something went wrong inside the AlbumController Add Album action: {ex}");  
            return StatusCode((int)HttpStatusCode.InternalServerError, "Error occurred while executing Add Albums");  
        }
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public ActionResult<AlbumApiModel> Put(int id, [FromBody] AlbumApiModel input)
    {
        try  
        {  
            if (input == null)  
            {  
                return StatusCode((int)HttpStatusCode.BadRequest, "Given Album is null");
            }  
            else  
            {
                return Ok(chinookSupervisor.UpdateAlbum(input));
            }  
        }
        catch (ValidationException  ex)  
        {  
            logger.LogError($"The Album could not validated: Update Album action: {ex}");  
            return StatusCode((int)HttpStatusCode.InternalServerError, "Validation error while executing Update Albums");  
        }
        catch (Exception ex)  
        {  
            logger.LogError($"Something went wrong inside the AlbumController Update Album action: {ex}");  
            return StatusCode((int)HttpStatusCode.InternalServerError, "Error occurred while executing Update Albums");  
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try  
        {  
            return Ok(chinookSupervisor.DeleteAlbum(id)); 
        }  
        catch (Exception ex)  
        {  
            logger.LogError($"Something went wrong inside the AlbumController Delete action: {ex}");  
            return StatusCode((int)HttpStatusCode.InternalServerError, "Error occurred while executing Delete Album");  
        }
    }

    [HttpGet("artist/{id}")]
    public ActionResult<List<AlbumApiModel>> GetByArtistId(int id)
    {
        try  
        {  
            var albums = chinookSupervisor.GetAlbumByArtistId(id);  

            if (albums.Any())  
            {  
                return Ok(albums);  
            }  
            else  
            {  
                return StatusCode((int)HttpStatusCode.NotFound, "No Albums Could Be Found for the Artist");  
            }  
        }  
        catch (Exception ex)  
        {  
            logger.LogError($"Something went wrong inside the AlbumController Get By Artist action: {ex}");  
            return StatusCode((int)HttpStatusCode.InternalServerError, "Error occurred while executing Get All Albums for Artist");  
        }  
    }
}