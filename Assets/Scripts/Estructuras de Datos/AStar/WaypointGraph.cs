using System.Collections.Generic;
using UnityEngine;
using Demonics;
using ED262C;

public class WaypointGraph : MonoBehaviour
{
    public static WaypointGraph Instance { get; private set; }

    public const int CostScale = 10; // misma escala para el costo y la heurística

    [SerializeField] private int columns = 20;   // cantidad de nodos en X
    [SerializeField] private int rows = 20;      // cantidad de nodos en Z
    [SerializeField] private float spacing = 2f; // distancia entre nodos
    [SerializeField] private LayerMask obstacleMask; // layers de paredes/obstáculos

    public IGraph<GraphNode<Vector3>> Graph { get; private set; }

    private List<GraphNode<Vector3>> allNodes = new List<GraphNode<Vector3>>();

    private void Awake()
    {
        Instance = this;
        BuildGraph();
    }

    private void BuildGraph()
    {
        Graph = new ListGraph<GraphNode<Vector3>>();

        // Matriz temporal para encontrar fácil al vecino de cada nodo
        GraphNode<Vector3>[,] grid = new GraphNode<Vector3>[columns, rows];

        // PASO 1: crear un nodo por cada celda (si no hay un obstáculo)
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 pos = transform.position + new Vector3(x * spacing, 0, z * spacing);

                // Si choca con un obstáculo, dejamos la celda vacía (null)
                if (Physics.CheckSphere(pos + Vector3.up, 0.5f, obstacleMask))
                    continue;

                GraphNode<Vector3> node = new GraphNode<Vector3>(pos);
                grid[x, z] = node;
                allNodes.Add(node);
                Graph.AddVertex(node);
            }
        }

        // PASO 2: conectar cada nodo con el de la derecha y el de arriba
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                GraphNode<Vector3> current = grid[x, z];
                if (current == null) continue;

                // Vecino de la derecha (x + 1)
                if (x + 1 < columns && grid[x + 1, z] != null)
                    Connect(current, grid[x + 1, z]);

                // Vecino de arriba (z + 1)
                if (z + 1 < rows && grid[x, z + 1] != null)
                    Connect(current, grid[x, z + 1]);
            }
        }
    }

    // Une dos nodos en los dos sentidos
    private void Connect(GraphNode<Vector3> a, GraphNode<Vector3> b)
    {
        int cost = Mathf.RoundToInt(Vector3.Distance(a.Value, b.Value) * CostScale);
        Graph.AddEdge(a, b, cost);
        Graph.AddEdge(b, a, cost);
    }

    // Devuelve el nodo más cercano a una posición
    public GraphNode<Vector3> GetClosestNode(Vector3 pos)
    {
        GraphNode<Vector3> closest = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < allNodes.Count; i++)
        {
            float d = Vector3.Distance(allNodes[i].Value, pos);
            if (d < minDistance)
            {
                minDistance = d;
                closest = allNodes[i];
            }
        }
        return closest;
    }

    // Dibuja los nodos en la Scene (solo para verlos)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < allNodes.Count; i++)
            Gizmos.DrawSphere(allNodes[i].Value, 0.2f);
    }
}