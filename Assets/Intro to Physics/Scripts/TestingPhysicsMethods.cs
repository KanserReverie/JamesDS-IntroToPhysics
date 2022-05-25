using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JamesMotion.IntroToPhysics
{
    public class TestingPhysicsMethods : MonoBehaviour
    {
        [Header("An example serialized LayerMask")]
        [SerializeField] private LayerMask serializedLayerMask;
        [Header("All Layers for casts to ignore")]
        [SerializeField] private LayerMask layersToIgnoreInCasts;
        [SerializeField] private QueryTriggerInteraction queryTriggerInteraction;
        // Start is called before the first frame update
        void Start()
        {
            RaycastHit[] hits;
            hits = Physics.CapsuleCastAll(Vector3.negativeInfinity, Vector3.negativeInfinity, 3, transform.forward);

        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}