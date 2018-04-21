using balloon.Selectors;

namespace balloon
{
    public class Program
    {
        static void Main()
        {
            // var selector = new ManualSelector();
            // var selector = new AutomaticSelector();
            var selector = new SimplifiedAutomaticSelector();

            selector.Run();
        }
    }
}
