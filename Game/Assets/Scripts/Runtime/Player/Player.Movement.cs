using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.PlayerOperations
{
    using System;
    using Animations;
    using Debug = Utils.Logger.Debug;
    //Player.Movement
    public partial class Player
    {
        [Header("Movement")]
        public Rigidbody rigidBody;
        public float movementSpeed = 5f;
        [NonSerialized] public bool blockMovement = false;
        [NonSerialized] public Vector3 lastMouseHitPoint;


        private void InitMovement()
        {
            rigidBody.sleepThreshold = 0f;
            rigidBody.freezeRotation = true;
            PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        }

        private void UpdateMovement()
        {
            Move();
            Look();
        }


        private void Move()
        {
            if (blockMovement) return;

            Vector2 input = movementInput * movementSpeed;
            Vector3 movement = new Vector3(input.x, rigidBody.velocity.y, input.y);

            rigidBody.velocity = movement;

            animator.SetBool(AnimationTable.IsMoving, rigidBody.velocity.magnitude > 0.1);

            //Removed
            // animator.SetFloat(AnimationTable.x, movementInput.x * transform.forward.x);
            // animator.SetFloat(AnimationTable.y, movementInput.y * transform.forward.z);
        }

        private void Look()
        {
            //looking at mouse position
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 lookAt = hit.point;
                lookAt.y = transform.position.y;
                var rot = Quaternion.LookRotation(lookAt - transform.position);
                Tween.Rotation(transform, rot, 0.1f);
                lastMouseHitPoint = hit.point;
            }
        }


        public void OnCollisionEnter(Collision collision)
        {
            onCollisionEnter?.Invoke(collision);
        }

        public void OnTriggerEnter(Collider other)
        {
            onTriggerEnter?.Invoke(other);
        }

        public void OnCollisionExit(Collision collision)
        {
            onCollisionExit?.Invoke(collision);
        }

        public void OnTriggerExit(Collider other)
        {
            onTriggerExit?.Invoke(other);
        }

    }
}