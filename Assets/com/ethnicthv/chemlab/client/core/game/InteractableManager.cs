using System.Collections.Generic;
using com.ethnicthv.chemlab.client.api.core.game;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.core.game
{
    public class InteractableManager
    {
        private static Dictionary<GameObject, IInteractable> _interactables = new Dictionary<GameObject, IInteractable>();
        
        public static void RegisterInteractable(GameObject gameObject, IInteractable interactable)
        {
            _interactables.Add(gameObject, interactable);
        }
        
        public static void UnregisterInteractable(GameObject gameObject)
        {
            _interactables.Remove(gameObject);
        }
        
        public static IInteractable GetInteractable(GameObject gameObject)
        {
            return _interactables[gameObject];
        }
        
        public static bool TryGetInteractable(GameObject gameObject, out IInteractable interactable)
        {
            return _interactables.TryGetValue(gameObject, out interactable);
        }
    }
}