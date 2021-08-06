using System.Collections.Generic;
using System.Runtime.InteropServices;
using package.patapon.core;
using package.patapon.core.RhythmCommandBehavior;
using package.patapon.def.Data;
using package.stormiumteam.shared;
using Scripts;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Experimental.U2D;
using UnityEngine.Rendering;

namespace DefaultNamespace
{
    #if FALSE
    [DisableAutoCreation]
    public class RhythmEngineTestBase : ComponentSystem
    {
        protected static Entity ShardRhythmEngine;
        protected static Entity ShardRhythmCommandMgr;
        protected static NativeArray<FlowCommandSequence>[] AllCommands;

        [Inject] protected FlowRhythmEngine CustomRhythmEngine;
        [Inject] protected FlowCommandManager CustomCommandMgr;

        protected override void OnUpdate()
        {
        }
    }

    [DisableAutoCreation]
    public class RhythmEngineTestCreateShard : RhythmEngineTestBase
    {
        protected override void OnCreateManager()
        {
            ShardRhythmEngine = CustomRhythmEngine.AddEngine();
            ShardRhythmCommandMgr = CustomCommandMgr.AddEngine();

            EntityManager.SetComponentData(ShardRhythmCommandMgr, new FlowCommandManagerSettingsData(4));

            AllCommands = new NativeArray<FlowCommandSequence>[9];
            
            // Pata Pata Pata Pon
            AllCommands[0] = new NativeArray<FlowCommandSequence>(4, Allocator.Persistent)
            {
                [0] = new FlowCommandSequence(0, FlowRhythmEngine.KeyPata),
                [1] = new FlowCommandSequence(1, FlowRhythmEngine.KeyPata),
                [2] = new FlowCommandSequence(2, FlowRhythmEngine.KeyPata),
                [3] = new FlowCommandSequence(3, FlowRhythmEngine.KeyPon)
            };
            
            // Pon Pon Pata Pon
            AllCommands[1] = new NativeArray<FlowCommandSequence>(4, Allocator.Persistent)
            {
                [0] = new FlowCommandSequence(0, FlowRhythmEngine.KeyPon),
                [1] = new FlowCommandSequence(1, FlowRhythmEngine.KeyPon),
                [2] = new FlowCommandSequence(2, FlowRhythmEngine.KeyPata),
                [3] = new FlowCommandSequence(3, FlowRhythmEngine.KeyPon)
            };
            
            // Chaka Chaka Pata Pon
            AllCommands[2] = new NativeArray<FlowCommandSequence>(4, Allocator.Persistent)
            {
                [0] = new FlowCommandSequence(0, FlowRhythmEngine.KeyChaka),
                [1] = new FlowCommandSequence(1, FlowRhythmEngine.KeyChaka),
                [2] = new FlowCommandSequence(2, FlowRhythmEngine.KeyPata),
                [3] = new FlowCommandSequence(3, FlowRhythmEngine.KeyPon)
            };
            
            // Pon Pata Pon Pata
            AllCommands[3] = new NativeArray<FlowCommandSequence>(4, Allocator.Persistent)
            {
                [0] = new FlowCommandSequence(0, FlowRhythmEngine.KeyPon),
                [1] = new FlowCommandSequence(1, FlowRhythmEngine.KeyPata),
                [2] = new FlowCommandSequence(2, FlowRhythmEngine.KeyPon),
                [3] = new FlowCommandSequence(3, FlowRhythmEngine.KeyPata)
            };
            
            // Pon Pon Chaka Chaka
            AllCommands[4] = new NativeArray<FlowCommandSequence>(4, Allocator.Persistent)
            {
                [0] = new FlowCommandSequence(0, FlowRhythmEngine.KeyPon),
                [1] = new FlowCommandSequence(1, FlowRhythmEngine.KeyPon),
                [2] = new FlowCommandSequence(2, FlowRhythmEngine.KeyChaka),
                [3] = new FlowCommandSequence(3, FlowRhythmEngine.KeyChaka)
            };
            
            // Don Don Chaka Chaka
            AllCommands[5] = new NativeArray<FlowCommandSequence>(4, Allocator.Persistent)
            {
                [0] = new FlowCommandSequence(0, FlowRhythmEngine.KeyDon),
                [1] = new FlowCommandSequence(1, FlowRhythmEngine.KeyDon),
                [2] = new FlowCommandSequence(2, FlowRhythmEngine.KeyChaka),
                [3] = new FlowCommandSequence(3, FlowRhythmEngine.KeyChaka)
            };
            
            // Pata Pon Don Chaka
            AllCommands[6] = new NativeArray<FlowCommandSequence>(4, Allocator.Persistent)
            {
                [0] = new FlowCommandSequence(0, FlowRhythmEngine.KeyPata),
                [1] = new FlowCommandSequence(1, FlowRhythmEngine.KeyPon),
                [2] = new FlowCommandSequence(2, FlowRhythmEngine.KeyDon),
                [3] = new FlowCommandSequence(3, FlowRhythmEngine.KeyChaka)
            };
            
            // Don DonDon DonDon
            AllCommands[7] = new NativeArray<FlowCommandSequence>(5, Allocator.Persistent)
            {
                [0] = new FlowCommandSequence(0, FlowRhythmEngine.KeyDon),
                [1] = new FlowCommandSequence(1, 1, FlowRhythmEngine.KeyDon),
                [2] = new FlowCommandSequence(1, 1, FlowRhythmEngine.KeyDon),
                [3] = new FlowCommandSequence(2, 1, FlowRhythmEngine.KeyDon),
                [4] = new FlowCommandSequence(3, 0, FlowRhythmEngine.KeyDon)
            };
            
            // Chaka Pata Chaka Pata
            AllCommands[8] = new NativeArray<FlowCommandSequence>(4, Allocator.Persistent)
            {
                [0] = new FlowCommandSequence(0, FlowRhythmEngine.KeyChaka),
                [1] = new FlowCommandSequence(1, FlowRhythmEngine.KeyPata),
                [2] = new FlowCommandSequence(2, FlowRhythmEngine.KeyChaka),
                [3] = new FlowCommandSequence(3, FlowRhythmEngine.KeyPata)
            };
        }

