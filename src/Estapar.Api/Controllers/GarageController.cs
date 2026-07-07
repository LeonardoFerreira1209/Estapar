using Estapar.Domain.Contracts.Services;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Dtos.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Estapar.Api.Controllers;

/// <summary>
/// Provides API endpoints for managing Garages within a specific Park.
/// </summary>
/// <param name="garageService">The garage service responsible for business logic.</param>
[ApiController]
[Route("api/estapar/v1/garages")]
public class GarageController(
    IGarageService garageService
    ) : ControllerBase
{
    /// <summary>
    /// Creates a new garage within the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="request">The request containing garage details.</param>
    /// <returns>An <see cref="IActionResult"/> containing the created garage.</returns>
    [HttpPost("parks/{parkId}")]
    public async Task<IActionResult> CreateGarageAsync(
        [FromRoute] Guid parkId,
        [FromBody] CreateGarageRequest request,
        CancellationToken cancellationToken
        )
    {
        var response =
            await garageService.CreateAsync(
                parkId,
                request,
                cancellationToken
            );

        return new ApiObjectResult(
            HttpStatusCode.Created,
            new ApiResult<GarageResponse>(
                true,
                HttpStatusCode.Created,
                response
            )
        );
    }

    /// <summary>
    /// Retrieves a garage by its unique identifier.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="id">The unique identifier of the garage.</param>
    /// <returns>An <see cref="IActionResult"/> containing the garage details.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGarageByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
    {
        var response =
            await garageService.GetByIdAsync(
                id,
                cancellationToken
            );

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<GarageResponse>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Retrieves all garages belonging to the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <returns>An <see cref="IActionResult"/> containing a list of garages.</returns>
    [HttpGet("parks/{parkId}")]
    public async Task<IActionResult> GetGaragesByParkAsync(
        [FromRoute] Guid parkId,
        CancellationToken cancellationToken
        )
    {
        var response =
            await garageService.GetByParkIdAsync(
                parkId,
                cancellationToken
            );

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<List<GarageResponse>>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Updates an existing garage.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="id">The unique identifier of the garage to update.</param>
    /// <param name="request">The request containing updated garage details.</param>
    /// <returns>An <see cref="IActionResult"/> containing the updated garage.</returns>
    [HttpPut]
    public async Task<IActionResult> UpdateGarageAsync(
        [FromBody] UpdateGarageRequest request,
        CancellationToken cancellationToken
        )
    {
        var response =
            await garageService.UpdateAsync(
                request,
                cancellationToken
            );

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<GarageResponse>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Deletes an existing garage.
    /// </summary>
    /// <param name="id">The unique identifier of the garage to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating successful deletion.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGarageAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
    {
        await garageService.DeleteAsync(
            id,
            cancellationToken
        );

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<object>(
                true,
                HttpStatusCode.OK,
                null
            )
        );
    }
}
