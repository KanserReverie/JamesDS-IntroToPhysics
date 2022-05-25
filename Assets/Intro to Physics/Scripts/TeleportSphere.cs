using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JamesMotion.IntroToPhysics
{
    public class TeleportSphere : MonoBehaviour
    {
        public string whatThisDoes = "Move forward 2 spaces on press of 'F'";
        
        [SerializeField] private SphereCollider thisSphereCollider;

        private void Start()
        {
            thisSphereCollider = GetComponent<SphereCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                if(AreThereCollidersInTeleportSpace())
                {
                    print("Can't teleport here");
                    return;
                }
                else
                {
                    print("Can teleport here");
                }

                //TeleportToSpace();
            }
        }


        private bool AreThereCollidersInTeleportSpace()
        {
            // Default to the Space not being free.
            bool areThereCollidersInTeleportSpace = true;
            // Get the world space where we are at.
            Vector3 whereToTeleport = transform.position;
            // Add 2 spaces forward.
            whereToTeleport += transform.forward * 2;
            // Get the radius of the Sphere Collider.
            float radiusOfSpaceToCheck = thisSphereCollider.radius;
            // Check if the space is free for the Sphere to Teleport.
            areThereCollidersInTeleportSpace = Physics.CheckSphere(whereToTeleport, radiusOfSpaceToCheck);
            return areThereCollidersInTeleportSpace;
        }
        
        private void TeleportToSpace()
        {
            throw new System.NotImplementedException();
        }

        //Find the Center Of where to teleport. 
        void OnDrawGizmosSelected()
        {
            // Display the center of the sphere to teleport.
            // Draw Gizmos Green. 
            Gizmos.color = Color.green;
            // Get the world space where we are at.
            Vector3 whereToTeleport = transform.position;
            // Add 2 spaces forward.
            whereToTeleport.z += 2;
            // Get the radius of the Sphere Collider.
            float radiusOfSpaceToCheck = thisSphereCollider.radius;
            // Draw the space in front of us that will be checked and teleported to.
            Gizmos.DrawSphere(whereToTeleport, radiusOfSpaceToCheck);
        }
    }
}