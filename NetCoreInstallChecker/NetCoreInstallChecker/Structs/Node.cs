using System.Collections.Generic;

namespace NetCoreInstallChecker.Structs;

/// <summary>
/// Represents an individual node within a graph.
/// </summary>
public class Node<T>
{
    /// <summary>
    /// Declares if this node has been visited or not.
    /// </summary>
    public Mark Visited = Mark.NotVisited;
        
    /// <summary>
    /// The individual element assigned to this node.
    /// </summary>
    public T Element;

    /// <summary>
    /// Stores the child (dependency) of the current node.
    /// </summary>
    public List<Node<T>> Edges = new();

    public Node(T element)
    {
        Element = element;
    }

    public static implicit operator Node<T>(T value) => new(value);
}

public enum Mark
{
    NotVisited,
    Visiting,
    Visited
}