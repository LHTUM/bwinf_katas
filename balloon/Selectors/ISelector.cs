namespace balloon.Selectors
{
    public interface ISelector
    {
        void Run();
        int ProvideAction();
        bool IsFinished();
        void SetBalloonMachine(BalloonMachine balloonMachine);
    }
}
