using Demonics;
using ED262C;
using System;
using UnityEngine;
public class AStar<T>
{
    //F(n) = g(n) + h(n)

    public ISimpleList<T> GetShortesPath(GraphNode<T> origin, GraphNode<T> destination, IGraph<GraphNode<T>> graph, Func<GraphNode<T>, GraphNode<T>, float> heuristic)
    {
        ISimpleDictionary<GraphNode<T>, Edge<GraphNode<T>>> table = new SimpleArrayDictionary<GraphNode<T>, Edge<GraphNode<T>>>();
        PriorityQueue<GraphNode<T>> unvisited = new();
        SimpleArraySet<GraphNode<T>> visited = new();

        GraphNode<T>[] vertices = graph.Vertices;

        for (int i = 0; i < vertices.Length; i++)
            table.Add(vertices[i], new Edge<GraphNode<T>>(null, int.MaxValue));

        table[origin].cost = 0;

        unvisited.Enqueue(origin, (int)heuristic(origin, destination)); // F = 0 + h
 
        while (!unvisited.IsEmpty)
        {
            GraphNode<T> current = unvisited.Dequeue();

            if (visited.Contains(current)) continue;

            if (current == destination) break;

            ISimpleList<Edge<GraphNode<T>>> neighbors = graph.GetNeighbors(current);

            for (int i = 0; i < neighbors.Count; i++)
            {
                GraphNode<T> next = neighbors[i].to;

                if (visited.Contains(next)) continue;
                    
                int newCost = table[current].cost + neighbors[i].cost; // g nuevo

                if (newCost < table[neighbors[i].to].cost)
                {
                    table[next].cost = newCost; // g
                    table[next].to = current; // nodo padre

                    unvisited.Enqueue(next, newCost + (int)heuristic(next, destination));
                }

                //if (!visited.Contains(neighbors[i].to))
                //    unvisited.Enqueue(neighbors[i].to, newCost);
            }

            visited.Add(current);
        }

        if (table[destination].cost == int.MaxValue) return null;

        ISimpleList<GraphNode<T>> nodePath = GeneratePath(destination, table);
        
        ISimpleList<T> result = new SimpleArrayList<T>(nodePath.Count);

        for (int i = 0; i < nodePath.Count; i++)
            result.Add(nodePath[i].Value);

        return result;
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
