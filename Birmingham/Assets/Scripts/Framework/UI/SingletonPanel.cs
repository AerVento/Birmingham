using System.Buffers.Text;
using Framework.Singleton;
using UnityEngine;

namespace Framework.UI
{
    /// <summary>
    /// Class for panels only have one instance, and makes it easier for extern to access the panel.
    /// This is temporary singleton panel behaviour because panels can be created and destroyed by UIManager.
    /// </summary>
    /// <typeparam name="T">The panel type want to be singleton.</typeparam>
    public abstract class SingletonPanel<T> : MonoSingleton<T>, IPanel where T : MonoBehaviour
    {
        /// <summary>
        /// Get panel singleton instance. Returns null when singleton doesn't exists. 
        /// </summary>
        public static new T Instance => _instance;

        protected bool _isShowing;
        public virtual bool IsShowing => _isShowing;

        public virtual void Hide()
        {
            _isShowing = false;
            gameObject.SetActive(false);
        }
        public virtual void Show()
        {
            _isShowing = true;
            gameObject.SetActive(true);
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}