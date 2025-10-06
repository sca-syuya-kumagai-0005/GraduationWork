using UnityEngine;

public class GameStateSystem : MonoBehaviour
{
    public enum State
    {
        Start,
        Wait,
        DeliveryPreparation,
        Route,
        End,
    }
    private State gameState;
    public State GameState {  get { return gameState; } set { gameState = value; } }
    private void Awake()
    {
        gameState = State.Start;
    }
}
