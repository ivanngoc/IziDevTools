using System;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using SharpYaml.Serialization.Logging;

namespace IziHardGames.Libs.Tests;

public class TestEndpoints
{
    public void Test()
    {
    }

    public void Test(TestServer testServer)
    {
        throw new NotImplementedException();
    }

    public void Test(IHost host)
    {
        throw new NotImplementedException();
    }

    public void ExposeMeta(RouteAttribute atrRoute, HttpPostAttribute atrPost, ProducesResponseTypeAttribute atrProd, HttpMethodMetadata hmmd, RouteNameMetadata rnm, ApiControllerAttribute apiControllerAttribute, IRouteDiagnosticsMetadata iRouteMetaData)
    {
    }

    public void ExposeTypes(RouteEndpoint routeEndpoint)
    {
    }
}