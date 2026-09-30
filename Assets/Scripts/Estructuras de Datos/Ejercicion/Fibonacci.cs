using System.Data;
using UnityEngine;

public class Fibonacci : MonoBehaviour
{
    private void Start()
    {
        FibonacciSequence(30);
    }

    private void FibonacciSequence(int x)
    {
        int a = 0, b = 1,c = 0;
        for(int i = 0; i < x; i++)
        {
            c = a + b;
            print(c);
            a = b;
            b = c;
        }
    }
}
