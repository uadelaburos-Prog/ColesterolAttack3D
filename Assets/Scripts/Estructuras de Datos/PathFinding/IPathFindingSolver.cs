using ED262C;
namespace Demonics 
{
    public interface IPathFindingSolver<T>
    {
        public ISimpleDictionary<GraphNode<T>, Edge<GraphNode<T>>> GetAllShortesPaths(GraphNode<T> origin, IGraph<GraphNode<T>> graph);
        public ISimpleList<T> GetShortesPath(GraphNode<T> origin, GraphNode<T> destination, IGraph<GraphNode<T>> graph);
    }
}


