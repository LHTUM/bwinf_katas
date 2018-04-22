namespace balloon.Selectors
{
    public interface ISelector
    {
        void Run();
        int ProvideBox();
        bool IsFinished();
        void SetBalloonMachine(BalloonMachine balloonMachine);
    }
}
