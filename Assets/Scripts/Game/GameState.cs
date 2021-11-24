namespace TheseusAndMinotaur.Game
{
    public enum GameState
    {
        None,
        NewGameStarted,
        ListenUserInput,
        Active,
        HandleInput,
        GameOver,
        Victory,
        TerminatingCurrentLoop
    }
}