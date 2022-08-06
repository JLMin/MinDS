namespace MinDS.DirectedGraph;

public struct Vertice<V> where V : struct
{
    public V Value { get; }

    public HashSet<Vertice<V>> Parents { get; set; }

    public HashSet<Vertice<V>> Children { get; set; }

    public int InDegree => Parents.Count;

    public int OutDegree => Children.Count;

    public bool IsVisited { get; set; }

    public Vertice(V value)
    {
        Value = value;
        Parents = new HashSet<Vertice<V>>();
        Children = new HashSet<Vertice<V>>();
        IsVisited = false;
    }

    public Vertice<V> Clone()
    {
        return new Vertice<V>(Value);
    }
}
