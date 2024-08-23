using GitIssuer.Domain;
using MediatR;

namespace GitIssuer.Application.Repository.Queries.GetRepositories;

public record GetRepositoriesQuery(ContentType ContentType, string Owner) : IRequest<List<string>>;