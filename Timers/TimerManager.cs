using System.Collections.Generic;
using System.Linq;

namespace PhenomTools
{
    public static class TimerManager
    {
        public static List<TimeKeeper> activeTimeKeepers { get; private set; } = new List<TimeKeeper>();

        public static void RegisterNewTimer(TimeKeeper timer)
        {
            CoroutineHolder.StartCoroutine(timer.keeperCoroutine);

            if(!activeTimeKeepers.Contains(timer))
                activeTimeKeepers.Add(timer);

            //UpdateActiveTimersList();
        }

        public static void RemoveTimer(TimeKeeper timer)
        {
            if (timer.isRunning)
                timer.Stop();

            activeTimeKeepers.Remove(timer);
        }

        //public static void UpdateActiveTimersList()
        //{
        //    activeTimeKeepers = activeTimeKeepers.OrderBy(t => t.currentTime).ToList();
        //}
    }
}
