using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor;
using UnityEngine;

namespace JamesMotion.IntroToPhysics
{
    [RequireComponent(typeof(CapsuleCollider))] public class TeleportingCapsule : MonoBehaviour
    {
        [SerializeField] private float teleportDistance;
        [SerializeField] private float channelTime;
        [SerializeField] private LayerMask layersToIgnore;
        [SerializeField] private QueryTriggerInteraction canThisTeleportIntoTriggers;
        [SerializeField] private KeyCode teleportFowardKeyCode;
        private CapsuleCollider thisCapsuleCollider;
        private bool currentlyTeleporting;


        // Start is called before the first frame update
        private void Start()
        {
            thisCapsuleCollider = GetComponent<CapsuleCollider>();
            currentlyTeleporting = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(teleportFowardKeyCode))
                StartTeleportCapsule();

            void OnDrawGizmos()
            {
                Gizmos.DrawIcon(TeleportTopCenterPoint(), "Light Gizmo.tiff", true);
            }
            //DrawWireCapsule(TeleportTopCenterPoint(), TeleportBottomCenterPoint(), thisCapsuleCollider.radius);
        }
        private void StartTeleportCapsule()
        {
            if(!currentlyTeleporting)
            {
                currentlyTeleporting = true;
                print("Start Teleporting");
                Invoke(nameof(TeleportCapsule), channelTime);
            }
        }
        
        private Vector3 MiddleOfLocationToTeleportTo()
        {
            Vector3 placeToTeleportVector3 = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            placeToTeleportVector3 += Vector3.forward * teleportDistance;
            return placeToTeleportVector3;
        }


        private void TeleportCapsule()
        {
            print("Teleport");
            
            // I think this is wrong and needs to be fixed and needs to us the "placeToTeleport".
            bool isTeleportationSpaceFree = CheckIfSpaceIsFree();

            print($"Is this space in front of me me free to move to ? {isTeleportationSpaceFree}");
            return;

            if(isTeleportationSpaceFree)
            {
                transform.position = MiddleOfLocationToTeleportTo();
                Debug.Log("Teleported forward");
            }
            else
            {
                Debug.Log("Sorry can't teleport there, something is there");
            }

            currentlyTeleporting = false;
        }

        private bool CheckIfSpaceIsFree()
        {
            bool isThisSpaceFreeToMoveTo = false;
            isThisSpaceFreeToMoveTo = Physics.CheckCapsule(TeleportTopCenterPoint(), TeleportBottomCenterPoint(), thisCapsuleCollider.radius);
            return isThisSpaceFreeToMoveTo;
        }

        private Vector3 TeleportBottomCenterPoint()
        {
            Vector3 bottomCenterPoint = MiddleOfLocationToTeleportTo();
            bottomCenterPoint.y -= OffSetOfCenterPointsOfSpheresToTheMiddleOfTheCapsule();
            return bottomCenterPoint;
        }

        private Vector3 TeleportTopCenterPoint()
        {
            Vector3 topCenterPoint = MiddleOfLocationToTeleportTo();
            print(MiddleOfLocationToTeleportTo());
            topCenterPoint.y += OffSetOfCenterPointsOfSpheresToTheMiddleOfTheCapsule();
            return topCenterPoint;
        }

        private float OffSetOfCenterPointsOfSpheresToTheMiddleOfTheCapsule()
        {
            return thisCapsuleCollider.height / 2 + thisCapsuleCollider.radius;
        }

        /// <summary>
        /// Draws a wireframe capsule in Draw Gizmos.
        /// </summary>
        /// <param name="_sphere1Centre">Centre of Sphere 1.</param>
        /// <param name="_sphere2Centre">Centre of Sphere 2.</param>
        /// <param name="_capsuleRadius">Radius of the Capsule.</param>
        public static void DrawWireCapsule(Vector3 _sphere1Centre, Vector3 _sphere2Centre, float _capsuleRadius)
        {
        #if UNITY_EDITOR
            // Special case when both points are in the same position
            if(_sphere1Centre == _sphere2Centre)
            {
                // DrawWireSphere works only in gizmo methods
                Gizmos.DrawWireSphere(_sphere1Centre, _capsuleRadius);
                return;
            }

            using(new UnityEditor.Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
            {
                Quaternion p1Rotation = Quaternion.LookRotation(_sphere1Centre - _sphere2Centre);
                Quaternion p2Rotation = Quaternion.LookRotation(_sphere2Centre - _sphere1Centre);
                // Check if capsule direction is collinear to Vector.up
                float c = Vector3.Dot((_sphere1Centre - _sphere2Centre).normalized, Vector3.up);

                if(c == 1f || c == -1f)
                {
                    // Fix rotation
                    p2Rotation = Quaternion.Euler(p2Rotation.eulerAngles.x, p2Rotation.eulerAngles.y + 180f, p2Rotation.eulerAngles.z);
                }

                // First side
                UnityEditor.Handles.DrawWireArc(_sphere1Centre, p1Rotation * Vector3.left, p1Rotation * Vector3.down, 180f, _capsuleRadius);
                UnityEditor.Handles.DrawWireArc(_sphere1Centre, p1Rotation * Vector3.up, p1Rotation * Vector3.left, 180f, _capsuleRadius);
                UnityEditor.Handles.DrawWireDisc(_sphere1Centre, (_sphere2Centre - _sphere1Centre).normalized, _capsuleRadius);
                // Second side
                UnityEditor.Handles.DrawWireArc(_sphere2Centre, p2Rotation * Vector3.left, p2Rotation * Vector3.down, 180f, _capsuleRadius);
                UnityEditor.Handles.DrawWireArc(_sphere2Centre, p2Rotation * Vector3.up, p2Rotation * Vector3.left, 180f, _capsuleRadius);
                UnityEditor.Handles.DrawWireDisc(_sphere2Centre, (_sphere1Centre - _sphere2Centre).normalized, _capsuleRadius);
                // Lines
                UnityEditor.Handles.DrawLine(_sphere1Centre + p1Rotation * Vector3.down * _capsuleRadius, _sphere2Centre + p2Rotation * Vector3.down * _capsuleRadius);
                UnityEditor.Handles.DrawLine(_sphere1Centre + p1Rotation * Vector3.left * _capsuleRadius, _sphere2Centre + p2Rotation * Vector3.right * _capsuleRadius);
                UnityEditor.Handles.DrawLine(_sphere1Centre + p1Rotation * Vector3.up * _capsuleRadius, _sphere2Centre + p2Rotation * Vector3.up * _capsuleRadius);
                UnityEditor.Handles.DrawLine(_sphere1Centre + p1Rotation * Vector3.right * _capsuleRadius, _sphere2Centre + p2Rotation * Vector3.left * _capsuleRadius);
            }
        #endif
        }
    }
}