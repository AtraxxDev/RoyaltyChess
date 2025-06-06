public enum BoardType
{
    Default,
    TableroGlacial,
    TableroDeLava,
    TableroReal,
    TableroVerde
}


public interface IBoardApplier
{
    void ApplyBoard(BoardData boardData);
}
