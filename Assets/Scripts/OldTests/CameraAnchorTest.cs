using package.patapon.core;
using package.stormiumteam.shared;
using package.patapon.core;
using StormiumTeam.GameBase.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{ 
    [AlwaysUpdateSystem]
    public class CameraAnchorTest : ComponentSystem
    {
        public Entity TargetId;

        private EntityQuery m_CameraQuery;
        
        protected override void OnCreate()
        {
            base.OnCreate();

            m_CameraQuery = GetEntityQuery(typeof(Camera), typeof(GameCamera));
        }

        protected override void OnUpdate()
        {
            if (m_CameraQuery.CalculateLength() == 0)
                return;

            var cameraEntity = m_CameraQuery.GetSingletonEntity();
            
            if (TargetId == Entity.Null)
            {
                var firstCameraId = m_CameraQuery.GetSingletonEntity();

                TargetId = EntityManager.CreateEntity();
                TargetId.SetOrAddComponentData(new CameraTargetData(firstCameraId, 1));
                TargetId.SetOrAddComponentData(new CameraTargetAnchor(AnchorType.Screen, new float2(0, 0.825f)));
                TargetId.SetOrAddComponentData(new CameraTargetPosition());
            }

            var camera = EntityManager.GetComponentObject<Camera>(cameraEntity);

            GetCharacter();

            if (HasCharacter())
            {
                UpdateCharacterCamera();
                return;
            }

            var position = TargetId.GetComponentData<CameraTargetPosition>().Value;

            if (Input.GetKey(KeyCode.LeftArrow))
                position -= math.float3(2.5f * Time.deltaTime, 0, 0);

            if (Input.GetKey(KeyCode.RightArrow))
                position += math.float3(2.5f * Time.deltaTime, 0, 0);

            camera.orthographicSize += (Input.GetKey(KeyCode.KeypadPlus) ? 1 : Input.GetKey(KeyCode.KeypadMinus) ? -1 : 0)
                                                         * Time.deltaTime;

            var anchor = TargetId.GetComponentData<CameraTargetAnchor>();
            anchor.Value.y += (Input.GetKey(KeyCode.I) ? 0.25f : Input.GetKey(KeyCode.K) ? -0.25f : 0)
                              * Time.deltaTime;
            TargetId.SetOrAddComponentData(anchor);

            TargetId.SetComponentData(new CameraTargetPosition(position));
        }

        public CharacterController2D Character;
        public Vector3 InterpolatedTarget;

        private void GetCharacter()
        {
            Character = Object.FindObjectOfType<CharacterController2D>();
        }

        private bool HasCharacter()
        {
            return Character != null;
        }

        private void UpdateCharacterCamera()
        {
            var characterPosition = Character.transform.position;

            InterpolatedTarget = Vector3.MoveTowards
            (
                Vector3.Lerp(InterpolatedTarget, characterPosition, Time.deltaTime * 5),
                characterPosition,
                Time.deltaTime * 5
            );
            InterpolatedTarget.y = 0;
            
            TargetId.SetComponentData(new CameraTargetPosition(InterpolatedTarget - (Vector3.left * 2.5f)));
        }
    }
}