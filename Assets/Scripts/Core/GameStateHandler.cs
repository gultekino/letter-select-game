using System;

public struct GameStateChanged : IEvent
{
    public GameState gameState;
}
public static class GameStateHandler
{
    private static GameState currentGameState = GameState.PreGame;
    public static GameState GameState
    {
        get => currentGameState;
        set
        {
            if (currentGameState == value) return;

            currentGameState = value;
            EventBus<GameStateChanged>.Raise(new GameStateChanged(){gameState = currentGameState});
        }
    }

}

