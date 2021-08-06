using System.Diagnostics;
using StormiumTeam.GameBase;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using BoxCollider = Unity.Physics.BoxCollider;
using Collider = Unity.Physics.Collider;
using Debug = UnityEngine.Debug;
using Ray = Unity.Physics.Ray;
using SphereCollider = Unity.Physics.SphereCollider;

namespace DefaultNamespace
{
#if FALSE
	public class TestCollideWith : ComponentSystem
	{
		protected override void OnCreateManager()
		{			
			return;
			
			var origin = EntityManager.CreateEntity();

			var boxCollider = BoxCollider.Create(float3.zero, quaternion.identity, new float3(1, 1, 1), 0.01f);
			var entityCollide1 = EntityManager.CreateEntity(typeof(Translation), typeof(Rotation), typeof(PhysicsCollider), typeof(PhysicsMass));
			EntityManager.SetComponentData(entityCollide1, new Translation {Value = new float3(1.5f, 4.3f, 0.4f)});
			EntityManager.SetComponentData(entityCollide1, new Rotation {Value = Quaternion.identity});
			EntityManager.SetComponentData(entityCollide1, new PhysicsCollider {Value = boxCollider});
			EntityManager.SetComponentData(entityCollide1, PhysicsMass.CreateKinematic(boxCollider.Value.MassProperties));

			var spCollider = SphereCollider.Create(float3.zero, 0.75f);
			var entityCollide2 = EntityManager.CreateEntity(typeof(Translation), typeof(Rotation), typeof(PhysicsCollider), typeof(PhysicsMass));
			EntityManager.SetComponentData(entityCollide2, new Translation {Value = new float3(6.0f, 3.9f, 0.4f)});
			EntityManager.SetComponentData(entityCollide2, new Rotation {Value = Quaternion.identity});
			EntityManager.SetComponentData(entityCollide2, new PhysicsCollider {Value = spCollider});
			EntityManager.SetComponentData(entityCollide2, PhysicsMass.CreateKinematic(spCollider.Value.MassProperties));

			var collideWith = EntityManager.AddBuffer<CollideWith>(origin);
			collideWith.Add(new CollideWith {Target = entityCollide1});
			collideWith.Add(new CollideWith {Target = entityCollide2});
		}

		protected override void OnUpdate()
		{
			return;
			
			var transformCollideWithBufferSystem = World.GetExistingSystem<TransformCollideWithBufferSystem>();
			transformCollideWithBufferSystem.Update();

			Entities.ForEach((DynamicBuffer<CollideWith> collideWithBuffer) =>
			{
				var rayInput = new RaycastInput
				{
					Ray = new Ray(new float3(0, 4, 0), new float3(1, 0, 0) * 10),
					Filter = CollisionFilter.Default
				};

				Debug.DrawRay(rayInput.Ray.Origin, rayInput.Ray.Direction, Color.blue);
				Debug.DrawRay(rayInput.Ray.Origin, Vector3.up, Color.red);

				var cwCollection = new CollideWithCollection(collideWithBuffer);
				if (cwCollection.CastRay(rayInput, out var closestHit))
				{
					Debug.DrawRay(closestHit.Position, closestHit.SurfaceNormal, Color.green);
				}
			});
		}
	}
#endif
}