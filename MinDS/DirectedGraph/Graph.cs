namespace MinDS.DirectedGraph;

public class Graph<V, E> where V : struct where E : struct
{
    private readonly Dictionary<V, Vertice<V>> vertices;

    private readonly Dictionary<Tuple<V, V>, HashSet<Edge<V, E>>> edges;

    public List<Vertice<V>> Vertices => vertices.Values.ToList();

    public List<Edge<V, E>> Edges => edges.Values.SelectMany(v => v).ToList();

    public int VerticeCount => Vertices.Count;

    public int EdgeCount => Edges.Count;

    public Graph()
    {
        vertices = new Dictionary<V, Vertice<V>>();
        edges = new Dictionary<Tuple<V, V>, HashSet<Edge<V, E>>>();
    }

    public Graph<V, E> Clone()
    {
        var newGraph = new Graph<V, E>();
        Vertices.ForEach(v => newGraph.AddVertice(v.Clone()));
        Edges.ForEach(e => newGraph.AddEdge(e.Clone()));
        return newGraph;
    }

    public void ResetVisited()
    {
        Vertices.ForEach(v => v.IsVisited = false);
    }

    public void ClearSelfCycle()
    {
        Edges.Where(e => e.Source.Equals(e.Destination)).ToList().ForEach(e => RemoveEdge(e));
    }

    #region Vertices

    public bool ContainsVertice(V vertice)
    {
        return vertices.ContainsKey(vertice);
    }

    public Vertice<V> GetVertice(V target)
    {
        vertices.TryGetValue(target, out Vertice<V> vertice);
        return vertice;
    }

    private Vertice<V> GetOrAddVertice(V vertice)
    {
        if (vertices.TryGetValue(vertice, out Vertice<V> target))
        {
            return target;
        }
        var newVertice = new Vertice<V>(vertice);
        AddVertice(newVertice);
        return newVertice;
    }

    public void AddVertice(V vertice)
    {
        vertices.TryAdd(vertice, new Vertice<V>(vertice));
    }

    private void AddVertice(Vertice<V> vertice)
    {
        vertices.TryAdd(vertice.Value, vertice);
    }

    public void RemoveVertice(V vertice)
    {
        if (vertices.TryGetValue(vertice, out Vertice<V> target))
        {
            target.Parents.ToList().ForEach(p => p.Children.Remove(target));
            target.Children.ToList().ForEach(c => c.Parents.Remove(target));
            RemoveEdgeFrom(vertice);
            RemoveEdgeTo(vertice);
            vertices.Remove(vertice);
        }
    }

    public void RemoveVertice(Vertice<V> vertice)
    {
        RemoveVertice(vertice.Value);
    }

    #endregion

    #region Edge

    public bool ContainsEdge(V source, V dest)
    {
        return edges.ContainsKey(new Tuple<V, V>(source, dest));
    }

    public HashSet<Edge<V, E>>? GetEdges(V source, V dest)
    {
        edges.TryGetValue(new Tuple<V, V>(source, dest), out HashSet<Edge<V, E>>? edgeSet);
        return edgeSet;
    }

    public void AddEdge(V source, V dest, E data)
    {
        var parent = GetOrAddVertice(source);
        var child = GetOrAddVertice(dest);
        parent.Children.Add(child);
        child.Parents.Add(parent);
        var key = new Tuple<V, V>(source, dest);
        if (!edges.ContainsKey(key))
        {
            edges.Add(key, new HashSet<Edge<V, E>>());
        }
        edges[key].Add(new Edge<V, E>(source, dest, data));
    }

    private void AddEdge(Edge<V, E> edge)
    {
        AddEdge(edge.Source, edge.Destination, edge.Value);
    }

    public void RemoveEdge(V source, V dest)
    {
        var key = new Tuple<V, V>(source, dest);
        if (edges.ContainsKey(key))
        {
            var parent = GetOrAddVertice(source);
            var child = GetOrAddVertice(dest);
            parent.Children.Remove(child);
            child.Parents.Remove(parent);
            edges.Remove(key);
        }
    }

    public void RemoveEdge(Edge<V, E> edge)
    {
        RemoveEdge(edge.Source, edge.Destination);
    }

    public void RemoveEdgeFrom(V source)
    {
        if (ContainsVertice(source))
        {
            GetOrAddVertice(source).Children.ToList().ForEach(c => RemoveEdge(source, c.Value));
        }
    }

    public void RemoveEdgeTo(V dest)
    {
        if (ContainsVertice(dest))
        {
            GetOrAddVertice(dest).Parents.ToList().ForEach(p => RemoveEdge(p.Value, dest));
        }
    }

    #endregion
}
