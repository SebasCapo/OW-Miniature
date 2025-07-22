using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OWMiniature.Utils;

using Steamworks;

using UnityEngine;

namespace OWMiniature.Gameplay.Wrappers
{
    public class NeemVessel : MonoBehaviour
    {
        private const string VesselName = "Nomai Vessel";
        private const string BlackHoleName = "Gloaming Galaxy";
        private const float SupernovaDelay = 8f;

        public static NeemVessel Instance;

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
            new Keyframe(SupernovaDelay * 1.4f, 1),
            new Keyframe(SupernovaDelay * 2.4f, 0)
        );

        private readonly AnimationCurve _vesselScaleAnim = new AnimationCurve
        (
            new Keyframe(0, 1f),
            new Keyframe(SupernovaDelay * 1.7f, 1f),
            new Keyframe(SupernovaDelay * 2.1f, 0f)
        );

        private GameObject _blackHole;
        private GameObject _blackHoleSector;
        private Transform _blackHoleTransform;
        private float _bhAnimTime;
        private bool _playAnim;
        private bool _endingStarted;

        public static void Generate()
        {
            GameObject vesselObj = GameObject.Find(VesselName.GetHierarchyName());
            GameObject blackHoleObj = GameObject.Find(BlackHoleName.GetHierarchyName());
            NeemVessel vessel = vesselObj.AddComponent<NeemVessel>();
            
            if (blackHoleObj.transform.TryGetChildByName("Sector", out Transform sector))
            {
                vessel._blackHoleSector = sector.gameObject;
                vessel._blackHoleSector.SetActive(false);
            }

            vessel.BlackHole = blackHoleObj;
            Instance = vessel;
        }

        public static void BeginEnding()
        {
            Instance._playAnim = true;
        }

        /// <inheritdoc />
        protected void Start()
        {
            CleanupBodies();
            CleanupWalls();
            ResizeAirlock();
            DestroyBaseDoor();
        }

        /// <inheritdoc />
        protected void Update()
        {
            if (!_playAnim)
            {
                _bhAnimTime = 0f;
                return;
            }

            if (!_blackHoleSector.activeInHierarchy)
                _blackHoleSector.SetActive(true);

            float time = _bhAnimTime += Time.deltaTime;
            float bhScale = _blackHoleScaleAnim.Evaluate(time);
            float vesselScale = _vesselScaleAnim.Evaluate(time);

            _blackHoleTransform.localScale = Vector3.one * bhScale;
            transform.localScale = Vector3.one * vesselScale;

            if (time <= SupernovaDelay)
                return;

            if (_endingStarted)
                return;

            _endingStarted = true;
            StarController.Instance.StartCollapse();
            DialogueConditionManager.SharedInstance.SetConditionState("BRIGHTSPARK_ENDING_1", true);
        }

        private void CleanupBodies()
        {
            Transform[] allObjects = GetComponentsInChildren<Transform>();

            for (int i = allObjects.Length - 1; i >= 0; i--)
            {
                Transform obj = allObjects[i];

                if (!obj.name.Contains("Skeleton_Body"))
                    continue;

                Destroy(obj.gameObject);
            }
        }

        private void CleanupWalls()
        {
            NomaiWallText[] walls = GetComponentsInChildren<NomaiWallText>(true);

            for (int i = walls.Length - 1; i >= 0; i--)
            {
                NomaiWallText wall = walls[i];

                if (wall.name.Contains("Outgoing"))
                {
                    Destroy(wall.gameObject);
                    continue;
                }

                // Originally wanted to keep some text entries, but fuck is it difficult. :/
                Destroy(wall.gameObject);
                //ClearWallEntries(wall);
            }
        }

        private void ClearWallEntries(NomaiWallText wall, int skippedEntries = 4)
        {
            Transform wallTransform = wall.transform;

            for (int i = wallTransform.childCount - 1; i >= 0; i--)
            {
                if (i <= skippedEntries)
                    continue;

                Transform child = wallTransform.GetChild(i);

                Destroy(child.gameObject);
            }

            wall.Awake();
            wall.LateInitialize();
        }

        private void ResizeAirlock()
        {
            if (!transform.TryGetChildByName("Sector", out Transform sector))
                return;

            if (!sector.TryGetChildByName("Door", out Transform door))
                return;

            if (!door.TryGetChildByName("Structure_NOM_Airlock_new", out Transform airlock))
                return;

            if (!airlock.TryGetChildByName("Airlock_Anchor", out Transform anchor))
                return;

            float size = 1f;

            for (int i = 0; i < 5; i++)
            {
                size += 0.13f;

                GameObject airlockClone = Instantiate(anchor.gameObject);
                airlockClone.transform.SetParent(airlock, false);
                airlockClone.transform.localPosition = anchor.localPosition;

                airlockClone.transform.localScale = Vector3.one * size;

                Destroy(airlockClone.GetComponent<OWCollider>());
                airlockClone.GetComponent<Collider>().enabled = true;
            }
        }

        private void DestroyBaseDoor()
        {
            //NomaiVessel_Body/Sector/Rename/Sector_VesselBridge/Interactibles_VesselBridge/WarpController/Structure_NOM_Door_Silver_Round_Open

            if (!transform.TryGetChildByName("Sector", out Transform sector))
                return;

            if (!sector.TryGetChildByName("Rename", out Transform vessel))
                return;

            if (!vessel.TryGetChildByName("Sector_VesselBridge", out Transform bridge))
                return;

            if (!bridge.TryGetChildByName("Interactibles_VesselBridge", out Transform interactables))
                return;

            if (!interactables.TryGetChildByName("WarpController", out Transform warpController))
                return;

            if (!warpController.TryGetChildByName("Structure_NOM_Door_Silver_Round_Open", out Transform door))
                return;

            Destroy(door.gameObject);
        }
    }
}
