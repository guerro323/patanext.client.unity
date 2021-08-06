/*using System;
using Karambolo.Common;
using Revolution;
using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
	public class CreateSystemFile : ComponentSystem
	{
		private bool m_created;

		protected override void OnUpdate()
		{
			if (m_created)
				return;

			var snapshotMgr = World.GetExistingSystem<SnapshotManager>();
			if (snapshotMgr.SystemsToId.Count == 0)
				return;
			
			//snapshotMgr.CustomSerializer = new CustomSerializer();

			m_created = true;

			var str = "S Y S T E M  L I S T  F O R  S N A P S H O T\n";
			foreach (var kvp in snapshotMgr.SystemsToId.ToOrderedDictionary(k => k.Key, v => v.Value))
			{
				var id     = kvp.Value;
				var system = kvp.Key;
				if (system == null)
					continue;
				if (system is ISystemDelegateForSnapshot)
				{
					var typeName = system.GetType().FullName.Replace("+", ".");
					var atComma  = typeName.IndexOf(',');
					
					if (atComma >= 0) typeName = typeName.Substring(0, atComma);

					str += $"parameters.SystemId = {id};\n";
					str += $"{typeName}.Serialize(ref parameters);\n";
				}
			}

			Debug.Log(str);
		}

		public static string GetFriendlyName(Type type)
		{
			string friendlyName = type.Name;
			if (type.IsGenericType)
			{
				int iBacktick = friendlyName.IndexOf('`');
				if (iBacktick > 0)
				{
					friendlyName = friendlyName.Remove(iBacktick);
				}
				friendlyName += "<";
				Type[] typeParameters = type.GetGenericArguments();
				for (int i = 0; i < typeParameters.Length; ++i)
				{
					string typeParamName = GetFriendlyName(typeParameters[i]);
					friendlyName += (i == 0 ? typeParamName : "," + typeParamName);
				}
				friendlyName += ">";
			}

			return friendlyName;
		}
	}
}*/