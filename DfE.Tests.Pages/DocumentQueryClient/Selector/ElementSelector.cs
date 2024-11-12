﻿namespace DfE.Tests.Pages.DocumentQueryClient.Selector;

public sealed class ElementSelector : IQuerySelector
{
    private readonly string _locator;

    public ElementSelector(string locator)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(locator, nameof(locator));
        _locator = locator;
    }

    public string ToSelector() => _locator;
    public override string ToString() => ToSelector();
}
