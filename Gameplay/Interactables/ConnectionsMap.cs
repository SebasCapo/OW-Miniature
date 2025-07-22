using System;
using System.Collections.Generic;
using System.Linq;

using OWMiniature.Gameplay.Lines;
using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    public class ConnectionsMap : MapInteractableBase
    {
        public static ConnectionsMap Instance { get; private set; }

        private const string AwaitingInput = "Awaiting Input";
        private List<Transform> _connections = new List<Transform>();
        private List<int> _input = new List<int>();
        private ScreenPrompt _codePrompt;

        public bool IsInputCorrect => _input.SequenceEqual(Spoilers.OverclockSequence);

        /// <inheritdoc />
        public override CustomMapMode MapMode => CustomMapMode.Connections;

        /// <inheritdoc />
        protected override string MapViewText => "Input Sequence";

        /// <inheritdoc />
        protected override void OnTargetSelect(ReferenceFrame frame, Transform attachedObject)
        {
            base.OnTargetSelect(frame, attachedObject);

            if (IsInputCorrect)
                return;

            if (_connections.Contains(attachedObject))
                return;

            bool isInputCorrect = false;
            _connections.Add(attachedObject);

            if (attachedObject.TryGetComponent(out OverclockMarker overclock))
            {
                _input.Add(overclock.StoredCode);

                isInputCorrect = IsInputCorrect;
                string code = string.Join(string.Empty, _input);
                string validate = isInputCorrect ? "<<< SEQUENCE INITIATED >>>" : $"[INVALID CODE] {code}";

                _codePrompt.SetText(validate);
            }

            CreateLine(attachedObject);
            
            // If we had more time I'd use isInputCorrect here to keep the lines for visuals.
            // Maybe something we can add later!
        }

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();

            Instance = this;
            GenerateMarkers();
        }

        /// <inheritdoc />
        protected override void Start()
        {
            base.Start();

            if (_codePrompt == null)
            {
                _codePrompt = new ScreenPrompt(AwaitingInput);
                Locator.GetPromptManager().AddScreenPrompt(_codePrompt, PromptPosition.UpperLeft, false);
            }
        }

        /// <inheritdoc />
        protected override void OnDestroy()
        {
            base.OnDestroy();

            Instance = null;

            if (_codePrompt != null)
            {
                Locator.GetPromptManager()?.RemoveScreenPrompt(_codePrompt, PromptPosition.UpperLeft);
            }
        }

        /// <inheritdoc />
        protected override void OnExitMapView()
        {
            base.OnExitMapView();

            _connections.Clear();
            _input.Clear();

            UpdatePromptVisibility(false);
        }

        /// <inheritdoc />
        protected override void OnEnterMapView()
        {
            base.OnEnterMapView();

            _codePrompt.SetText(AwaitingInput);
            UpdatePromptVisibility(true);
        }

        private void UpdatePromptVisibility(bool isVisible)
        {
            if (_codePrompt != null)
            {
                _codePrompt.SetVisibility(isVisible);
            }
        }

        private void CreateLine(Transform attachedObject)
        {
            GameObject lineObj = attachedObject.CreateChild(objName: LineObjectDefaultName);

            if (Lines.Count == 0)
            {
                SelectionLine selection = lineObj.AddComponent<SelectionLine>();

                selection.VisibleInWorld = false;
                selection.Assign(attachedObject);
                Lines.Add(selection);
                return;
            }

            ConnectionLine connection = lineObj.AddComponent<ConnectionLine>();
            PlanetaryLineBase previousLine = Lines[Lines.Count - 1];
            connection.SetColors(Color.green, Color.cyan.WithAlpha(0.75f));
            connection.TargetPosition = previousLine.transform;
            connection.VisibleInWorld = false;
            connection.SwapPositions = true;
            connection.Assign(attachedObject);
            Lines.Add(connection);
        }

        private static void GenerateMarkers()
        {
            int code = 0;
            foreach (AstroObject astro in PlanetaryUtils.AstroObjects)
            {
                string astroName = astro.name;

                if (StartsWith(astroName, "GloamingGalaxy") || StartsWith(astroName, "Nomai") || StartsWith(astroName, "BrightSpark"))
                    continue;

                Transform astroTransform = astro.gameObject.transform;
                GameObject markerObj = astroTransform.CreateChild(objName: "Custom Marker");
                OverclockMarker marker = markerObj.AddComponent<OverclockMarker>();

                marker.StoredCode = code++;
                marker.StartingLabel = $"<color=#f56969>{marker.StoredCode}</color>";
                marker.MapMode = CustomMapMode.Connections;
                marker.MapModeExclusive = true;
                marker.SetTarget(astroTransform);
            }
        }

        private static bool StartsWith(string targetText, string match)
        {
            return targetText.StartsWith(match, StringComparison.OrdinalIgnoreCase);
        }
    }
}