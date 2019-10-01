﻿using Calabonga.Microservice.IdentityModule.Data.ModelConfigurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calabonga.Microservice.IdentityModule.Data.ModelConfigurations
{
    /// <summary>
    /// Entity Type Configuration for entity <see cref="ApplicationUserProfile"/>
    /// </summary>

    public class ApplicationUserProfileModelConfiguration : AuditableModelConfigurationBase<ApplicationUserProfile>
    {
        protected override void AddBuilder(EntityTypeBuilder<ApplicationUserProfile> builder)
        {
            builder.Property(x => x.ApplicationUserId).IsRequired();
            builder.HasOne(x => x.ApplicationUser);
        }

        protected override string TableName()
        {
            return "Profiles";
        }
    }
}