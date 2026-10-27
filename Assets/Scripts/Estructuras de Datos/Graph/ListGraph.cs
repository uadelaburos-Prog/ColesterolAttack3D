using ED262C;
using System;
namespace Demonics
{
    public class ListGraph<T> : IGraph<T>
    {
        SimpleArrayDictionary<T, SimpleArrayList<Edge<T>>> adjacentList;

        public ListGraph()
        {
            adjacentList = new SimpleArrayDictionary<T, SimpleArrayList<Edge<T>>>();
        }

        public T[] Vertices => adjacentList.Keys();

        public bool AddEdge(T from, T to, int cost)
        {
            AddVertex(from);
            AddVertex(to);

            Edge<T> edge = GetEdge(from, to);

            if(edge == null)
            {
                adjacentList[from].Add(new Edge<T>(to, cost));
                return true;
            }

            if(edge.cost != cost)
            {
                edge.cost = cost;
                return true;
            }

            return false;
        }

        public bool AddVertex(T Vertex)
        {
            //if(ContainsVertex(Vertex)) return false;
            //adjacentList.Add(Vertex, new SimpleArrayList<Edge<T>>());
            //return true;
            return adjacentList.TryAdd(Vertex, new SimpleArrayList<Edge<T>>());
        }

        public bool RemoveEdge(T from, T to)
        {
            throw new NotImplementedException();
        }

        public bool RemoveVertex(T Vertex)
        {
            if(!ContainsVertex(Vertex)) return false;

            for(int i = 0; i < Vertices.Length; i++)
            {
                if (Vertices[i].Equals(Vertex)) continue;
                Edge<T> targetEdge = GetEdge(Vertices[i], Vertex);

                // pa la siguiente clase
                //if(targetEdge != null) return true;
            }

            return adjacentList.Remove(Vertex);
        }

        public ISimpleList<Edge<T>> GetNeighbors(T vertex) 
        {
            if(!ContainsVertex(vertex)) return null;
            return adjacentList[vertex]; 
        }

        public int GetWeight(T from, T to)
        {
            Edge<T> targetEdge = GetEdge(from, to);

            if(targetEdge == null) return -1;

            return targetEdge.cost;
        }
        public bool ContainsVertex(T vertex) { return adjacentList.ContainsKey(vertex); }

        public bool ContainsEdge(T form, T to) { return GetEdge(form, to) != null; }

        Edge<T> GetEdge(T from, T to)
        {
            if (!adjacentList.ContainsKey(from)) return null;

            SimpleArrayList<Edge<T>> neighbors = adjacentList[from];

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (neighbors[i].to.Equals(to)) return neighbors[i];
            }

            return null;
        }
    }
}