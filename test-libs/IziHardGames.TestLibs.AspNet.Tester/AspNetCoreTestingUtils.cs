using System.Net.Http.Json;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing.Patterns;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using IziHardGames.Libs.Methods.Tester;
using IziHardGames.Libs.Methods.Tester.Randoms;

namespace СommissioningService.IntegrationTests;

public class AspNetCoreTestingUtils
{
    /// <summary>
    /// Каждый эндпоинт будет протестирован набором инпутов. Каждая модель в наборе будет иметь только 1 установленное свойство, остальные будут по умолчанию. <see cref="IziHardGames.Libs.Methods.Tester.VariatorForModel"/>  
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="logger"></param>
    /// <typeparam name="T"></typeparam>
    public static async Task TestWithVariants<T>(WebApplicationFactory<T> factory, ILogger logger) where T : class
    {
        await Task.CompletedTask;
        var lookups = AspNetCoreTestingUtils.GetEndpoints(factory);
        var infos = AspNetCoreTestingUtils.ToActionInfo(lookups);
        var client = factory.CreateClient();
        foreach (var info in infos)
        {
            await TestEndpoint(client, info, logger);
        }
    }

    public static IEnumerable<Endpoint> GetEndpoints<T>(WebApplicationFactory<T> factory) where T : class
    {
        var builder = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Disable startup discovery to prevent automatic startup
                    // services.AddSingleton<IStartupFilter>();
                });
            });

        var server = builder.Server;
        var endpointDataSource = server.Services.GetRequiredService<EndpointDataSource>();
        var endpoints = endpointDataSource.Endpoints;
        return endpoints;
    }

    public static IEnumerable<ActionInfo> ToActionInfo(IEnumerable<Endpoint> endpoints)
    {
        return endpoints.Select(x => CreateActionInfo(x));
    }

    public static bool IsMinimalApi(Endpoint endpoint)
    {
        return endpoint.DisplayName == "HTTP:GET" && !endpoint.Metadata.Any(x => x is ControllerActionDescriptor);
    }

    public static bool IsControllerAction(Endpoint endpoint)
    {
        return endpoint.Metadata.Any(x => x is ControllerActionDescriptor);
    }

    public static ActionInfo CreateActionInfo(Endpoint endpoint)
    {
        MethodInfo? methodInfo = default;
        HttpMethod? httpMethod = default;
        if (endpoint is RouteEndpoint routeEndpoint)
        {
            var metaData = routeEndpoint.Metadata.GetRequiredMetadata<HttpMethodMetadata>();
            if (metaData.HttpMethods.Any(x => x == "GET"))
            {
                httpMethod = HttpMethod.Get;
            }
            else if (metaData.HttpMethods.Any(x => x == "POST"))
            {
                httpMethod = HttpMethod.Post;
            }

            var patter = routeEndpoint.RoutePattern;
            var descriptor = routeEndpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (descriptor != null)
            {
                MethodInfo mi = descriptor.MethodInfo;
                methodInfo = mi;
            }

            var iroute = endpoint.Metadata.FirstOrDefault(x => x is IRouteDiagnosticsMetadata) as IRouteDiagnosticsMetadata;
            var route = iroute?.Route ?? string.Empty;


            var runtimeMethodInfo = endpoint.Metadata.Where(x => x.GetType().IsSubclassOf(typeof(MethodInfo)));
            if (runtimeMethodInfo.Count() > 0)
            {
                // internal RuntimeMethodInf, значит делегат 
            }

            ArgumentNullException.ThrowIfNull(methodInfo);

            return new ActionInfo()
            {
                endpoint = routeEndpoint,
                handler = methodInfo,
                model = default!,
                method = httpMethod!,
                route = route,
                pattern = routeEndpoint.RoutePattern,
                target = routeEndpoint.RequestDelegate?.Target,
                methodInstance = routeEndpoint.RequestDelegate
            };
        }
        else
        {
            throw new NotImplementedException(endpoint.GetType().FullName);
        }
    }

    public static async Task TestEndpoint(HttpClient client, ActionInfo actionInfo, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(actionInfo.handler);
        var mi = actionInfo.handler;

        if (mi.IsSingleArgument())
        {
            var model = mi.GetParameters().First().ParameterType;
            Type type = GetContentType(actionInfo);
            var instance = Activator.CreateInstance(model);
            HttpContent? content = default;
            HttpResponseMessage? response = default;

            if (type == typeof(JsonContent))
            {
                content = JsonContent.Create(instance);
            }
            else
            {
                throw new NotImplementedException();
            }

            if (actionInfo.method == HttpMethod.Get)
            {
                response = await client.GetAsync(actionInfo.route).ConfigureAwait(false);
            }
            else if (actionInfo.method == HttpMethod.Post)
            {
                response = await client.PostAsync(actionInfo.route, content).ConfigureAwait(false);
            }
            else
            {
                throw new NotImplementedException();
            }

            logger.LogInformation(await response.Content.ReadAsStringAsync());
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, ex.Message);
            }
            // await MethodSingleArgumentTester.TestAsync(actionInfo.target, mi, model);
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private static Type GetContentType(ActionInfo actionInfo)
    {
        return typeof(JsonContent);
    }

    private static Task TestEndpointForCreate(object target, MethodInfo handler, Type model)
    {
        // send invalid model
        // send ok model
        // repeat send ok model. check AlreadyExists exception
        throw new NotImplementedException();
    }
}

public class ActionInfo
{
    public Endpoint? endpoint;
    public HttpMethod? method;
    public MethodInfo? handler;

    /// <summary>
    /// Not Argument's type. модель MVC.
    /// </summary>
    public Type? model;

    public string? route;
    public RoutePattern? pattern;

    /// <summary>
    /// Объект-владелец <see cref="method"/>. Null если метод статический
    /// </summary>
    public object? target;
    public object? methodInstance;
}