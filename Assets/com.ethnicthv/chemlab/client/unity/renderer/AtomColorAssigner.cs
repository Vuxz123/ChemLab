using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.unity.renderer
{
    public class AtomColorAssigner
    {
        private readonly Dictionary<Element, Color> _elementColors = new();

        public Color GetColorForElement(Element element)
        {
            if (_elementColors.TryGetValue(element, out var forElement))
            {
                return forElement;
            }

            var color = GetRandomColor();
            while (CheckColorExist(color))
            {
                color = GetRandomColor();
            }
            _elementColors[element] = color;
            Debug.Log($"Color for {element} is {color}");
            return color;
        }
        
        public void Clear()
        {
            _elementColors.Clear();
        }

        private Color GetRandomColor()
        {
            return Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        private bool CheckColorExist(Color color)
        {
            foreach (var (_, elementColor) in _elementColors)
            {
                if (elementColor == color)
                {
                    return true;
                }
            }

            return false;
        }
    }
}