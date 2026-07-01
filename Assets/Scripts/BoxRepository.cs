using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IBoxReader
{
    public Box GetFirstBox();
    public IReadOnlyList<BoxTargetArea> TargetAreas { get; }
}

public interface IBoxWriter
{
    void AddStack(Stack<Box> stack);
    void ClearAllStacks();
    void RegisterTargetArea(BoxTargetArea area);
}

public class BoxRepository : MonoBehaviour, IBoxReader, IBoxWriter
{
    public static BoxRepository Instance;

    List<Stack<Box>> boxStacks = new List<Stack<Box>>();
    List<BoxTargetArea> targetAreas = new List<BoxTargetArea>();

    public IReadOnlyList<BoxTargetArea> TargetAreas => targetAreas;
    public IReadOnlyList<Stack<Box>> BoxStacks => boxStacks;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddStack(Stack<Box> stack)
    {
        boxStacks.Add(stack);
    }

    public void ClearAllStacks()
    {
        boxStacks.Clear();
    }

    public Box GetFirstBox()
    {
        if (boxStacks.Count == 0)
            return null;

        Stack<Box> firstStack = boxStacks.First();

        if (firstStack == null || firstStack.Count == 0)
            return null;

        return firstStack.Pop();
    }

    public void RegisterTargetArea(BoxTargetArea area)
    {
        if (!targetAreas.Contains(area))
            targetAreas.Add(area);
    }
}
