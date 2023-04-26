using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ExtraCollision
{
    Empty,
    Frozen
}

namespace SpecialItemCollisions
{
    public class SpecialCollisions : MonoBehaviour
    {
        private Dictionary<ExtraCollision, UnityEvent> collisionsDictionary = 
            new Dictionary<ExtraCollision, UnityEvent>();
        public Dictionary<ExtraCollision, UnityEvent> CollisionsDictionary => collisionsDictionary;
        private UnityEvent lastInvokedEvent = null;
        
        private void Awake()
        {
            collisionsDictionary.Add(ExtraCollision.Frozen, new UnityEvent());
        }
        
        public void RunSpecialCollision(ExtraCollision extraCollision)
        {
            lastInvokedEvent = collisionsDictionary.GetValueOrDefault(extraCollision);
            lastInvokedEvent.Invoke();
        }
    }
}


