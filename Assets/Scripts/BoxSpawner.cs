using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;
    [SerializeField] int minBoxCountInAStack, maxBoxCountInAStack;
    EventBinding<GameStartedEvent> gameStartedBinding;

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
            int randomBoxColorValue = Random.Range(0, System.Enum.GetValues(typeof(ColorType)).Length);

            box.colorType = (ColorType)randomBoxColorValue;

            box.SetMaterialColor(box.colorType);

            foreach (BoxTargetArea targetArea in BoxRepository.Instance.targetAreas)
            {
                if(box.colorType == targetArea.colorType)
                {
                    box.targetArea = targetArea;
                    box.direction = targetArea.direction;
                    break;
                }
            }

            return box;
        }

        return null;
    }
}
