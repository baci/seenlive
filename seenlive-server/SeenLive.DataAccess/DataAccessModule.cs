﻿using Autofac;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Settings;
using SeenLive.DataAccess.Models;
using SeenLive.DataAccess.Services.MongoServices;

namespace SeenLive.DataAccess
{
    public class DataAccessModule : Module
    {
        private readonly ISeenLiveDatabaseSettings _databaseSettings;

        public DataAccessModule(ISeenLiveDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // TODO db services singleinstance or per dependency?
            builder.RegisterType<ArtistsRepository<ArtistEntity>>().As<IArtistsRepository>().SingleInstance();
            builder.RegisterType<DatesRepository<DateEntity>>().As<IDatesRepository>().SingleInstance();
            
            // database setup with appsettings configuration
            builder.RegisterInstance(_databaseSettings).As<ISeenLiveDatabaseSettings>().SingleInstance();

            builder.RegisterType<MongoDBContext>().AsSelf().SingleInstance();
        }
    }
}
