using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Wrappers
{
    public class NeemVessel : MonoBehaviour
    {
        private const string VesselName = "Nomai Vessel";
        private const string BlackHoleName = "Gloaming Galaxy";
        private const float SupernovaDelay = 8f;

        public GameObject BlackHole 
        {
            get => _blackHole; 
            private set
            {
                _blackHole = value;
                _blackHoleTransform = value.transform;
            }
        }

        private readonly AnimationCurve _blackHoleScaleAnim = new AnimationCurve
            (
                new Keyframe(0, 0),
                new Keyframe(4f, 1),
                new Keyframe(SupernovaDelay, 0)
            );

        private GameObject _blackHole;
        private Transform _blackHoleTransform;
        private float _bhAnimTime;
        private bool _playAnim;

        public static void Generate()
        {
            GameObject vesselObj = GameObject.Find(VesselName.GetHierarchyName());
            GameObject blackHoleObj = GameObject.Find(BlackHoleName.GetHierarchyName());
            NeemVessel vessel = vesselObj.AddComponent<NeemVessel>();
            
            if (blackHoleObj.transform.TryGetChildByName("Sector", out Transform sector))
            {
                sector.gameObject.SetActive(false);
            }

            vessel.BlackHole = blackHoleObj;
        }

        /// <inheritdoc />
        protected void Update()
        {
            if (!_playAnim)
                return;

            if (!_blackHole.activeInHierarchy)
                _blackHole.SetActive(true);

            float time = _bhAnimTime += Time.deltaTime;
            float scale = _blackHoleScaleAnim.Evaluate(time);

            _blackHoleTransform.localScale = Vector3.one * scale;

            if (time <= SupernovaDelay)
                return;

            _playAnim = false;
            _blackHole.SetActive(false);
            StarController.Instance.StartCollapse();
        }
    }
}
