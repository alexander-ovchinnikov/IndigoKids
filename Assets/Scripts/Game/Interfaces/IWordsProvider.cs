namespace Game.Interfaces
{
    public interface IWordsProvider
    {
        string GetNextWord();
        void Reset();
    }
}