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
                experiment.Setup();
            }
        }

        public void TriggerExperiment<T>() where T : ExperimentBase
        {
            foreach (ExperimentBase experiment in Experiments)
            {
                if (experiment is not T)
                    continue;

                experiment.Trigger();
            }
        }
    }
}
