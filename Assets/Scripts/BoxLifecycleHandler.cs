using System.Collections.Generic;
using UnityEngine;

public class BoxLifecycleHandler : MonoBehaviour
{
    [SerializeField] private BoxRepository boxRepository;

    private List<Box> activeBoxes = new List<Box>();

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

    private void HandleNewBoxesSpawned(NewBoxStackSpawnedEvent e)
    {
        boxRepository.AddStack(e.boxes);

        foreach (Box box in e.boxes)
        {
            activeBoxes.Add(box);
        }
    }

    private void HandleGameOver()
    {
        ClearBoxes();
    }

    private void ClearBoxes()
    {
        foreach (Box box in activeBoxes)
        {
            if (box != null)
                Destroy(box.gameObject);
        }

        activeBoxes.Clear();
        boxRepository.ClearAllStacks();
    }
}