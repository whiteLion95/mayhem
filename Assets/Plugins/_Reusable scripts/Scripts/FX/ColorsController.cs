using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class ColorsController : MonoBehaviour
    {
        [SerializeField] protected float changeSmoothness = 0.5f;
        [SerializeField] protected Color targetColor;
        [SerializeField] protected List<Renderer> renderers;
        [SerializeField] protected List<int> materialsIndexes;
        [SerializeField] protected float _toBlinkDuration = 0.2f;
        [SerializeField] protected float _blinkDuration = 0.1f;

        protected List<Color> _startingColors;
        protected List<Material> _materials;
        protected Coroutine _blinkRoutine;

        protected virtual void Start()
        {
            Init();
        }

        private void Init()
        {
            _materials = new List<Material>();
            _startingColors = new List<Color>();

            for (int i = 0; i < materialsIndexes.Count; i++)
            {
                int index = materialsIndexes[i];

                for (int k = 0; k < renderers.Count; k++)
                {
                    if (index < renderers[k].materials.Length)
                    {
                        Material mat = renderers[k].materials[index];
                        _materials.Add(mat);
                        _startingColors.Add(mat.color);
                    }
                }
            }
        }

        /// <summary>
        /// Sets color of changing material
        /// </summary>
        /// <param name="color"></param>
        public void SetColorSmooth(Color color, float duration)
        {
            foreach (Material material in _materials)
            {
                material.DOColor(color, duration);
            }
        }

        public void SetColorSmooth(Color color)
        {
            SetColorSmooth(color, changeSmoothness);
        }

        public void SetColor()
        {
            SetColorSmooth(targetColor);
        }

        /// <summary>
        /// Resets color of changing material to the starting color
        /// </summary>
        public void ResetColor()
        {
            for (int i = 0; i < _materials.Count; i++)
            {
                _materials[i].color = _startingColors[i];
            }
        }

        public void ResetColorSmooth(float duration)
        {
            for (int i = 0; i < _materials.Count; i++)
            {
                _materials[i].DOColor(_startingColors[i], duration);
            }
        }

        public void Blink()
        {
            Blink(targetColor);
        }

        public void Blink(Color color)
        {
            if (_blinkRoutine != null)
                StopCoroutine(_blinkRoutine);

            _blinkRoutine = StartCoroutine(BlinkRoutine(color));
        }

        private IEnumerator BlinkRoutine(Color color)
        {
            SetColorSmooth(color, _toBlinkDuration);

            yield return new WaitForSeconds(_toBlinkDuration);
            yield return new WaitForSeconds(_blinkDuration);

            ResetColorSmooth(_toBlinkDuration);
        }
    }
}
