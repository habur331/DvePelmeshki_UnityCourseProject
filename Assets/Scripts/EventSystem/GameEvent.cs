public static class GameEvent
{
    public static readonly string PlayerDied = $"{nameof(PlayerDied)}";
    public static readonly string BombPlanted = $"{nameof(BombPlanted)}";
    public static readonly string BombDiffused = $"{nameof(BombDiffused)}";
    public static readonly string BombBlewUp = $"{nameof(BombBlewUp)}";
    public static readonly string PauseStateChanged = $"{nameof(PauseStateChanged)}";
}