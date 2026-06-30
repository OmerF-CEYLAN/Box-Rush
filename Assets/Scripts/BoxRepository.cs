using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxRepository : MonoBehaviour
{
    public static BoxRepository Instance;

    public List<Stack<Box>> boxStacks = new List<Stack<Box>>();
    public List<BoxTargetArea> targetAreas = new List<BoxTargetArea>();

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
}
