namespace DefaultNamespace
{
	/*[UpdateInWorld(UpdateInWorld.TargetWorld.Default)]
	[AlwaysUpdateSystem]
	public class DiscordBoot : ComponentSystem
	{
		private Discord.Discord m_Discord;

		private enum CurrentDetails
		{
			InMenu,
			InMatch
		}

		private CurrentDetails m_Details;

		private Activity m_Activity;

		public EntityQuery m_GameModeQuery;

		private float m_ReloadTime;

		public unsafe Activity Activity
		{
			get => m_Activity;
			set
			{
				var prev = m_Activity;

				m_Activity = value;
				if (UnsafeUtility.MemCmp(UnsafeUtility.AddressOf(ref prev), UnsafeUtility.AddressOf(ref value), UnsafeUtility.SizeOf<Activity>()) != 0)
				{
					m_Discord
						.GetActivityManager()
						.UpdateActivity(value, (res) => { Debug.Log($"[t:{Time.time}] RES -> [{res}]"); });
				}
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();

			m_Discord = new Discord.Discord(609427243395055616, (uint) Discord.CreateFlags.Default);
			UpdateActivity(String.Empty, null);

			m_Discord.SetLogHook(LogLevel.Debug, (level, message) => Debug.Log($"[{level}] {message}"));
		}

		protected override void OnUpdate()
		{
			m_Discord.RunCallbacks();

			var user = m_Discord.GetUserManager()?.GetCurrentUser();
			if (user != null)
			{
				User nnUser = (User) user;
				Debug.Log($"{nnUser.Username}#{nnUser.Discriminator} [{nnUser.Id}] [avatar={nnUser.Avatar}]");
			}

			foreach (var world in World.AllWorlds)
			{
				if (world.GetExistingSystem<ClientPresentationSystemGroup>()?.Enabled == false)
					continue;
				
				DoClientWorld(world);
				return;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			m_Discord.GetActivityManager().ClearActivity(result => { });
			m_Discord.Dispose();
		}

		private void DoClientWorld(World clientWorld)
		{
			if (m_GameModeQuery == null)
				m_GameModeQuery = clientWorld.EntityManager.CreateEntityQuery(typeof(MpVersusHeadOn));
			
			var mapMgr = clientWorld.GetOrCreateSystem<MapManager>();
			if (mapMgr.IsMapLoaded && m_Details != CurrentDetails.InMatch)
			{
				m_Details = CurrentDetails.InMatch;

				var map = mapMgr.GetSingleton<ExecutingMapData>();
				UpdateActivity(map.Key.ToString(), mapMgr.GetMapFormat(map.Key.ToString()));
			}
			else if (!mapMgr.IsMapLoaded && m_Details == CurrentDetails.InMatch)
			{
				m_Details = CurrentDetails.InMenu;
				UpdateActivity(string.Empty, null);
			}

			if (m_Details == CurrentDetails.InMatch && m_GameModeQuery.CalculateEntityCount() > 0 && m_ReloadTime <= 0.0f)
			{
				var gmData = clientWorld.EntityManager.GetComponentData<MpVersusHeadOn>(m_GameModeQuery.GetSingletonEntity());
				var activity = Activity;
				{
					activity.Details = $"{gmData.Team0Points} - {gmData.Team1Points}";
				}
				Activity = activity;

				m_ReloadTime = 2.5f;
			}

			m_ReloadTime -= Time.DeltaTime;
		}

		private void UpdateActivity(string mapKey, MapManager.JMapFormat? mapFormat)
		{
			Activity = new Activity
			{
				State = "Waiting",
				Details = m_Details == CurrentDetails.InMenu ? "In Menu" :
					m_Details == CurrentDetails.InMatch      ? "In Match" :
					                                           "Unknown",
				Assets = new ActivityAssets
				{
					LargeImage = mapFormat == null ? "in-menu" : $"map_thumb_{mapKey}",
					LargeText  = mapFormat != null ? $"{mapFormat.Value.name}" : string.Empty
				},
				Party = new ActivityParty
				{
					Id = "party",
					Size = new PartySize
					{
						CurrentSize = 1,
						MaxSize     = 4
					}
				},
				Secrets = new ActivitySecrets
				{
					Match    = "match",
					Join     = "join",
					Spectate = "spectate"
				},	
				Instance = true
			};
		}
	}*/
}