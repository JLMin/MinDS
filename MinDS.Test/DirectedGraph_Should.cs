using MinDS.DirectedGraph;

namespace MinDS.Test;

[TestClass]
public class DirectedGraph_Should
{
    [TestMethod]
    public void Be_Empty_When_Created()
    {
        var graph = new Graph<int, int>();

        Assert.AreEqual(0, graph.EdgeCount);
        Assert.AreEqual(0, graph.VerticeCount);
    }
}
