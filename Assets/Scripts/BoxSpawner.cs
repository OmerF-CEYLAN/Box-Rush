using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;
    [SerializeField] int minBoxCountInAStack, maxBoxCountInAStack;
    [SerializeField] MonoBehaviour boxRepositorySource;
    [SerializeField] MonoBehaviour boxColorProviderSource;

    EventBinding<GameStartedEvent> gameStartedBinding;
    IBoxColorProvider boxColorProvider;
    IBoxReader boxReader;

    private void Awake()
    {
        boxReader = boxRepositorySource as IBoxReader;
        boxColorProvider = boxColorProviderSource as IBoxColorProvider;
    }

    private void OnEnable()
    {
        gameStartedBinding = new EventBinding<GameStartedEvent>(HandleGameStarted);
        EventBus<GameStartedEvent>.Subscribe(gameStartedBinding);
    }

    private void OnDisable()
    {
        EventBus<GameStartedEvent>.Unsubscribe(gameStartedBinding);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleGameStarted()
    {
        SpawnBoxStack();
    }

    void SpawnBoxStack()
    {
        Stack<Box> boxStack = new Stack<Box>();

        int randomCount = Random.Range(minBoxCountInAStack, maxBoxCountInAStack + 1);

        for (int i = 0; i < randomCount; i++)
        {
            Box newBox = SpawnRandomBox();
            boxStack.Push(newBox);

            Vector3 pos = newBox.transform.position;
            pos.y += i * 0.6f;

            newBox.transform.position = pos;
        }

        EventBus<NewBoxStackSpawnedEvent>.Publish(new NewBoxStackSpawnedEvent { boxes = boxStack });
    }

    Box SpawnRandomBox()
    {
        GameObject boxObject = Instantiate(boxPrefab);

        if(boxObject.TryGetComponent(out Box box))
        {
            box.colorType = boxColorProvider.GetRandomActiveColor();

            box.SetMaterialColor(box.colorType);

            foreach (BoxTargetArea targetArea in boxReader.TargetAreas)
            {
                if(box.colorType == targetArea.colorType)
                {
                    box.SetTargetArea(targetArea);
                    break;
                }
            }

            return box;
        }

        return null;
    }
}
