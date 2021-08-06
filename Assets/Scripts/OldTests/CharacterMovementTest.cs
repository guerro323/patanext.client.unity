using System;
using package.stormiumteam.shared;
using package.patapon.core;
using package.patapon.core.RhythmCommandBehavior;
using package.patapon.def.Data;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public class CharacterMovementTest : MonoBehaviour
    {
        public float MovementSpeed = 8;
        public float GroundAcceleration = 10f;
        public float GroundMinimalSpeed = 2f;
        public float AirAcceleration = 1;
        public float GroundDrag = 1;
        public float AirDrag = 0f;
        
        private CharacterController2D m_CharacterController;
        private Rigidbody2D m_Rigidbody;

        private void Awake()
        {
            var referencable = ReferencableGameObject.GetComponent<ReferencableGameObject>(gameObject);

            var queryCharacterController = referencable.GetComponentFast<CharacterController2D>();
            var queryRigidbody2D = referencable.GetComponentFast<Rigidbody2D>();
            
            if (!queryCharacterController.HasValue)
                throw new Exception("queryCharacterController.HasValue");
            if (!queryRigidbody2D.HasValue)
                throw new Exception("queryRigidbody2D.HasValue");

            m_CharacterController = queryCharacterController.Value;
            m_Rigidbody = queryRigidbody2D.Value;
        }

        private void Update()
        {
            var input = Input.GetAxisRaw("Horizontal");
            var isGrounded = m_CharacterController.IsGrounded();
            
            /*if (m_CharacterController.IsGrounded())
            {
                RunOnGround(m_Rigidbody, input);
                m_Rigidbody.drag = GroundDrag;
            }
            else
            {
                RunInAir(m_Rigidbody, input);
                m_Rigidbody.drag = AirDrag;
            }*/

            // Jump
            if (Input.GetAxisRaw("Vertical") > 0.5f && isGrounded)
            {
                m_Rigidbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            }
            
            if (Input.GetKeyDown(KeyCode.E))
                m_Rigidbody.AddForce(Vector2.left * 12, ForceMode2D.Impulse);
        }

        private void RunOnGround(Rigidbody2D rb, float input)
        {
            var velocityX = rb.velocity.x;
            var wishSpeed = MovementSpeed * math.abs(input);
            var wishDirection = new Vector2(math.sign(input), 0);

            velocityX = Mathf.MoveTowards(velocityX, wishSpeed * wishDirection.x, GroundAcceleration * Time.deltaTime);
            if (math.abs(input) > 0.1f && math.abs(velocityX) < GroundMinimalSpeed)
            {
                velocityX = GroundMinimalSpeed * wishDirection.x;
            }
            
            rb.velocity = new Vector2(velocityX, rb.velocity.y);
        }

        private void RunInAir(Rigidbody2D rb, float input)
        {
            var wishSpeed = input * MovementSpeed;
            var velocity = rb.velocity;
            var lineVelocity = new Vector2(velocity.x, 0);
            
            lineVelocity.x += input * AirAcceleration * Time.deltaTime;
            lineVelocity = ClampSpeed(lineVelocity, velocity, MovementSpeed);
            
            velocity.x = lineVelocity.x;

            rb.velocity = velocity;
        }

        public void Move(float input)
        {
            if (m_CharacterController.IsGrounded())
            {
                RunOnGround(m_Rigidbody, input);
                m_Rigidbody.drag = GroundDrag;
            }
        }

        private Vector3 ClampSpeed(Vector2 lineVelocity, Vector2 velocity, float speed)
        {
            return Vector3.ClampMagnitude(lineVelocity, math.max(math.abs(velocity.x), speed));
        }
    }
}