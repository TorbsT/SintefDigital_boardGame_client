using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    [ExecuteAlways]
    public class ColorSchemeComponent : MonoBehaviour
    {
        [SerializeField] private ColorSchemeSystem.Scheme scheme;
        [SerializeField] private new Image renderer;

        private void Awake()
        {
            renderer = GetComponent<Image>();
            SchemeChanged();
        }
        private void OnEnable()
        {
            if (ColorSchemeSystem.Instance != null)
                ColorSchemeSystem.Instance.ColorChanged += SchemeChanged;
        }
        private void OnDisable()
        {
            if (ColorSchemeSystem.Instance != null)
                ColorSchemeSystem.Instance.ColorChanged -= SchemeChanged;
        }
        private void SchemeChanged()
        {
            if (renderer == null)
            {
                Debug.LogWarning(gameObject +
                    " has no renderer assigned to its colorschemecomponent");
                return;
            }
            ColorSchemeSystem system = ColorSchemeSystem.Instance;
            if (system == null)
                return;
            renderer.color = system.GetColor(scheme);
        }
        private void OnValidate()
        {
            SchemeChanged();
        }
    }
}

