using ED262C;
using System;
using System.Collections.Generic;
using UnityEngine; //Delete when not using debugLog
public class BST<T> where T : IComparable
{
    int count = 0;
    public int Count { get => count; }
    TreeNode<T> root = null;
    public int height { get => GetNodeHeight(root); }
    public int BañlanceFactor;

    public bool ISEmpty { get => count == 0; }
    public void Insert(T value)
    {
        root = InsertRecursive(root, value);
    }

    protected virtual TreeNode<T> InsertRecursive(TreeNode<T> current, T value)
    {
        if (current == null)
        {
            count++;
            return new TreeNode<T>(value);
        }

        int comparison = value.CompareTo(current.Value);

        if (comparison < 0)
        {
            current.Left = InsertRecursive(current.Left, value);
        }

        if (comparison < 0)
        {
            current.Right = InsertRecursive(current.Right, value);
        }
        return current;
    }

    public void Remove(T value)
    {
        root = RemoveRecursive(root, value);
    }

    protected virtual TreeNode<T> RemoveRecursive(TreeNode<T> current, T value)
    {
        if (current == null) { return null; }
        int comparison = value.CompareTo(current.Value);

        if (comparison == 0)
        { 
            count--;

            if (current.Left == null && current.Right == null)
                return null;
            if (current.Right == null)
                return current.Left;
            if (current.Left == null)
                return current.Right;
            else
            {
                TreeNode<T> successor = GetMinChild(current.Right);
                current.Value = successor.Value;
                current.Right = RemoveRecursive(current.Right, successor.Value);
                //compensates the extra count-- that gets executed as we remove the successor
                count++;
            }
        }

        if (comparison < 0)
        {
            current.Left = InsertRecursive(current.Left, value);
        }

        if (comparison < 0)
        {
            current.Right = InsertRecursive(current.Right, value);
        }
        return current;
    }

    TreeNode<T> GetMinChild(TreeNode<T> current)
    {
        while (current.Left != null)
            current = current.Left;
        return current;

    }
    public bool Contains(T value)
    {

        return ContainsRecursive(root, value);

    }
    private bool ContainsRecursive(TreeNode<T> current, T value)
    {
        if (current == null) return false;
        int comparisson = value.CompareTo(current.Value);

        if (comparisson < 0)
            return ContainsRecursive(current.Left, value);


        if (comparisson > 0)
            return ContainsRecursive(current.Right, value);

        return true;
    }

    protected int GetNodeHeight(TreeNode<T> node)
    {
        if (node == null) return -1;
        return 1 + Math.Max(GetNodeHeight(node.Left), GetNodeHeight(node.Right));
    }

    protected int GetNodeBalanceFactor(TreeNode<T> node)
    {
        return GetNodeHeight(node.Left) - GetNodeHeight(node.Right);
    }

    public void PreOrder()
    {
        PreOrderRecursive(root);
    }

    //Example Functions, rn they only print Value, but can be changed to do actually useful stuff

    void PreOrderRecursive(TreeNode<T> current)
    {
        if (current == null)
            return;
        Debug.Log(current.Value.ToString());
        PreOrderRecursive(current.Left);
        PreOrderRecursive(current.Right);
    }

    void InOrderRecursive(TreeNode<T> current)
    {
        InOrderRecursive(current.Left);
        Debug.Log(current.Value.ToString());
        InOrderRecursive(current.Right);
    }
    void PostOrderRecursive(TreeNode<T> current)
    {
        PostOrderRecursive(current.Left);
        PostOrderRecursive(current.Right);
        Debug.Log(current.Value.ToString());
    }

    public ISimpleList<T> LevelOrder()
    {
        ISimpleList<T> result = new SimpleArrayList<T>(Count);
        SimpleLinkedQueue<TreeNode<T>> queue = new();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            TreeNode<T> current = queue.Dequeue();
            result.Add(current.Value);
            queue.Enqueue(current.Left);
            queue.Enqueue(current.Right);
        }
        return result;
    }

    public IEnumerable<T> InOrder()
    {
        var stack = new Stack<TreeNode<T>>();
        TreeNode<T> cur = root;
        while (cur != null || stack.Count > 0)
        {
            while (cur != null) { stack.Push(cur); cur = cur.Left; }
            cur = stack.Pop();
            yield return cur.Value;
            cur = cur.Right;
        }
    }
}
