using System;
using UnityEngine;

public class AVL<T> : BST<T> where T : IComparable
{
    protected override TreeNode<T> InsertRecursive(TreeNode<T> current, T value)
    {
        return RebalanceNode(base.InsertRecursive(current, value));
    }

    protected override TreeNode<T> RemoveRecursive(TreeNode<T> current, T value)
    {
        return RebalanceNode(base.RemoveRecursive(current, value));
    }

    TreeNode<T> RebalanceNode(TreeNode<T> node)
    {
        int balanceFactor = GetNodeBalanceFactor(node);

        if (balanceFactor > 1)
        {
            if (GetNodeBalanceFactor(node.Left) >= 0)
                return RotateRight(node);
            else return RotateLeftRigth(node);
        }

        else if (balanceFactor < -1)
        {
            if (GetNodeBalanceFactor(node.Right) <= 0)
                return RotateLeft(node);
            else return RotateRightLeft(node);
        }
        return node;
    }

    TreeNode<T> RotateRight(TreeNode<T> y)
    {
        TreeNode<T> x = y.Left;
        TreeNode<T> t2 = x.Right;

        x.Right = y;
        y.Left = t2;

        return x;
    }
    TreeNode<T> RotateLeft(TreeNode<T> x)
    {
        TreeNode<T> y = x.Right;
        TreeNode<T> t2 = y.Left;

        y.Left = x;
        x.Right = t2;

        return y;
    }

    TreeNode<T> RotateRightLeft(TreeNode<T> node)
    {
        node.Right = RotateRight(node.Right);
        return RotateLeft(node);
    }

    TreeNode<T> RotateLeftRigth(TreeNode<T> node)
    {
        node.Left = RotateLeft(node.Left);
        return RotateRight(node);
    }
}
