// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// DI extension methods for adding IdentityServer
    /// </summary>
    public static class IdentityServerServiceCollectionExtensions
    {
        /// <summary>
        /// Creates a builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddIdentityServerBuilder(this IServiceCollection services)
        {
            return new IdentityServerBuilder(services);
        }

        /// <summary>
        /// Adds IdentityServer.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services)
        {
            var builder = services.AddIdentityServerBuilder();

            builder
                //增加平台相关的配置 往DI中增加Ioptions ，IHttpContextAccessor等
                .AddRequiredPlatformServices()
                //增加认证服务 IAuthenticationService  IdentityServerAuthenticationService  新的实现类，并将原来的任务服务进行装饰，使用装饰器模式调用原来的类
                .AddCookieAuthentication()
                //添加核心服务
                .AddCoreServices()
                //添加终结点处理器
                .AddDefaultEndpoints()
                //添加可以拓展的服务
                .AddPluggableServices()
                //添加验证器
                .AddValidators()
                //添加结果生成器
                .AddResponseGenerators()
                //添加安全相关的解析器
                .AddDefaultSecretParsers()
                //添加安全相关的验证器
                .AddDefaultSecretValidators();
                //将授权 保存在内存中
            // provide default in-memory implementation, not suitable for most production scenarios
            builder.AddInMemoryPersistedGrants();

            return builder;
        }

        /// <summary>
        /// Adds IdentityServer.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="setupAction">The setup action.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, Action<IdentityServerOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddIdentityServer();
        }

        /// <summary>
        /// Adds the IdentityServer.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityServerOptions>(configuration);
            return services.AddIdentityServer();
        }

        /// <summary>
        /// Configures the OpenIdConnect handlers to persist the state parameter into the server-side IDistributedCache.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="schemes">The schemes to configure. If none provided, then all OpenIdConnect schemes will use the cache.</param>
        public static IServiceCollection AddOidcStateDataFormatterCache(this IServiceCollection services, params string[] schemes)
        {
            services.AddSingleton<IPostConfigureOptions<OpenIdConnectOptions>>(
                svcs => new ConfigureOpenIdConnectOptions(
                    schemes,
                    svcs.GetRequiredService<IHttpContextAccessor>())
            );

            return services;
        }
    }
}