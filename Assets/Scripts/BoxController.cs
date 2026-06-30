using System;
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
        Box box = BoxRepository.Instance.GetFirstBox();

        if (box == null)
            return;

        if (box.targetArea.direction == e.direction)
            MoveBoxToTarget(box);
        else
            EventBus<GameOverEvent>.Publish(new GameOverEvent());
    }

    private void MoveBoxToTarget(Box box)
    {
        box.gameObject.transform.position = box.targetArea.transform.position;
    }
}
