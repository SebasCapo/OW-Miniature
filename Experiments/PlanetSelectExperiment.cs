using OWMiniature.Gameplay.Lines;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Experiments
{
    public class PlanetSelectExperiment : ExperimentBase
    {
        /// <inheritdoc />
        public override bool IsEnabled => false;

        /// <inheritdoc />
        internal override void Enable()
        {
            base.Enable();

            GlobalMessenger<ReferenceFrame>.AddListener(EventUtils.TargetReferenceFrame, OnTargetSelect);
            GlobalMessenger.AddListener(EventUtils.ExitMapView, Trigger);
        }

        /// <inheritdoc />
        internal override void Disable()
        {
            base.Disable();

            GlobalMessenger<ReferenceFrame>.RemoveListener(EventUtils.TargetReferenceFrame, OnTargetSelect);
            GlobalMessenger.RemoveListener(EventUtils.ExitMapView, Trigger);
        }

        /// <inheritdoc />
        public override void Trigger()
        {
            // Empty, we handle everything through events.
        }

        private void OnTargetSelect(ReferenceFrame frame)
        {
            if (frame == null)
                return;

            //if (!MapUtils.Lines.TryGetValue(frame, out PlanetaryLineBase selector))
            //{
            //    GameObject lineObj = new GameObject("Custom OrbitLine");
            //    AstroObject astroObject = frame._attachedAstroObject;
            //    Transform lineTransform = lineObj.transform;

            //    lineTransform.SetParent(astroObject.transform);
            //    lineTransform.localPosition = Vector3.zero;

            //    selector = lineObj.AddComponent<SelectionLine>();
            //    selector.Assign(astroObject.transform, frame);
            //}

            //OWMiniature.Console.WriteLine($"{selector.name} has a selector line!", OWML.Common.MessageType.Success);
        }
    }
}
