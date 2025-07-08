using System.Collections.Generic;

namespace OWMiniature.Experiments
{
    /// <summary>
    /// Script holding all the different experiments we may wanna try out, but not scrap!
    /// </summary>
    public class ExperimentManager
    {
        public List<ExperimentBase> Experiments { get; } =
        [
            new MapNotificationExperiment(),
        ];

        public void Setup()
        {
            foreach (ExperimentBase experiment in Experiments)
            {
                if (!experiment.IsEnabled)
                    continue;

                experiment.Setup();
            }
        }

        /// <summary>
        /// Allows triggering a specific experiment from outside of them.
        /// </summary>
        /// <typeparam name="T">The specified experiment.</typeparam>
        /// <param name="allowDisabledExperiments">Whether this will trigger disabled experiments.</param>
        public void TriggerExperiment<T>(bool allowDisabledExperiments = false) where T : ExperimentBase
        {
            foreach (ExperimentBase experiment in Experiments)
            {
                if (experiment is not T)
                    continue;

                if (allowDisabledExperiments && !experiment.IsEnabled)
                    continue;

                experiment.Trigger();
            }
        }
    }
}
