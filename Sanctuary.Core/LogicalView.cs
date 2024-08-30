﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Sanctuary;

[PublicAPI]
public class LogicalView
{
    /// <summary>
    /// Key: data access. Value: tenant name.
    /// </summary>
    internal readonly Dictionary<Type, string> _dataAccess;

    /// <summary>
    /// Key: tenant name. Value: component name and data source.
    /// </summary>
    internal readonly Dictionary<string, TenantConfig> _tenants;

    /// <summary>
    /// Key: component name. Value: component specification.
    /// </summary>
    internal readonly Dictionary<string, ComponentSpec> _components;

    internal LogicalView()
    {
        _dataAccess = new();
        _tenants = new();
        _components = new();
    }

    internal LogicalView(LogicalView original)
    {
        _dataAccess = new Dictionary<Type, string>(original._dataAccess);
        _tenants = new Dictionary<string, TenantConfig>(original._tenants);
        _components = new Dictionary<string, ComponentSpec>(original._components);
    }

    public LogicalView AddDataAccess<TDataAccess>(string tenantName)
        where TDataAccess : class
    {
        _dataAccess.Add(typeof(TDataAccess), tenantName);
        return this;
    }

    public ITenantSpecBuilder<TTenant> AddTenant<TTenant>(string tenantName, string componentName)
    {
        _tenants.Add(tenantName, new TenantConfig(typeof(TTenant), componentName, null));
        return new TenantSpecBuilder<TTenant>(this, tenantName);
    }

    public void AddComponent<TComponent>(string componentName)
    {
        _components.Add(componentName, new ComponentSpec(typeof(TComponent)));
    }

    private class TenantSpecBuilder<TTenant>(LogicalView logicalView, string tenantName)
        : ITenantSpecBuilder<TTenant>
    {
        public ITenantSpecBuilder<TTenant> WithDataSource<TDataSource>(TDataSource dataSource)
        {
            logicalView._tenants[tenantName] = logicalView._tenants[tenantName] with { DataSource = dataSource };
            return this;
        }
    }
}