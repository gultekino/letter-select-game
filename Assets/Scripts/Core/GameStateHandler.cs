using System;

public static class GameStateHandler
{
    private static GameState currentGameState = GameState.PreGame;
    public static event Action<GameState> OnGameStateChange;

    public static GameState GameState
    {
        get => currentGameState;
        set
        {
            if (currentGameState == value) return;

            currentGameState = value;
            OnGameStateChange?.Invoke(currentGameState);
        }
    }

}

