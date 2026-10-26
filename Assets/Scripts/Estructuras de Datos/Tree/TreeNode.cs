using UnityEngine;

public class TreeNode<T>
{
    public T Value;
    public TreeNode<T> Left;
    public TreeNode<T> Right;

    public TreeNode(T value) => Value = value;
}
