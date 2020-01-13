using System.Collections.Generic;
using System.Linq;

namespace PhenomTools
{
    public static class TimerManager
    {
        public static List<TimeKeeperBase> activeTimeKeepers { get; private set; } = new List<TimeKeeperBase>();

        public static void RegisterNewTimer(TimeKeeperBase timer)
        {
            CoroutineHolder.StartCoroutine(timer.keeperCoroutine);

            if(!activeTimeKeepers.Contains(timer))
                activeTimeKeepers.Add(timer);

            UpdateActiveTimersList();
        }

        public static void RemoveTimer(TimeKeeperBase timer)
        {
            if (timer.isRunning)
                timer.Stop();

            activeTimeKeepers.Remove(timer);
        }

        public static void UpdateActiveTimersList()
        {
            activeTimeKeepers = activeTimeKeepers.OrderBy(t => t.currentTime).ToList();
        }
    }
}
