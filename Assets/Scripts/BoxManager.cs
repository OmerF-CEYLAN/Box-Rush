using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public List<Stack<Box>> boxStacks = new List<Stack<Box>>();
    public List<BoxTargetArea> targetAreas = new List<BoxTargetArea>();
    public List<Box> boxesToBeDestroyed = new List<Box>();

    public static BoxManager Instance;
    EventBinding<NewBoxStackSpawnedEvent> newBoxStackSpawnedBinding;

    EventBinding<GameOverEvent> gameOverBinding;

    private void OnEnable()
    {
        newBoxStackSpawnedBinding = new EventBinding<NewBoxStackSpawnedEvent>(HandleNewBoxesSpawned);
        EventBus<NewBoxStackSpawnedEvent>.Subscribe(newBoxStackSpawnedBinding);

        gameOverBinding = new EventBinding<GameOverEvent>(HandleGameOver);
        EventBus<GameOverEvent>.Subscribe(gameOverBinding);
    }

    private void OnDisable()
    {
        EventBus<NewBoxStackSpawnedEvent>.Unsubscribe(newBoxStackSpawnedBinding);
        EventBus<GameOverEvent>.Unsubscribe(gameOverBinding);
    }

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

    void Start()
    {

    }

    void Update()
    {
        
    }

    void HandleGameOver()
    {
        ClearBoxes();
    }

    void ClearBoxes()
    {
        List<Box> boxes = boxesToBeDestroyed;

        foreach (Box box in boxesToBeDestroyed)
        {
            Destroy(box.gameObject);
        }

        boxesToBeDestroyed.Clear();
        boxStacks.Clear();
    }

    void HandleNewBoxesSpawned(NewBoxStackSpawnedEvent e)
    {
        boxStacks.Add(e.boxes);
        
        foreach (Box box in e.boxes)
        {
            boxesToBeDestroyed.Add(box);
        }

    }

    public Box GetFirstBox()
    {
        if(boxStacks.Count == 0)
            return null;

        Stack<Box> firstStack = boxStacks.First();

        if(firstStack == null || firstStack.Count == 0)
            return null;

        Box box =  firstStack.Pop();

        return box;
    }
}
