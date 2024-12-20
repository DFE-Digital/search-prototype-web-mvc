﻿using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;

namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels;

/// <summary>
/// A view model representation of search results defining view logic predicated on the status of the results.
/// </summary>
public sealed class SearchResults
{
    private List<Facet>? _facets;

    /// <summary>
    /// View model representation of the available facets.
    /// </summary>
    public List<Facet>? Facets {
        get => _facets;
        set => _facets = value?.OrderBy(facet => facet.Name).ToList();
    }
    /// <summary>
    /// View model representation of aggregated search results.
    /// </summary>
    public List<Establishment>? SearchItems { get; set; }

    /// <summary>
    /// Property determining whether we have at least one search result.
    /// </summary>
    public bool HasResults => SearchResultsCount >= 1;

    /// <summary>
    /// Property determining whether we have more than one search result.
    /// </summary>
    public bool HasMoreThanOneResult => SearchResultsCount > 1;

    /// <summary>
    /// Property determining the number of search results.
    /// </summary>
    public int SearchResultsCount => SearchItems?.Count ?? 0;

    /// <summary>
    /// Determines whether there are filters in the results
    /// </summary>
    public bool HasFilters => Facets?.Count > 0 ? true : false;

    /// <summary>
    /// View model to deal with pagination for search results.
    /// </summary>
    public Pagination? Pagination { get; set; }
}