        protected override void OnDestroyManager()
        {
            for (int i = 0; i != AllCommands.Length; i++)
                AllCommands[i].Dispose();
        }
    }

    [DisableAutoCreation]
    [UpdateAfter(typeof(RhythmEngineTestCreateShard))]
    public class RhythmEngineTestAddPressure : RhythmEngineTestBase
    {
        protected override void OnUpdate()
        {
            // Pata
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                CustomRhythmEngine.AddPressure(ShardRhythmEngine, FlowRhythmEngine.KeyPata, PostUpdateCommands);
            }
            // Pon
            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                CustomRhythmEngine.AddPressure(ShardRhythmEngine, FlowRhythmEngine.KeyPon, PostUpdateCommands);
            }
            // Don
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                CustomRhythmEngine.AddPressure(ShardRhythmEngine, FlowRhythmEngine.KeyDon, PostUpdateCommands);
            }
            // Chaka
            else if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                CustomRhythmEngine.AddPressure(ShardRhythmEngine, FlowRhythmEngine.KeyChaka, PostUpdateCommands);
            }
        }
    }

    [DisableAutoCreation]
    [UpdateAfter(typeof(RhythmEngineTestAddPressure))]
    public class RhythmEngineTestCheckEvents : RhythmEngineTestBase,
                                               INativeEventOnGUI
    {
        private struct PressureGroup
        {
            public ComponentDataArray<RhythmShardEvent>           ShardEventArray;
            public ComponentDataArray<RhythmShardTarget>          ShardTargetArray;
            public ComponentDataArray<FlowRhythmPressureData> PressureArray;

            public readonly int Length;
        }

        private struct BeatGroup
        {
            public ComponentDataArray<RhythmShardEvent>       ShardEventArray;
            public ComponentDataArray<RhythmShardTarget>      ShardTargetArray;
            public ComponentDataArray<FlowRhythmBeatData> BeatArray;

            public readonly int Length;
        }

        private struct ComponentLessCharacter
        {
            public ComponentArray<CharacterMovementTest> Characters;

            public EntityArray Entities;
            
            public readonly int Length;
        }

        private struct CharacterGroup
        {
            public ComponentArray<CharacterMovementTest> Characters;
            public ComponentDataArray<RhythmShardTarget> RhythmShardTargetArray;
            public EntityArray Entities;
            
            public readonly int Length;
        }

        [Inject] private PressureGroup m_PressureGroup;
        [Inject] private BeatGroup     m_BeatGroup;
        [Inject] private ComponentLessCharacter m_ComponentLessCharacter;
        [Inject] private CharacterGroup m_CharacterGroup;

        [Inject] private AppEventSystem m_AppEventSystem;

        private AudioSource AudioSourceOnNewBeat;
        private AudioSource AudioSourceOnNewPressure;

        protected AudioClip                                   AudioOnNewBeat;
        protected Dictionary<int, Dictionary<int, AudioClip>> AudioOnPressure;

        protected bool UsePanSound;
        protected float LastTime, LastScore;
        protected int LastKeyBeat;

        protected List<FlowRhythmPressureData> CurrentCombo;
        protected List<NativeArray<FlowCommandSequence>> PredictedCommands;
        
        protected override void OnCreateManager()
        {
            Addressables.InitializationOperation.Completed += op => { OnLoadAssets(); };

            AudioSourceOnNewBeat               = new GameObject("(Clip) On New Beat", typeof(AudioSource)).GetComponent<AudioSource>();
            AudioSourceOnNewBeat.reverbZoneMix = 0f;
            AudioSourceOnNewBeat.spatialBlend  = 0f;
            AudioSourceOnNewBeat.volume        = 0.25f;

            AudioSourceOnNewPressure               = new GameObject("(Clip) On New Pressure", typeof(AudioSource)).GetComponent<AudioSource>();
            AudioSourceOnNewPressure.reverbZoneMix = 0f;
            AudioSourceOnNewPressure.spatialBlend  = 0f;
            AudioSourceOnNewPressure.volume        = 0.25f;
            
            CurrentCombo = new List<FlowRhythmPressureData>(4);
            PredictedCommands = new List<NativeArray<FlowCommandSequence>>(3);

            m_AppEventSystem.SubscribeToAll(this);
        }

        protected void OnLoadAssets()
        {
            Addressables.LoadAsset<AudioClip>("int:RhythmEngine/Sounds/on_new_beat.ogg")
                        .Completed += op => AudioOnNewBeat = op.Result;

            AudioOnPressure = new Dictionary<int, Dictionary<int, AudioClip>>(12);

            for (int i = 0; i != 4; i++)
            {
                var key = i + 1;

                AudioOnPressure[key] = new Dictionary<int, AudioClip>(3);

                for (int r = 0; r != 3; r++)
                {
                    var rank = r;

                    Addressables.LoadAsset<AudioClip>($"int:RhythmEngine/Sounds/drum_{key}_{rank}.ogg").Completed += op =>
                    {
                        Debug.Assert(op.IsValid, "op.IsValid");

                        AudioOnPressure[key][rank] = op.Result;
                    };
                }
            }
        }

        protected override void OnUpdate()
        {
            var currentFrame = Time.frameCount;

            for (int i = 0; i != m_ComponentLessCharacter.Length; i++)
            {
                var e = m_ComponentLessCharacter.Entities[i];
                
                EntityManager.AddComponentData(e, new RhythmShardTarget(ShardRhythmEngine));
            }

            if (UsePanSound)
            {
                AudioSourceOnNewBeat.panStereo     = -1f;
                AudioSourceOnNewPressure.panStereo = 1f;
            }
            else
            {
                AudioSourceOnNewBeat.panStereo     = 0f;
                AudioSourceOnNewPressure.panStereo = 0f;
            }

            var time = (float)ShardRhythmEngine.GetComponentData<FlowRhythmEngineProcessData>().Time;
            
            var lastBeat = -1;
            
            for (int i = 0; i != m_BeatGroup.Length; i++)
            {
                var beatData = m_BeatGroup.BeatArray[i];

                //Debug.Log($"(New beat) Key: {beatData.Beat}");

                if (AudioOnNewBeat != null)
                {
                    AudioSourceOnNewBeat.PlayOneShot(AudioOnNewBeat);
                }

                lastBeat = beatData.Beat;
            }

            for (int i = 0; i != m_PressureGroup.Length; i++)
            {
                var pressureData = m_PressureGroup.PressureArray[i];

                //Debug.Log($"(New pressure) Key: {pressureData.KeyId}, Score: {pressureData.Score}, Beat: {pressureData.CorrectedBeat}");

                var key = pressureData.KeyId;
                int rank;
                if (pressureData.GetAbsoluteScore() < 0.15f)
                    rank = 0;
                else
                    rank = 1;

                LastKeyBeat = pressureData.CorrectedBeat;
                LastTime    = time;
                LastScore   = pressureData.Score;

                CurrentCombo.Add(pressureData);

                if (AudioOnPressure[key][rank] != null)
                {
                    AudioSourceOnNewPressure.clip = (AudioOnPressure[key][rank]);
                    AudioSourceOnNewPressure.Play();
                }
                
                
            }

            // Happen only on a new beat
            if (lastBeat != -1)
            {                                
                PredictedCommands.Clear();
                
                // 
                var array = CheckValidCommand(PredictedCommands, lastBeat, out var _, out var cmdId);
                if (array.IsCreated)
                {
                    CurrentCombo.Clear();
                    AudioSourceOnNewPressure.Play();
                    
                    //EntityManager.SetComponentData(ShardRhythmCommandMgr, new FlowCurrentCommand(cmdId, lastBeat, true));
                }
                
                // Clear old pressures from combo
                ResizeCombo(lastBeat);
            }

            var flowCurrentCommand = EntityManager.GetComponentData<FlowCurrentCommand>(ShardRhythmCommandMgr);
            var rhythmEngineProcessData = EntityManager.GetComponentData<FlowRhythmEngineProcessData>(ShardRhythmEngine);
            
            for (int i = 0; i != m_CharacterGroup.Length; i++)
            {
                var movement = m_CharacterGroup.Characters[i];
                var entity   = m_CharacterGroup.Entities[i];
                    
                // totally bad vvvv
                if (flowCurrentCommand.IsActive == 1 && flowCurrentCommand.ActiveAtBeat + 3 >= rhythmEngineProcessData.Beat)
                {
                    /*if (flowCurrentCommand.CommandId == 0 && !EntityManager.HasComponent(entity, typeof(RhythmDefaultMarchBehavior)))
                    {
                        PostUpdateCommands.AddComponent(entity, new RhythmDefaultMarchBehavior());
                    }*/
                }
                else
                {
                    if (EntityManager.HasComponent(entity, typeof(RhythmDefaultMarchBehavior)))
                        PostUpdateCommands.RemoveComponent<RhythmDefaultMarchBehavior>(entity);
                }
            }
        }

        public void ResizeCombo(int lastBeat)
        {
            for (int comboIndex = 0; comboIndex != CurrentCombo.Count; comboIndex++)
            {
                var current = CurrentCombo[comboIndex];
                if (current.CorrectedBeat + 4 <= lastBeat)
                {
                    CurrentCombo.RemoveAt(comboIndex);

                    comboIndex--;
                }
            }
        }

        public NativeArray<FlowCommandSequence> CheckValidCommand(List<NativeArray<FlowCommandSequence>> predictedCommands, int currentBeat, out bool canConcept, out int cmdId)
        {
            var validCommand = default(NativeArray<FlowCommandSequence>);

            canConcept = true;
            cmdId = -1;
            
            var startComboIndex = 0;
            for (int i = 0; i != CurrentCombo.Count; i++)
            {
                if (CurrentCombo[i].CorrectedBeat == currentBeat)
                {
                    startComboIndex = i;
                    break;
                }
            }

            for (var commandIndex = 0; commandIndex != AllCommands.Length; commandIndex++)
            {
                var command = AllCommands[commandIndex];

                var matchedKeys = 0;
                var canBePredicted = false;

                for (var comboIndex = startComboIndex; comboIndex != CurrentCombo.Count; comboIndex++)
                {
                    if (command.Length <= comboIndex)
                        break;

                    var pressure = CurrentCombo[comboIndex];
                    var pressureBeatZeroOffset = pressure.CorrectedBeat - CurrentCombo[startComboIndex].CorrectedBeat;
                    
                    var beatRange = CustomCommandMgr.GetBeatAt(ShardRhythmCommandMgr, command, comboIndex);
                    var seq = command[comboIndex];
                    
                    //Debug.Log($"{beatRange.start} <= {pressureBeatZeroOffset} <= {beatRange.end}, {seq.Key} == {pressure.KeyId}");

                    if (beatRange.start <= pressureBeatZeroOffset && pressureBeatZeroOffset <= beatRange.end && seq.Key == pressure.KeyId)
                    {                   
                        canBePredicted = true;

                        matchedKeys++;
                    }
                    else
                    {
                        canBePredicted = false;

                        break;
                    }
                }

                if (canBePredicted)
                    predictedCommands.Add(command);

                if (matchedKeys == command.Length)
                {
                    canConcept = false;
                    cmdId = commandIndex;
                    
                    return command;
                }
            }

            if (predictedCommands.Count > 0) canConcept = false;

            return default(NativeArray<FlowCommandSequence>);
        }

        public void NativeOnGUI()
        {
            return;
        
            using (new GUILayout.VerticalScope())
            {
                GUI.contentColor = Color.black;
                UsePanSound      = GUILayout.Toggle(UsePanSound, "Make the metronome sounds on left and drum sounds on right");

                string combo = string.Empty;
                foreach (var c in CurrentCombo)
                {
                    combo += $"({c.CorrectedBeat}, {c.KeyId})";
                }

                string predicted = string.Empty;
                foreach (var c in PredictedCommands)
                {
                    var isBeatDifferent = false;
                    var lastBeat        = -1;

                    foreach (var a in c)
                    {
                        var close = false;

                        if (lastBeat != a.BeatEnd)
                        {
                            if (isBeatDifferent)
                            {
                                predicted += ")";
                            }

                            isBeatDifferent = true;
                            lastBeat        = a.BeatEnd;

                            predicted += "(";
                        }

                        predicted += $"{a.Key}";
                    }

                    predicted += "); ";
                }

                GUILayout.Label("Current Combo: " + combo);
                GUILayout.Label("Predicted: " + predicted);
            }
        }
    }
    #endif
}