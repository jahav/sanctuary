﻿using JetBrains.Annotations;

namespace DataIsland.SqlServer;

[PublicAPI]
public sealed record SqlServerSpec : ComponentSpec<SqlServerComponent>
{
    internal string? Collation { get; private set; }

    /// <summary>
    /// SQL Server must have a specified collation.
    /// </summary>
    /// <param name="collation">Desired collation name.</param>
    public SqlServerSpec WithCollation(string collation)
    {
        return new SqlServerSpec { Collation = collation };
    }
}
