namespace JasperFx.Core;

/// <summary>
/// Create complex allow and/or deny list logical sets
/// </summary>
/// <typeparam name="T"></typeparam>
public class CompositeFilter<T>
{
    /// <summary>
    /// If empty, this includes everything. Use this for allow filter
    /// </summary>
    public CompositePredicate<T> Includes { get; set; } = new();
    
    /// <summary>
    /// If empty, this has no effect. Use this for deny filters
    /// </summary>
    public CompositePredicate<T> Excludes { get; set; } = new();

    /// <summary>
    /// Does the item meet all the include and exclude criteria?
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool Matches(T target)
    {
        return Includes.MatchesAny(target) && Excludes.DoesNotMatchAny(target);
    }
}

public class CompositePredicate<T>
{
    private readonly List<Func<T, bool>> _list = new();
    private Func<T, bool> _matchesAll = _ => true;
    private Func<T, bool> _matchesAny = _ => true;
    private Func<T, bool> _matchesNone = _ => false;

    public void Add(Func<T, bool> filter)
    {
        _matchesAll = x => _list.All(predicate => predicate(x));
        _matchesAny = x => _list.Any(predicate => predicate(x));
        _matchesNone = x => !MatchesAny(x);

        _list.Add(filter);
    }

    public static CompositePredicate<T> operator +(CompositePredicate<T> invokes, Func<T, bool> filter)
    {
        invokes.Add(filter);
        return invokes;
    }

    public bool MatchesAll(T target)
    {
        return _matchesAll(target);
    }

    public bool MatchesAny(T target)
    {
        return _matchesAny(target);
    }

    public bool MatchesNone(T target)
    {
        return _matchesNone(target);
    }

    public bool DoesNotMatchAny(T target)
    {
        return _list.Count == 0 || !MatchesAny(target);
    }
}
