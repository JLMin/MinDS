namespace MinDS.DirectedGraph;

public struct Edge<V, E> where V : struct where E : struct
{
    public V Source { get; }

    public V Destination { get; }

    public E Value { get; }

    public Edge(V source, V dest, E value)
    {
        Source = source;
        Destination = dest;
        Value = value;
    }

    public Edge<V, E> Clone()
    {
        return new Edge<V, E>(this.Source, this.Destination, this.Value);
    }
}
