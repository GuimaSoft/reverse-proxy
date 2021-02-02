// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ReverseProxy.Service.Config;
using Microsoft.ReverseProxy.Service.RuntimeModel.Transforms;
using Xunit;

namespace Microsoft.ReverseProxy.Abstractions.Config
{
    public class PathTransformExtensionsTests : TransformExtentionsTestsBase
    {
        private readonly PathTransformFactory _factory;

        public PathTransformExtensionsTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();
            serviceCollection.AddRouting();
            var services = serviceCollection.BuildServiceProvider();

            _factory = new PathTransformFactory(services.GetRequiredService<TemplateBinderFactory>());
        }

        [Fact]
        public void WithTransformPathSet()
        {
            var proxyRoute = CreateProxyRoute();
            proxyRoute = proxyRoute.WithTransformPathSet(new PathString("/path#"));

            var transformValues = Assert.Single(proxyRoute.Transforms);
            Validate(_factory, proxyRoute, transformValues);

            var builderContext = CreateBuilderContext(proxyRoute);
            Assert.True(_factory.Build(builderContext, transformValues));

            ValidatePathSet(builderContext);
        }

        [Fact]
        public void AddPathSet()
        {
            var proxyRoute = CreateProxyRoute();
            var builderContext = CreateBuilderContext(proxyRoute);
            builderContext.AddPathSet(new PathString("/path#"));

            ValidatePathSet(builderContext);
        }

        private static void ValidatePathSet(TransformBuilderContext builderContext)
        {
            var requestTransform = Assert.Single(builderContext.RequestTransforms);
            var pathStringTransform = Assert.IsType<PathStringTransform>(requestTransform);
            Assert.Equal(PathStringTransform.PathTransformMode.Set, pathStringTransform.Mode);
            Assert.Equal("/path#", pathStringTransform.Value.Value);
        }

        [Fact]
        public void WithTransformPathRemovePrefix()
        {
            var proxyRoute = CreateProxyRoute();
            proxyRoute = proxyRoute.WithTransformPathRemovePrefix(new PathString("/path#"));

            var transformValues = Assert.Single(proxyRoute.Transforms);
            Validate(_factory, proxyRoute, transformValues);

            var builderContext = CreateBuilderContext(proxyRoute);
            Assert.True(_factory.Build(builderContext, transformValues));

            ValidatePathRemovePrefix(builderContext);
        }

        [Fact]
        public void AddPathRemovePrefix()
        {
            var proxyRoute = CreateProxyRoute();
            var builderContext = CreateBuilderContext(proxyRoute);
            builderContext.AddPathRemovePrefix(new PathString("/path#"));

            ValidatePathRemovePrefix(builderContext);
        }

        private static void ValidatePathRemovePrefix(TransformBuilderContext builderContext)
        {
            var requestTransform = Assert.Single(builderContext.RequestTransforms);
            var pathStringTransform = Assert.IsType<PathStringTransform>(requestTransform);
            Assert.Equal(PathStringTransform.PathTransformMode.RemovePrefix, pathStringTransform.Mode);
            Assert.Equal("/path#", pathStringTransform.Value.Value);
        }

        [Fact]
        public void WithTransformPathPrefix()
        {
            var proxyRoute = CreateProxyRoute();
            proxyRoute = proxyRoute.WithTransformPathPrefix(new PathString("/path#"));

            var transformValues = Assert.Single(proxyRoute.Transforms);
            Validate(_factory, proxyRoute, transformValues);

            var builderContext = CreateBuilderContext(proxyRoute);
            Assert.True(_factory.Build(builderContext, transformValues));

            ValidatePathPrefix(builderContext);
        }

        [Fact]
        public void AddPathPrefix()
        {
            var proxyRoute = CreateProxyRoute();
            var builderContext = CreateBuilderContext(proxyRoute);
            builderContext.AddPathPrefix(new PathString("/path#"));

            ValidatePathPrefix(builderContext);
        }

        private static void ValidatePathPrefix(TransformBuilderContext builderContext)
        {
            var requestTransform = Assert.Single(builderContext.RequestTransforms);
            var pathStringTransform = Assert.IsType<PathStringTransform>(requestTransform);
            Assert.Equal(PathStringTransform.PathTransformMode.Prefix, pathStringTransform.Mode);
            Assert.Equal("/path#", pathStringTransform.Value.Value);
        }

        [Fact]
        public void WithTransformPathRouteValues()
        {
            var proxyRoute = CreateProxyRoute();
            proxyRoute = proxyRoute.WithTransformPathRouteValues(new PathString("/path#"));

            var transformValues = Assert.Single(proxyRoute.Transforms);
            Validate(_factory, proxyRoute, transformValues);

            var builderContext = CreateBuilderContext(proxyRoute);
            Assert.True(_factory.Build(builderContext, transformValues));

            ValidatePathRouteValues(builderContext);
        }

        [Fact]
        public void AddPathRouteValues()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();
            serviceCollection.AddRouting();
            var services = serviceCollection.BuildServiceProvider();

            var proxyRoute = CreateProxyRoute();
            var builderContext = CreateBuilderContext(proxyRoute, services);
            builderContext.AddPathRouteValues(new PathString("/path#"));

            ValidatePathRouteValues(builderContext);
        }

        private static void ValidatePathRouteValues(TransformBuilderContext builderContext)
        {
            var requestTransform = Assert.Single(builderContext.RequestTransforms);
            var pathRouteValuesTransform = Assert.IsType<PathRouteValuesTransform>(requestTransform);
            Assert.Equal("/path#", pathRouteValuesTransform.Template.TemplateText);
        }
    }
}