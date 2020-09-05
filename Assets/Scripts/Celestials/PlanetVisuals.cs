using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Planetarity.Celestials
{
    public class PlanetVisuals : MonoBehaviour
    {
        private static readonly int MainColor = Shader.PropertyToID("_Color");

        [SerializeField] private MeshRenderer _meshRenderer;

        private IReadOnlyList<Color> _colors;

        public int RequiredColors => _meshRenderer.materials.Length;

        public void SetColors(IReadOnlyList<Color> colors)
        {
            if (RequiredColors > colors.Count)
                throw new ArgumentException($"Incorrect number of colors: {colors.Count}; expected not less then: {RequiredColors}");

            _colors = colors;

            ApplyColors(_colors);
        }

        private void ApplyColors(IReadOnlyList<Color> colors)
        {
            for (var i = 0; i < _meshRenderer.materials.Length; i++)
            {
                var propertyBlock = new MaterialPropertyBlock();
                propertyBlock.SetColor(MainColor, colors[i]);
                _meshRenderer.SetPropertyBlock(propertyBlock, i);
            }
        }

        public void MarkAsDead()
        {
            var deadPlanetColors = _colors
                .Select(color => color * 0.5f)
                .ToList();
            ApplyColors(deadPlanetColors);
        }
    }
}