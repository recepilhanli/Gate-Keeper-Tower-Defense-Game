using System.Collections;
using System.Collections.Generic;
using Game.PlayerOperations;
using Game.Utils;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game.Level
{
    using Debug = Utils.Logger.Debug;
    public class PlayerInventory : Singleton<PlayerInventory>
    {
        [Header("Inventory")]
        public PlacableSO[] placables = new PlacableSO[3];
        public PlacableSO activePlacable => placables[_activeSilhouetteIndex];

        [Header("Silhouette")]
        public Material silhouetteMaterial;
        public GameObject silhouetteGameObject;
        public MeshFilter silhouetteMeshFilter;

        private bool _isPlacing = false;
        private bool _isAllowed = false;

        [SerializeField] private int _activeSilhouetteIndex = 0;

        [SerializeField] private LayerMask _ignoreMask;

        private static readonly Color _notAllowedColor = new Color(1, 0, 0, .5f);
        private static readonly Color _allowedColor = new Color(0, 1, 0, .5f);

        private void Start()
        {
            silhouetteGameObject.SetActive(false);
        }

        public void SetActiveSillhouette(int index)
        {
            _activeSilhouetteIndex = index;
            silhouetteMeshFilter.mesh = placables[_activeSilhouetteIndex].silhouetteMesh;
            silhouetteGameObject.transform.localScale = activePlacable.silhouetteLocalScale;
            silhouetteGameObject.transform.localEulerAngles = activePlacable.silhouetteLocalRotation;
            silhouetteGameObject.SetActive(true);
            _isPlacing = true;
        }

        public void Place()
        {
            if (_isPlacing)
            {
                Instantiate(placables[_activeSilhouetteIndex].prefab, silhouetteGameObject.transform.position, silhouetteGameObject.transform.rotation);
                Disable();
            }
        }

        public void Disable()
        {
            _isPlacing = false;
            silhouetteGameObject.SetActive(false);
        }

        private void SetAllowed(bool isAllowed)
        {
            bool wasChanged = _isAllowed != isAllowed;
            _isAllowed = isAllowed;
            if (wasChanged) silhouetteMaterial.color = isAllowed ? _allowedColor : _notAllowedColor;
        }

        private void LateUpdate()
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                SetActiveSillhouette(0);
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                SetActiveSillhouette(1);
            }
            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                SetActiveSillhouette(2);
            }



            if (_isPlacing)
            {


                silhouetteGameObject.transform.position = Player.localPlayerInstance.lastMouseHitPoint + activePlacable.silhouettePositionOffset;

                var colliders = Physics.OverlapSphere(Player.localPlayerInstance.lastMouseHitPoint, activePlacable.silhouetteMesh.bounds.size.x / 2, _ignoreMask);

                Debug.Log("Colliders Length: " + colliders.Length);
                if (colliders.Length == 1)
                {
                    Debug.Log("Colliders[0]: " + colliders[0].name);
                    if (!colliders[0].CompareTag("Ground"))
                    {
                        SetAllowed(false);
                    }
                    else SetAllowed(true);
                }
                else SetAllowed(false);


                if (Keyboard.current.bKey.wasPressedThisFrame) Disable();
                if (Keyboard.current.fKey.wasPressedThisFrame) Place();

            }
        }

    }
}