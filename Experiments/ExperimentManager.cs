using System;
using System.Collections.Generic;
using System.Reflection;

namespace OWMiniature.Experiments
{
    /// <summary>
    /// Script holding all the different experiments we may wanna try out, but not scrap!
    /// </summary>
    public class ExperimentManager
    {
        public List<ExperimentBase> Experiments { get; } = new List<ExperimentBase>();

        /// <summary>
        /// Default constructor.
        /// 
        /// <para>Fetches and initializes all experiments in the project.</para>
        /// </summary>
        public ExperimentManager()
        {
            Experiments.Clear();
            Type[] assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type item in assemblyTypes)
            {
                if (!item.IsSubclassOf(typeof(ExperimentBase)) || item.IsAbstract)
                    continue;

                object instance = Activator.CreateInstance(item);

                if (instance is not ExperimentBase experiment)
                    continue;

                Experiments.Add(experiment);
            }
        }

        public void Setup()
        {
            foreach (ExperimentBase experiment in Experiments)
            {
                if (!experiment.IsEnabled)
                    continue;

                experiment.Enable();
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
