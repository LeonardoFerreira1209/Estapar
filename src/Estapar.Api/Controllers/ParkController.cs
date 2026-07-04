using Estapar.Domain.Contracts.Services;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Dtos.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Estapar.Api.Controllers;

/// <summary>
/// Provides API endpoints for managing parking facilities (Parks) and their associated Lanes and Garages.
/// </summary>
/// <param name="parkService">The park service responsible for business logic.</param>
[ApiController]
[Route("api/estapar/v1/parks")]
public class ParkController(
    IParkService parkService
    ) : ControllerBase
{
    /// <summary>
    /// Creates a new park with its associated lanes and garages.
    /// </summary>
    /// <remarks>
    /// This endpoint creates a complete park structure including:
    /// <list type="bullet">
    ///   <item><description>The park itself with name and description</description></item>
    ///   <item><description>All lanes specified in the request</description></item>
    ///   <item><description>All garages specified in the request</description></item>
    /// </list>
    /// </remarks>
    /// <param name="request">The request containing park details and nested lanes/garages.</param>
    /// <returns>An <see cref="IActionResult"/> containing the created park with all associations.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateParkAsync(
        [FromBody] CreateParkRequest request
        )
    {
        var response = 
            await parkService.CreateAsync(
                request
            );

        return new ApiObjectResult(
            HttpStatusCode.Created,
            new ApiResult<ParkDetailResponse>(
                true,
                HttpStatusCode.Created,
                response
            )
        );
    }

    /// <summary>
    /// Retrieves a park by its unique identifier including all associated lanes and garages.
    /// </summary>
    /// <param name="id">The unique identifier of the park.</param>
    /// <returns>An <see cref="IActionResult"/> containing the park details with all associations.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetParkByIdAsync(
        [FromRoute] Guid id
        )
    {
        var response = 
            await parkService.GetByIdAsync(
                id
            );

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<ParkDetailResponse>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Retrieves all parks with basic information (without nested lanes and garages).
    /// </summary>
    /// <remarks>
    /// This endpoint returns a list of all parks with only their basic information.
    /// To get detailed information including lanes and garages, use the GET by ID endpoint.
    /// </remarks>
    /// <returns>An <see cref="IActionResult"/> containing a list of all parks.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllParksAsync()
    {
        var response = 
            await parkService.GetAllAsync();

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<List<ParkResponse>>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Updates an existing park and manages its associated lanes and garages.
    /// </summary>
    /// <remarks>
    /// This endpoint handles full update of the park structure:
    /// <list type="bullet">
    ///   <item><description>Updates park name and description</description></item>
    ///   <item><description>Adds new lanes if they don't have an ID</description></item>
    ///   <item><description>Updates existing lanes that have an ID</description></item>
    ///   <item><description>Deletes lanes that are not in the request</description></item>
    ///   <item><description>Same logic applies to garages</description></item>
    /// </list>
    /// </remarks>
    /// <param name="id">The unique identifier of the park to update.</param>
    /// <param name="request">The request containing updated park details and nested lanes/garages.</param>
    /// <returns>An <see cref="IActionResult"/> containing the updated park with all associations.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateParkAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateParkRequest request
        )
    {
        var response = 
            await parkService.UpdateAsync(
                id, 
                request
            );

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<ParkDetailResponse>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Deletes a park and all its associated lanes and garages.
    /// </summary>
    /// <remarks>
    /// This operation is permanent and will delete:
    /// <list type="bullet">
    ///   <item><description>The park itself</description></item>
    ///   <item><description>All associated lanes</description></item>
    ///   <item><description>All associated garages</description></item>
    /// </list>
    /// </remarks>
    /// <param name="id">The unique identifier of the park to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating successful deletion.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParkAsync(
        [FromRoute] Guid id
        )
    {
        await parkService.DeleteAsync(
            id
        );

        return new ApiObjectResult(
            HttpStatusCode.NoContent,
            new ApiResult<object>(
                true,
                HttpStatusCode.NoContent,
                null
            )
        );
    }
}
