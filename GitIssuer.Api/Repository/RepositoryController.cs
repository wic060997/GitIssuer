using System.Threading;
using System.Threading.Tasks;
using GitIssuer.Api.Repository.Requests;
using GitIssuer.Application.Repository.Commands.CreateIssue;
using GitIssuer.Application.Repository.Commands.UpdateIssue;
using GitIssuer.Application.Repository.Commands.UpdateIssueAsClose;
using GitIssuer.Application.Repository.Queries.GetRepositories;
using GitIssuer.Application.Repository.Queries.GetRepositoryIssue;
using GitIssuer.Application.Repository.Queries.GetRepositoryIssues;
using GitIssuer.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GitIssuer.Api.Repository;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RepositoryController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "This method fetch list of name repository",
        Tags = ["Repository"]
    )]
    public async Task<IActionResult> GetRepositories(
        ContentType contentType,
        [FromQuery] GetRepositoryRequest request,
        CancellationToken cancellationToken
    )
    {
        return Ok(await mediator.Send(new GetRepositoriesQuery(contentType, request.Owner), cancellationToken));
    }

    [HttpGet("{repository}/issues")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "This method fetch list of issue repository",
        Tags = ["Repository"]
    )]
    public async Task<IActionResult> GetRepositoryIssues(
        string repository,
        [FromQuery] GetRepositoryIssuesRequest request,
        CancellationToken cancellationToken
    )
    {
        var result =
            await mediator.Send(new GetRepositoryIssuesQuery(
                    request.ContentType,
                    request.Owner,
                    repository,
                    request.Page,
                    request.Limit
                ),
                cancellationToken);
        return Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "This method fetch issue",
        Tags = ["Repository"]
    )]
    [HttpGet("{repository}/issues/{issueId}")]
    public async Task<IActionResult> GetRepositoryIssues(
        string repository,
        long issueId,
        [FromQuery] GetRepositoryIssueRequest request,
        CancellationToken cancellationToken
    )
    {
        var result =
            await mediator.Send(
                new GetRepositoryIssueQuery(request.ContentType, request.Owner, repository, issueId),
                cancellationToken);
        return Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "This method create issue in repository",
        Tags = ["Repository"]
    )]
    [HttpPost("{repository}/issues")]
    public async Task<IActionResult> CreateRepositoryIssue(
        string repository,
        [FromBody] CreateRepositoryIssueRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateIssueCommand(
            request.ContentType,
            request.Owner,
            repository,
            request.Title,
            request.Body,
            request.Assignee,
            request.Assignees,
            request.Milestones,
            request.Labels
        ), cancellationToken);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("{repository}/issues/{issueId}")]
    [SwaggerOperation(
        Summary = "This method update issue in repository",
        Tags = ["Repository"]
    )]
    public async Task<IActionResult> UpdateRepositoryIssue(
        string repository,
        long issueId,
        [FromQuery] UpdateRepositoryIssueRequest request,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateIssueCommand(
            request.ContentType,
            request.Owner,
            repository,
            issueId,
            request.State,
            request.StateReason,
            request.Title,
            request.Body,
            request.Assignees,
            request.Milestones,
            request.Labels
        ), cancellationToken);

        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpPut("{repository}/issues/{issueId}/Closed")]
    [SwaggerOperation(
        Summary = "This method set issue in repository as closed",
        Tags = ["Repository"]
    )]
    public async Task<IActionResult> UpdateRepositoryIssueAsClosed(
        string repository,
        long issueId,
        [FromQuery] UpdateRepositoryIssueAsClosedRequest request,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateIssueAsCloseCommand(
            request.ContentType,
            request.Owner,
            repository,
            issueId
        ), cancellationToken);

        return StatusCode(StatusCodes.Status204NoContent);
    }
}