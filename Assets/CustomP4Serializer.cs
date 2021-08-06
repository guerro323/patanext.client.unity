/*using Revolution;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;
using Unity.Networking.Transport.LowLevel.Unsafe;

namespace DefaultNamespace
{
	public unsafe class CustomP4Serializer : CustomSnapshotSerializer
	{
		public DefaultSnapshotSerializer Default = new DefaultSnapshotSerializer();

		public override void ClearChunks(uint systemId, IDynamicSnapshotSystem system)
		{
			Default.ClearChunks(systemId, system);
		}

		public override void AddChunk(uint systemId, IDynamicSnapshotSystem system, ArchetypeChunk chunk)
		{
			Default.AddChunk(systemId, system, chunk);
		}

		public override void Serialize(SerializeClientData jobData, NativeList<SortDelegate<OnSerializeSnapshot>> delegateSerializers, DataStreamWriter writer, NativeList<byte> outgoing, bool debugRange)
		{
			new SerializeJob
			{
				ClientData   = jobData,
				StreamWriter = writer,
				OutgoingData = outgoing,
			}.Run();
		}

		public override void ClearGhosts(uint systemId)
		{
			Default.ClearGhosts(systemId);
		}

		public override void AddGhost(uint systemId, uint ghostId)
		{
			Default.AddGhost(systemId, ghostId);
		}

		public override void RemoveGhost(uint systemId, uint ghostId)
		{
			Default.RemoveGhost(systemId, ghostId);
		}

		public override void Deserialize(DeserializeClientData jobData, NativeList<SortDelegate<OnDeserializeSnapshot>> delegateDeserializers, NativeArray<byte> data, NativeArray<DataStreamReader.Context> readCtxArray, bool debugRange)
		{
			Default.Deserialize(jobData, delegateDeserializers, data, readCtxArray, debugRange);
		}

		public override void Dispose()
		{
			throw new System.NotImplementedException();
		}

		[BurstCompile]
		public struct SerializeJob : IJob
		{
			public SerializeClientData ClientData;

			public DataStreamWriter StreamWriter;
			public NativeList<byte> OutgoingData;

			public void Execute()
			{
				var parameters = new SerializeParameters
				{
					m_ClientData = new Blittable<SerializeClientData>(ref ClientData),
					m_Stream     = new Blittable<DataStreamWriter>(ref StreamWriter)
				};

				parameters.SystemId = 1;
				Patapon.Mixed.GameModes.GameModeHudSettings.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 3;
				Patapon.Mixed.GameModes.VSHeadOn.HeadOnStructure.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 4;
				Patapon.Mixed.GameModes.VSHeadOn.MpVersusHeadOn.Synchronizer.Serialize(ref parameters);
				parameters.SystemId = 5;
				Patapon.Mixed.GameModes.VSHeadOn.VersusHeadOnUnit.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 6;
				Patapon.Mixed.GamePlay.Abilities.CTate.TaterazayBasicAttackAbility.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 7;
				Patapon.Mixed.GamePlay.Abilities.CTate.BasicTaterazayDefendAbility.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 8;
				Patapon.Mixed.GamePlay.Abilities.CTate.BasicTaterazayDefendFrontalAbility.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 9;
				Patapon.Mixed.GamePlay.Abilities.CYari.BasicYaridaAttackAbility.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 10;
				Patapon.Mixed.GamePlay.Abilities.CYari.BasicYaridaDefendAbility.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 11;
				Patapon.Mixed.GamePlay.Abilities.DefaultBackwardAbility.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 12;
				Patapon.Mixed.GamePlay.Abilities.DefaultJumpAbility.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 13;
				Patapon.Mixed.GamePlay.Abilities.DefaultMarchAbility.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 14;
				Patapon.Mixed.GamePlay.Abilities.DefaultPartyAbility.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 15;
				Patapon.Mixed.GamePlay.Abilities.DefaultRetreatAbility.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 16;
				Patapon.Mixed.GamePlay.AbilityRhythmState.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 17;
				Patapon.Mixed.GamePlay.RhythmEngine.GameComboState.Synchronize.Serialize(ref parameters);
				parameters.SystemId = 18;
				Patapon.Mixed.GamePlay.RhythmEngine.GameCommandState.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 19;
				Patapon.Mixed.GamePlay.Units.RhythmCurrentCommand.Synchronize.Serialize(ref parameters);
				parameters.SystemId = 21;
				Patapon.Mixed.RhythmEngine.Definitions.RhythmCommandDefinition.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 22;
				Patapon.Mixed.RhythmEngine.Flow.FlowEngineProcess.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 23;
				Patapon.Mixed.RhythmEngine.RhythmEngineDescription.Synchronize.Serialize(ref parameters);
				parameters.SystemId = 24;
				Patapon.Mixed.RhythmEngine.RhythmEngineSettings.Sync.Serialize(ref parameters);
				parameters.SystemId = 25;
				Patapon.Mixed.RhythmEngine.RhythmEngineState.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 26;
				Patapon.Mixed.Units.Statistics.UnitCurrentKit.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 27;
				Patapon.Mixed.Units.UnitAppliedArmyFormation.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 29;
				Patapon.Mixed.Units.UnitDirection.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 30;
				Patapon.Mixed.Units.UnitDisplayedEquipment.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 31;
				Patapon.Mixed.Units.UnitEnemySeekingState.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 33;
				Patapon.Mixed.Units.UnitStatistics.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 34;
				Patapon.Mixed.Units.UnitTargetDescription.RelativeSync.Serialize(ref parameters);
				parameters.SystemId = 36;
				Patapon4TLB.Core.MasterServer.P4.EntityDescription.MasterServerP4UnitMasterServerEntity.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 37;
				Patapon4TLB.Core.Snapshots.UnitTranslationSnapshot.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 38;
				Patapon4TLB.Core.Snapshots.TranslationDirectSnapshot.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 39;
				Patapon4TLB.Core.Snapshots.TranslationInterpolatedSnapshot.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 41;
				Patapon4TLB.Default.FormationParent.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 44;
				Patapon4TLB.Default.Player.GamePlayerCommand.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 45;
				Patapon4TLB.Default.TargetSceneAsset.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 48;
				Stormium.Core.Projectiles.ProjectileExplodedEndReason.Sync.Serialize(ref parameters);
				parameters.SystemId = 50;
				StormiumTeam.GameBase.ActionDescription.Sync.Serialize(ref parameters);
				parameters.SystemId = 51;
				StormiumTeam.GameBase.ActionHolderDescription.Sync.Serialize(ref parameters);
				parameters.SystemId = 52;
				StormiumTeam.GameBase.AimLookState.SynchronizeSnapshot.Serialize(ref parameters);
				parameters.SystemId = 53;
				StormiumTeam.GameBase.CharacterDescription.Sync.Serialize(ref parameters);
				parameters.SystemId = 54;
				StormiumTeam.GameBase.Components.ClubDescription.RelativeSync.Serialize(ref parameters);
				parameters.SystemId = 55;
				StormiumTeam.GameBase.Components.ClubInformation.SynchronizeSnapshot.Serialize(ref parameters);
				parameters.SystemId = 56;
				StormiumTeam.GameBase.Components.DefaultHealthData.SynchronizeSnapshot.Serialize(ref parameters);
				parameters.SystemId = 58;
				StormiumTeam.GameBase.Components.HealthDescription.Sync.Serialize(ref parameters);
				parameters.SystemId = 59;
				StormiumTeam.GameBase.Components.LivableHealth.Synchronize.Serialize(ref parameters);
				parameters.SystemId = 60;
				StormiumTeam.GameBase.Components.RegenerativeHealthData.SynchronizeSnapshot.Serialize(ref parameters);
				parameters.SystemId = 61;
				StormiumTeam.GameBase.Components.TargetDamageEvent.Sync.Serialize(ref parameters);
				parameters.SystemId = 62;
				StormiumTeam.GameBase.Data.ExecutingServerMap.NetSync.Serialize(ref parameters);
				parameters.SystemId = 63;
				StormiumTeam.GameBase.ExecutingGameMode.NetSynchronize.Serialize(ref parameters);
				parameters.SystemId = 64;
				StormiumTeam.GameBase.GamePlayerSnapshot.System.Serialize(ref parameters);
				parameters.SystemId = 66;
				StormiumTeam.GameBase.HitShapeDescription.Sync.Serialize(ref parameters);
				parameters.SystemId = 67;
				StormiumTeam.GameBase.LivableDescription.Sync.Serialize(ref parameters);
				parameters.SystemId = 68;
				StormiumTeam.GameBase.MovableDescription.Sync.Serialize(ref parameters);
				parameters.SystemId = 69;
				StormiumTeam.GameBase.Owner.Sync.Serialize(ref parameters);
				parameters.SystemId = 70;
				StormiumTeam.GameBase.OwnerServerId.NetSynchronization.Serialize(ref parameters);
				parameters.SystemId = 71;
				StormiumTeam.GameBase.PlayerDescription.Sync.Serialize(ref parameters);
				parameters.SystemId = 72;
				StormiumTeam.GameBase.ProjectileDescription.Sync.Serialize(ref parameters);
				parameters.SystemId = 73;
				StormiumTeam.GameBase.ServerCameraState.System.Serialize(ref parameters);
				parameters.SystemId = 74;
				StormiumTeam.GameBase.Snapshots.ActionAmmoSnapshot.Synchronize.Serialize(ref parameters);
				parameters.SystemId = 75;
				StormiumTeam.GameBase.TeamDescription.Sync.Serialize(ref parameters);
				parameters.SystemId = 76;
				StormiumTeam.GameBase.Velocity.System.Serialize(ref parameters);

				StreamWriter.Write(0);

				OutgoingData.AddRange(StreamWriter.GetUnsafePtr(), StreamWriter.Length);
			}
		}
	}
}*/