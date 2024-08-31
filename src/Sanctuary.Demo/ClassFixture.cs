﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sanctuary.xUnit.v3;

namespace Sanctuary.Demo;

/// <summary>
/// A fixture that is created for each test class/collection.
/// </summary>
public class ClassFixture
{
    public ClassFixture(TenantFixture tenantFixture)
    {
        var services = new ServiceCollection();

        // The "test" is a nonsensical connection string, but it doesn't matter,
        // because will be replaced by EfCore patcher.
        services.AddDbContext<QueryDbContext>(opt => opt.UseSqlServer("test"));

        // Needs to be last, because it overrides service registrations
        // of data access services.
        services.AddSanctuary<ClassFixture>(tenantFixture.Lake);

        ServiceProvider = services.BuildServiceProvider();
    }

    public ServiceProvider ServiceProvider { get; }
}