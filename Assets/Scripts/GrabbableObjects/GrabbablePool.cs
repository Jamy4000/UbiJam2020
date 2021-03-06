﻿using System.Collections.Generic;
using UnityEngine;

namespace UbiJam.GrabbableObjects
{
    public class GrabbablePool : MonoBehaviour
    {
        [Tooltip("Assign a grabbable type and it's gameObject IN GRABBABLEPOOL THE PREFAB")]
        public List<GrabbableObjectType> GrabbableObjectReferences = new List<GrabbableObjectType>();

        public GrabbableObjectType GetGrabbableObjectType(EGrabbableObjects type)
        {
            foreach (var got in GrabbableObjectReferences)
            {
                if (got.Type == type)
                    return got;
            }
            Debug.LogError("BUG BUG BUG BUG");
            return null;
        }
    }

    [System.Serializable]
    public class GrabbableObjectType
    {
        public EGrabbableObjects Type;
        public GameObject SceneGO;
        public Sprite ItemSprite;
    }
}
