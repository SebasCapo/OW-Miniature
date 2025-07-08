namespace OWMiniature.Experiments
{
    /// <summary>
    /// Base script for all experiments.
    /// </summary>
    public abstract class ExperimentBase
    {
        /// <summary>
        /// Indicates whether this experiment is enabled. <para/>
        /// 
        /// Generally used to keep experiments available in code but disabled.
        /// </summary>
        public virtual bool IsEnabled => true;

        /// <summary>
        /// Allows us to define any setups the experiment may need.
        /// </summary>
        internal virtual void Enable()
        {
        }

        /// <summary>
        /// Allows us to get rid of anything the experiment was using.
        /// </summary>
        internal virtual void Disable()
        {
        }

        /// <summary>
        /// Simple enough, triggers whatever this experiment holds!
        /// </summary>
        public abstract void Trigger();
    }
}
