namespace TheseusAndMinotaur.Data
{
    public enum BoardStatus
    {
        Active, // board started, Theseus is alive, waiting for action
        GameOver, // Minotaur Caught Theseus
        Victory // Theseus reached the Exit
    }
}