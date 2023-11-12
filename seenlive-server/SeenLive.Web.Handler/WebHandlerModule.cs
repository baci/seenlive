using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection;
using SeenLive.Web.Handler.Requests;

namespace SeenLive.Web.Handler
{
    public class WebHandlerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var configuration = MediatRConfigurationBuilder
            .Create(typeof(AddOrUpdateArtistEntryRequest).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

            // this will add all your Request- and Notificationhandler
            // that are located in the same project as your program-class
            builder.RegisterMediatR(configuration);
        }
    }
}
