﻿using Autofac;
using SeenLive.Core.Abstractions;
using SeenLive.DataAccess.Models;
using SeenLive.DataAccess.Services.MongoServices;

namespace SeenLive.DataAccess
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // TODO db services singleinstance or per dependency?
            builder.RegisterType<ArtistService<ArtistEntry>>().As<IArtistService>().SingleInstance();
            builder.RegisterType<DatesService<DateEntry>>().As<IDatesService>().SingleInstance();

            builder.RegisterType<MongoDBContext>().AsSelf().SingleInstance();
        }
    }
}