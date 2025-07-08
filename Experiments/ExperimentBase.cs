namespace OWMiniature.Experiments
{
    /// <summary>
    /// Base script for all experiments.
    /// </summary>
    public abstract class ExperimentBase
    {
        /// <summary>
        /// Allows us to define any setups the experiment may need.
        /// </summary>
        internal virtual void Setup()
        {
        }

        /// <summary>
        /// Allows us to get rid of anything the experiment was using.
        /// </summary>
        internal virtual void Exit()
        {
        }

        /// <summary>
        /// Simple enough, triggers whatever this experiment holds!
        /// </summary>
        public abstract void Trigger();
    }
}
