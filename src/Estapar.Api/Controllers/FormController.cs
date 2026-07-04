using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using TcmTask.Application.Domain.Contracts.Services;
using TcmTask.Application.Domain.Dtos.Request.Form;
using TcmTask.Application.Domain.Dtos.Request.FormFactory;
using TcmTask.Application.Domain.Dtos.Response;
using TcmTask.Application.Domain.Dtos.Response.Form;
using TcmTask.Application.Domain.Dtos.Response.FormFactory;

namespace TcmTask.Api.Controllers;

/// <summary>
/// Provides API endpoints for the FormFactory onboarding flow orchestrated by TCM.
/// </summary>
/// <param name="formService">The form service responsible for the invitation workflow.</param>
[ApiController]
[Route("api/tcmtask/v1/formfactory")]
public class FormController(
    IFormService formService
    ) : ControllerBase
{
    /// <summary>
    /// Creates a FormFactory onboarding invitation, stores the result and dispatches the invitation email.
    /// </summary>
    /// <remarks>
    /// The flow executed by this endpoint:
    /// <list type="number">
    ///   <item><description>Calls the FormFactory API to create the invitation and generate the form URL.</description></item>
    ///   <item><description>Persists the invitation record with status <c>Created</c>.</description></item>
    ///   <item><description>Sends the invitation email to the customer.</description></item>
    ///   <item><description>If the email is sent successfully, updates the status to <c>Sended</c>.</description></item>
    /// </list>
    /// </remarks>
    /// <param name="request">The invitation request containing customer data, required documents and optional process identifier.</param>
    /// <returns>An <see cref="IActionResult"/> containing the persisted TCM invitation record with its final status.</returns>
    [HttpPost("invitations")]
    public async Task<IActionResult> CreateInvitationAsync(
        [FromBody] CreateFormInvitationRequest request
        )
    {
        var response =
            await formService
                .CreateInvitationAsync(request);

        return new ApiObjectResult(
            HttpStatusCode.Created,
            new ApiResult<FormInvitationResponse>(
                true,
                HttpStatusCode.Created,
                response
            )
        );
    }

    /// <summary>
    /// Retrieves a FormFactory onboarding invitation by its unique identifier, including all linked documents.
    /// </summary>
    /// <param name="invitationId">The unique identifier of the TCM invitation record.</param>
    /// <returns>An <see cref="IActionResult"/> containing the invitation detail with its associated documents.</returns>
    [HttpGet("invitations/{invitationId}")]
    public async Task<IActionResult> GetInvitationByIdAsync(
        [FromRoute] Guid invitationId
        )
    {
        var response =
            await formService
                .GetByIdAsync(invitationId);

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<FormInvitationDetailResponse>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Generates a fresh presigned download URL for the specified document by calling the FormFactory API.
    /// </summary>
    /// <remarks>
    /// Since FormFactory presigned URLs expire after one hour, this endpoint can be used to renew
    /// access to a file already linked to an invitation without fetching the entire invitation record.
    /// </remarks>
    /// <param name="documentId">The unique identifier of the TCM document record.</param>
    /// <returns>An <see cref="IActionResult"/> containing the fresh presigned download URL and file metadata.</returns>
    [HttpGet("documents/download-url/{documentId}")]
    public async Task<IActionResult> GetDocumentDownloadUrlAsync(
        [FromRoute] Guid documentId
        )
    {
        var response =
            await formService
                .GetDocumentDownloadUrlAsync(documentId);

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<FormFactoryFileDownloadUrlResponse>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Generates a presigned PUT URL for uploading a file directly to the validated S3 bucket,
    /// outside of the onboarding invitation flow.
    /// </summary>
    /// <remarks>
    /// The returned <c>upload_url</c> must be used with an HTTP PUT request directly to S3.
    /// No record is persisted in FormFactory — the TCMTask system is responsible for tracking the upload.
    /// </remarks>
    /// <param name="request">The upload request details including client ID, document type, file name and size.</param>
    /// <returns>An <see cref="IActionResult"/> containing the presigned PUT URL and the resulting S3 key.</returns>
    [HttpPost("documents/upload-url")]
    public async Task<IActionResult> GetExternalUploadUrlAsync(
        [FromBody] FormFactoryExternalUploadUrlRequest request
        )
    {
        var response =
            await formService
                .GetExternalUploadUrlAsync(request);

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<FormFactoryExternalUploadUrlResponse>(
                true,
                HttpStatusCode.OK,
                response
            )
        );
    }

    /// <summary>
    /// Uploads a file to the specified invitation via the FormFactory API.
    /// </summary>
    /// <remarks>
    /// The request must be sent as <c>multipart/form-data</c> with the fields
    /// <c>document_type</c> (text) and <c>file</c> (binary).
    /// </remarks>
    /// <param name="invitationId">The unique identifier of the TCM invitation record.</param>
    /// <param name="documentType">The document type identifier (e.g. <c>image</c>).</param>
    /// <param name="file">The file to upload.</param>
    /// <returns>An <see cref="IActionResult"/> containing the uploaded file metadata.</returns>
    [HttpPost("invitations/{invitationId}/files/upload")]
    public async Task<IActionResult> UploadInvitationFileAsync(
        [FromRoute] Guid invitationId,
        [FromForm] string documentType,
        IFormFile file
        )
    {
        var response =
            await formService
                .UploadInvitationFileAsync(
                    invitationId,
                    documentType,
                    file
                );

        return new ApiObjectResult(
            HttpStatusCode.Created,
            new ApiResult<FormFactoryUploadFileResponse>(
                true,
                HttpStatusCode.Created,
                response
            )
        );
    }

    /// <summary>
    /// Uploads a file directly to the validated S3 bucket via the FormFactory
    /// <c>/files/upload-url</c> endpoint.
    /// </summary>
    /// <remarks>
    /// The request must be sent as <c>multipart/form-data</c> with the fields
    /// <c>document_type</c> (text) and <c>file</c> (binary).
    /// </remarks>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="documentType">The document type identifier (e.g. <c>image</c>).</param>
    /// <param name="file">The file to upload.</param>
    /// <returns>An <see cref="IActionResult"/> containing the uploaded file metadata.</returns>
    [HttpPost("files/upload")]
    public async Task<IActionResult> UploadFileAsync(
        [FromForm] string clientId,
        [FromForm] string documentType,
        IFormFile file
        )
    {
        var response =
            await formService
                .UploadFileAsync(
                    clientId,
                    documentType,
                    file
                );

        return new ApiObjectResult(
            HttpStatusCode.Created,
            new ApiResult<FormFactoryUploadFileResponse>(
                true,
                HttpStatusCode.Created,
                response
            )
        );
    }
}
