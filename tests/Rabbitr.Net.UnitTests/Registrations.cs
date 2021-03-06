using Autofac;
using Microsoft.Extensions.Options;
using Moq;
using Rabbitr.Handlers;
using Rabbitr.Logging;
using Rabbitr.Net;
using Rabbitr.Net.Abstractions;
using Rabbitr.Net.Configuration;

namespace test
{
    public static class Registrations
    {
        public static IContainer Generate(IRabbitrHandler handler)
        {
            var builder = new ContainerBuilder();

            var configuration = new Mock<IOptions<RabbitrConfiguration>>();
            configuration.Setup(x => x.Value).Returns(new RabbitrConfiguration{ HostName = "localhost", PortNumber = 32777 });

            builder.Register(c => configuration.Object).As<IOptions<RabbitrConfiguration>>();
            builder.RegisterType<RabbitrConnection>().As<IRabbitrConnection>();

            builder.Register(c => handler).As<IRabbitrHandler>().InstancePerLifetimeScope();
            builder.RegisterType<NullLogger>().As<IRabbitrLogging>();

            builder.RegisterType<RabbitrHandlerFactory>().As<IRabbitrHandlerFactory>();
            builder.RegisterType<RabbitrClient>().As<RabbitrClient>();

            return builder.Build();
        }
    }
}