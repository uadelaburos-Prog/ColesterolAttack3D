using ED262C;
namespace Demonics
{
    public class DijkstaSolver<T> : IPathFindingSolver<T>
    {
        public ISimpleDictionary<GraphNode<T>, Edge<GraphNode<T>>> GetAllShortesPaths(GraphNode<T> origin, IGraph<GraphNode<T>> graph)
        {
            ISimpleDictionary<GraphNode<T>, Edge<GraphNode<T>>> result = new SimpleArrayDictionary<GraphNode<T>, Edge<GraphNode<T>>>();
            PriorityQueue<GraphNode<T>> unvisited = new();
            SimpleArraySet<GraphNode<T>> visited = new();

            GraphNode<T>[] vertices = graph.Vertices;

            for (int i = 0; i < vertices.Length; i++)
                result.Add(vertices[i], new Edge<GraphNode<T>>(null, int.MaxValue));

            result[origin].cost = 0;

            unvisited.Enqueue(origin, result[origin].cost);

            while (!unvisited.IsEmpty)
            {
                GraphNode<T> current = unvisited.Dequeue();

                if (visited.Contains(current)) continue;

                ISimpleList<Edge<GraphNode<T>>> neighbors = graph.GetNeighbors(current);

                for (int i = 0; i < neighbors.Count; i++)
                {
                    int totalCost = result[current].cost + result[origin].cost;

                    if (totalCost < result[neighbors[i].to].cost)
                        result[neighbors[i].to].cost = totalCost;
                }

                visited.Add(current);
            }

            return result;
        }

        public ISimpleList<T> GetShortesPath(GraphNode<T> origin, GraphNode<T> destination, IGraph<GraphNode<T>> graph)
        {
            ISimpleDictionary<GraphNode<T>, Edge<GraphNode<T>>> table = new SimpleArrayDictionary<GraphNode<T>, Edge<GraphNode<T>>>();
            PriorityQueue<GraphNode<T>> unvisited = new();
            SimpleArraySet<GraphNode<T>> visited = new();

            GraphNode<T>[] vertices = graph.Vertices;

            for (int i = 0; i < vertices.Length; i++)
                table.Add(vertices[i], new Edge<GraphNode<T>>(null, int.MaxValue));

            table[origin].cost = 0;

            unvisited.Enqueue(origin, table[origin].cost);

            while (!unvisited.IsEmpty)
            {
                GraphNode<T> current = unvisited.Dequeue();

                if (current == destination) break;

                if (visited.Contains(current)) continue;

                ISimpleList<Edge<GraphNode<T>>> neighbors = graph.GetNeighbors(current);

                for (int i = 0; i < neighbors.Count; i++)
                {
                    int totalCost = table[current].cost + table[origin].cost;

                    if (totalCost < table[neighbors[i].to].cost)
                        table[neighbors[i].to].cost = totalCost;
                }

                visited.Add(current);
            }

            if (table[destination] == null) return null;

            return (ISimpleList<T>)GeneratePath(destination, table);
        }

        ISimpleList<GraphNode<T>> GeneratePath(GraphNode<T> destination, ISimpleDictionary<GraphNode<T>, Edge<GraphNode<T>>> table)
        {
            ISimpleStack<GraphNode<T>> reversePath = new SimpleLinkedStack<GraphNode<T>>();

            GraphNode<T> current = destination;

            while (current != null)
            {
                reversePath.Push(current);

                current = table[current].to;
            }

            ISimpleList<GraphNode<T>> path = new SimpleArrayList<GraphNode<T>>(reversePath.Count);

            while (!reversePath.IsEmpty)
                path.Add(reversePath.Pop());

            return path;
        }
    }
}