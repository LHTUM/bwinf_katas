namespace balloon.Selectors
{
    public interface ISelector
    {
        void ExecuteStep();
        bool IsFinished();
        void SetBalloonMachine(BalloonMachine balloonMachine);
    }
}
