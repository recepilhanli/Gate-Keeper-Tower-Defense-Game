using System;
using System.Collections;
using System.Collections.Generic;
using Game.Modes;
using Game.PlayerOperations;
using Game.Utils;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game.Level
{
    using Debug = Utils.Logger.Debug;
    public class PlayerInventory : Singleton<PlayerInventory>
    {
        public UnityAction onPlayerBuyAnyBuild;

        [Header("Inventory")]
        [SerializeField] private PlacableContainer[] _placables = new PlacableContainer[3];
        [SerializeField] private GameObject _instructionPanel;

        [Header("Silhouette")]
        public Material silhouetteMaterial;
        public GameObject silhouetteGameObject;
        public MeshFilter silhouetteMeshFilter;
        [SerializeField] private int _activeSilhouetteIndex = 0;
        [SerializeField, Space(2)] private LayerMask _ignoreMask;

        private bool _isPlacing = false;
        private bool _isAllowed = false;

        private static readonly Color _notAllowedColor = new Color(1, 0, 0, .5f);
        private static readonly Color _allowedColor = new Color(0, 1, 0, .5f);

        private static readonly Color _cantBuyColor = new Color(.75f, 0, 0);
        private static readonly Color _selectedColor = new Color(0, 1, 0);
        private static readonly Color _defaultColor = new Color(1, 1, 1);

        public PlacableSO activePlacable => _placables[_activeSilhouetteIndex].placable;




        private void Start()
        {
            silhouetteGameObject.SetActive(false);

            Player.localPlayerInstance.onBowlingAttackStart += Disable;
            onPlayerBuyAnyBuild += Disable;

            HandleColorsForAllImages();
        }

        public void SetActiveSillhouette(int index)
        {
            if (_activeSilhouetteIndex == index)
            {
                Disable();
                return;
            }

            if (Player.localPlayerInstance.blockMovement) return;
            if (GameManager.instance.currency < _placables[index].placable.price) return;

            _activeSilhouetteIndex = index;
            silhouetteMeshFilter.mesh = activePlacable.silhouetteMesh;
            silhouetteGameObject.transform.localScale = activePlacable.silhouetteLocalScale;
            silhouetteGameObject.transform.localEulerAngles = activePlacable.silhouetteLocalRotation;
            silhouetteGameObject.SetActive(true);
            _isPlacing = true;
            _instructionPanel.SetActive(true);
            HandleColorsForAllImages();

        }

        private void HandleColorsForAllImages()
        {
            for (int i = 0; i < _placables.Length; i++)
            {
                HandleColorForImage(i);
            }
        }

        private void HandleColorForImage(int index)
        {
            var image = _placables[index].image;
            var placable = _placables[index].placable;

            if (placable.price > GameManager.instance.currency)
            {
                image.color = _cantBuyColor;
            }
            else if (_activeSilhouetteIndex == index)
            {
                image.color = _selectedColor;
            }
            else
            {
                image.color = _defaultColor;
            }
        }

        public void Place()
        {
            if (_isPlacing && _isAllowed && GameManager.instance.currency >= activePlacable.price)
            {
                GameManager.instance.currency -= activePlacable.price;
                var created = Instantiate(activePlacable.prefab, silhouetteGameObject.transform.position, silhouetteGameObject.transform.rotation);
                Tween.ShakeScale(created.transform, new Vector3(1.1f, 1.1f, 1.1f), .15f);
                EffectManager.instance.CreatePuffEffect(created.transform.position, 2.5f);
                onPlayerBuyAnyBuild?.Invoke();
            }
            else
            {
                Tween.ShakeScale(silhouetteGameObject.transform, new Vector3(.5f, .5f, .5f), .15f);
            }
        }

        public void Disable()
        {
            _isPlacing = false;
            silhouetteGameObject.SetActive(false);
            _activeSilhouetteIndex = -1;
            HandleColorsForAllImages();
            _instructionPanel.SetActive(false);
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
            // if (Keyboard.current.digit3Key.wasPressedThisFrame)
            // {
            //     SetActiveSillhouette(2);
            // }

            if (_isPlacing)
            {
                silhouetteGameObject.transform.position = Player.localPlayerInstance.lastMouseHitPoint + activePlacable.silhouettePositionOffset;

                if (!Player.localPlayerInstance.blockMovement)
                {

                    var colliders = Physics.OverlapSphere(Player.localPlayerInstance.lastMouseHitPoint, activePlacable.silhouetteMesh.bounds.size.x / 2, _ignoreMask);

                    Debug.Log("Colliders Length: " + colliders.Length);
                    if (colliders.Length == 1)
                    {
                        Debug.Log("Colliders[0]: " + colliders[0].name);
                        if (!colliders[0].CompareTag("Ground") || colliders[0].gameObject.layer == LayerMasks.LAYER_CANT_PLACE)
                        {
                            SetAllowed(false);
                        }
                        else SetAllowed(true);
                    }
                    else SetAllowed(false);


                    if (Keyboard.current.bKey.wasPressedThisFrame) Disable();
                    if (Keyboard.current.fKey.wasPressedThisFrame) Place();
                }
                else SetAllowed(false);

            }

        }

        [Serializable]
        struct PlacableContainer
        {
            public PlacableSO placable;
            public Image image;
        }

    }
}