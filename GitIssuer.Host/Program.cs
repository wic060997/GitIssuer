using Autofac;
using Autofac.Extensions.DependencyInjection;
using GitIssuer.Api;
using GitIssuer.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configurations = builder.Configuration;

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new GitIssuerStartupAutofacModule());
});

builder.Services.AddControllers()
    .AddApplicationPart(typeof(IGitIssuerApi).Assembly)
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddHttpClient();
builder.Services.AddHttpClient("GithubProvider", client =>
{
    client.BaseAddress = new Uri(configurations["Credentials:GitHub:ApiUrl"] ?? string.Empty);
    client.DefaultRequestHeaders.Add("Accept", configurations["Credentials:GitHub:Accept"] ?? string.Empty);
    client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Github");
});

builder.Services.AddHttpClient("GitlabProvider",
    client =>
    {
        client.BaseAddress = new Uri(configurations["Credentials:Gitlab:ApiUrl"] ?? string.Empty);
        client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Gitlab");
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSwagger(options => { options.SerializeAsV2 = true; });
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
    options.InjectStylesheet("/assets/custom.css");
});

app.MapSwagger().RequireAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.Run();