using Autofac;
using Autofac.Extensions.DependencyInjection;
using GitIssuer.Application;
using GitIssuer.Infrastructure.Providers;
using GitIssuer.Provider;
using GitIssuer.Provider.Github;
using GitIssuer.Provider.Gitlab;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

namespace GitIssuer.Infrastructure;

public class GitIssuerStartupAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterMediatR(MediatRConfigurationBuilder.Create(typeof(IGitIssuerApplication).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered().Build());

        builder.RegisterType<GithubProvider>()
            .As<IGithubProvider>()
            .InstancePerDependency();
        
        builder.RegisterType<GitlabProvider>()
            .As<IGitlabProvider>()
            .InstancePerDependency();
    }
}