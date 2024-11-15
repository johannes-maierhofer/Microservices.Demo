﻿using Argo.MD.BuildingBlocks.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Argo.MD.BuildingBlocks.Core.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
    ) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(
            queryable.AsNoTracking(),
            pageNumber,
            pageSize,
            cancellationToken);
}
