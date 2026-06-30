using UnityEngine;

public class BoxController : MonoBehaviour
{
    EventBinding<MoveClickedEvent> moveClickedBinding;

    private void OnEnable()
    {
        moveClickedBinding = new EventBinding<MoveClickedEvent>(HandleMoveClicked);

        EventBus<MoveClickedEvent>.Subscribe(moveClickedBinding);
    }

    private void OnDisable()
    {
        EventBus<MoveClickedEvent>.Unsubscribe(moveClickedBinding);
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void HandleMoveClicked(MoveClickedEvent e)
    {
        Box box = BoxManager.Instance.GetFirstBox();

        if (box == null)
            return;

        if (e.direction == MoveDirection.Right)
        {
            if (box.colorType == ColorType.blue || box.colorType == ColorType.green)
            {
                box.transform.position = box.targetArea.transform.position;
            }
            else
            {
                EventBus<GameOverEvent>.Publish(new GameOverEvent());
            }
        }
        if(e.direction == MoveDirection.Left)
        {
            if (box.colorType == ColorType.red || box.colorType == ColorType.yellow)
            {
                box.transform.position = box.targetArea.transform.position;
            }
            else
            {
                EventBus<GameOverEvent>.Publish(new GameOverEvent());
            }

        }
    }
}
