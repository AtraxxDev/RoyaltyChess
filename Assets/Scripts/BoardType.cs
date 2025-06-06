public enum BoardType
{
    Default,
    TableroGlacial,
    TableroDeLava,
    TableroMedieval,
    TableroVerde
}


public interface IBoardApplier
{
    void ApplyBoard(BoardData boardData);
}
