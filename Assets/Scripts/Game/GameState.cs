namespace TheseusAndMinotaur.WorldControllers
{
    /// <summary>
    ///     WorldGameManager run the core game, and use this State in internal state machine
    /// </summary>
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