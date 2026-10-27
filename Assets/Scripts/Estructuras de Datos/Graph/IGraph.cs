using ED262C;
namespace Demonics
{
    public interface IGraph<T>
    {
        public T[] Vertices { get; }
        public bool AddVertex(T Vertex);
        public bool RemoveVertex(T Vertex);
        public bool AddEdge(T from, T to, int cost);
        public bool RemoveEdge(T from, T to);
        public ISimpleList<Edge<T>> GetNeighbors(T vertex);
        public bool ContainsVertex(T vertex);
        public bool ContainsEdge(T form, T to);
        public int GetWeight(T from, T to);
    }
}
