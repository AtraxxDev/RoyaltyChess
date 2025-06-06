public enum BoardType
{
    Default,
    TableroGlacial,
    TablerodeLava,
    TableroMedieval,
    TableroVerde
}


public interface IBoardApplier
{
    void ApplyBoard(BoardData boardData);
}
