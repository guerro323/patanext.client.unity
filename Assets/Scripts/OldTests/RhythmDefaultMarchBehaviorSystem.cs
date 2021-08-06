using package.patapon.core;
using package.patapon.core.RhythmCommandBehavior;
using package.patapon.def.Data;
using Unity.Entities;

namespace DefaultNamespace
{
    /*public class RhythmDefaultMarchBehaviorSystem : ComponentSystem
    {
        struct Group
        {
            public ComponentDataArray<RhythmShardTarget>          RhythmShardTargetArray;
            public ComponentDataArray<RhythmDefaultMarchBehavior> MarchBehaviorArray;
            public ComponentArray<CharacterMovementTest>          MovementArray;

            public readonly int Length;
        }

        [Inject] private Group m_Group;

        protected override void OnUpdate()
        {
            for (int i = 0; i != m_Group.Length; i++)
            {
                var behavior    = m_Group.MarchBehaviorArray[i];
                var movement    = m_Group.MovementArray[i];
                var shardTarget = m_Group.RhythmShardTargetArray[i].Target;

                if (!EntityManager.Exists(shardTarget)) continue;

                // get rhythm engine
                var rhythmEngineData = EntityManager.GetComponentData<FlowRhythmEngineProcessData>(shardTarget).Beat;

                // todo: prototype method for now, will replace by a real method soon
                movement.Move(1);
            }
        }
    }*/
}