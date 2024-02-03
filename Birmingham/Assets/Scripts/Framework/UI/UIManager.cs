using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Singleton;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        #region Initialization

        [Header("Initialization")]
        [SerializeField]
        private GameObject _canvasPrefab;

        [SerializeField]
        private GameObject _eventSystemPrefab;

        [Header("Settings")]
        // Available panels
        [SerializeField]
        private GameObject[] _panels;
        #endregion

        #region Runtime Fields
        // canvas instance & event system instance
        private Canvas _canvas;
        private EventSystem _eventSystem;

        // four layers for panels to show
        private Transform _bottomLayer;
        private Transform _middleLayer;
        private Transform _topLayer;
        private Transform _systemLayer;

        // Runtime Panel Dictionary, managing loaded panel prefabs.
        private Dictionary<Type, GameObject> _runtimePanels;

        // Runtime Panel Dictionary, managing showing panel component instance. 
        private Dictionary<string, GameObject> _showingPanels = new();
        #endregion

        public Canvas Canvas => _canvas;

        protected override void Awake()
        {
            base.Awake();
            InitializeEnvironment();
            InitializeRuntimePanel();
        }

        private void InitializeEnvironment()
        {
            _canvas = Instantiate(_canvasPrefab).GetComponent<Canvas>();
            _eventSystem = Instantiate(_eventSystemPrefab).GetComponent<EventSystem>();

            _bottomLayer = _canvas.transform.Find("Bot");
            _middleLayer = _canvas.transform.Find("Mid");
            _topLayer = _canvas.transform.Find("Top");
            _systemLayer = _canvas.transform.Find("System");
        }

        private void InitializeRuntimePanel()
        {
            _runtimePanels = new();
            foreach (var panel in _panels)
            {
                if (panel.TryGetComponent(out IPanel component))
                {
                    // if the type already exists
                    if (!_runtimePanels.TryAdd(component.GetType(), panel))
                    {
                        Debug.LogWarning($"Adding Panel Error: On GameObject {panel}, Type {component.GetType()} already exists. ");
                    }
                }
                else
                {
                    Debug.LogWarning($"GameObject {panel} doesn't have a component which derived from IPanel.");
                }
            }
        }
        
        /// <summary>
        /// Get layer transform.
        /// </summary>
        public Transform GetLayer(Layer layer)
        {
            return layer switch
            {
                Layer.Bottom => _bottomLayer,
                Layer.Middle => _middleLayer,
                Layer.Top => _topLayer,
                Layer.System => _systemLayer,
                _ => null
            };
        }

        /// <summary>
        /// Get a existing panel instance.
        /// </summary>
        /// <param name="panelIdentifier">The identifier of the panel to get.</param>
        /// <returns>The panel component.</returns>
        public T GetPanel<T>(string panelIdentifier) where T : IPanel
        {
            if (_showingPanels.TryGetValue(panelIdentifier, out var target))
            {
                return target.GetComponent<T>();
            }
            else
                throw new KeyNotFoundException($"The panel identifier {panelIdentifier} doesn't exists.");
        }

        /// <summary>
        /// Instantiate a panel and set a identifier for it. The identifier must be unique.
        /// </summary>
        /// <param name="type">The panel type.</param>
        public IPanel CreatePanel(Type type, string panelIdentifier, Layer layer = Layer.Middle)
        {
            if (!_runtimePanels.TryGetValue(type, out GameObject prefab))
            {
                Debug.LogWarning("Unknown panel type: " + type);
                return null;
            }

            if (_showingPanels.ContainsKey(panelIdentifier))
            {
                Debug.LogError("Panel identifier already exists: " + panelIdentifier);
                return null;
            }

            GameObject instance = Instantiate(prefab, GetLayer(layer));
            _showingPanels.Add(panelIdentifier, instance);

            IPanel panel = instance.GetComponent<IPanel>();
            return panel;
        }

        /// <summary>
        /// Instantiate a panel and set a identifier for it. The identifier must be unique.
        /// </summary>
        public T CreatePanel<T>(string panelIdentifier, Layer layer = Layer.Middle) where T : IPanel
        {
            return (T)CreatePanel(typeof(T), panelIdentifier, layer);
        }

        /// <summary>
        /// Destroy a panel.
        /// </summary>
        /// <param name="panelIdentifier">The identifier of the panel to destroy.</param>
        public void DestroyPanel(string panelIdentifier)
        {
            if (_showingPanels.TryGetValue(panelIdentifier, out GameObject target))
            {
                Destroy(target);
                _showingPanels.Remove(panelIdentifier);
            }
        }

        public enum Layer
        {
            Bottom,
            Middle,
            Top,
            System
        }
    }
}
