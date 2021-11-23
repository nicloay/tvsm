namespace TheseusAndMinotaur.Game
{
    public enum GameState
    {
        None,
        NewGameStarted,
        ListenUserInput,
        ActiveWithMovementOnScreen,
        GameOver,
        Victory,
        TerminatingCurrentLoop
    }
}