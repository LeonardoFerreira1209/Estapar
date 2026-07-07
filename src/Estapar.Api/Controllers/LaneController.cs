using Estapar.Domain.Contracts.Services;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Dtos.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Estapar.Api.Controllers;

/// <summary>
/// Provides API endpoints for managing Lanes within a specific Park.
/// </summary>
/// <param name="laneService">The lane service responsible for business logic.</param>
[ApiController]
[Route("api/estapar/v1/lanes")]
public class LaneController(
    ILaneService laneService
    ) : ControllerBase
{
    /// <summary>
    /// Creates a new lane within the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="request">The request containing lane details.</param>
    /// <returns>An <see cref="IActionResult"/> containing the created lane.</returns>
    [HttpPost("parks/{parkId}")]
    public async Task<IActionResult> CreateLaneAsync(
        [FromRoute] Guid parkId,
        [FromBody] CreateLaneRequest request,
        CancellationToken cancellationToken
        )
    {
        var response =
            await laneService.CreateAsync(
                parkId,
                request,
                cancellationToken
            );

        return new ApiObjectResult(
            HttpStatusCode.Created,
            new ApiResult<LaneResponse>(
                true,
                HttpStatusCode.Created,
                response
            )
        );
    }

    /// <summary>
    /// Retrieves a lane by its unique identifier.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="id">The unique identifier of the lane.</param>
    /// <returns>An <see cref="IActionResult"/> containing the lane details.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLaneByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
    {
        var response =
            await laneService.GetByIdAsync(
                id,
                cancellationToken
            );

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<LaneResponse>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Retrieves all lanes belonging to the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <returns>An <see cref="IActionResult"/> containing a list of lanes.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLanesByParkAsync(
        [FromRoute] Guid parkId,
        CancellationToken cancellationToken
        )
    {
        var response =
            await laneService.GetByParkIdAsync(
                parkId,
                cancellationToken
            );

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<List<LaneResponse>>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Updates an existing lane.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="id">The unique identifier of the lane to update.</param>
    /// <param name="request">The request containing updated lane details.</param>
    /// <returns>An <see cref="IActionResult"/> containing the updated lane.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLaneAsync(
        [FromBody] UpdateLaneRequest request,
        CancellationToken cancellationToken
        )
    {
        var response =
            await laneService.UpdateAsync(
                request,
                cancellationToken
            );

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<LaneResponse>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Deletes an existing lane.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="id">The unique identifier of the lane to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating successful deletion.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLaneAsync(
        [FromRoute] Guid parkId,
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
    {
        await laneService.DeleteAsync(
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
