using System;
using Etourney.Enums;
using UnityEngine;
using Environment = Etourney.Enums.Environment;

namespace Etourney.Settings
{
	[Serializable]
	public sealed class EtourneySettings : ScriptableObject
	{
        private static EtourneySettings _instance;

        public static EtourneySettings Instance
        {
            get
            {
                return _instance != null ? _instance : _instance = SettingsLoader.Settings;
            }
        }

        [HideInInspector]
		[SerializeField]
		public string GameKey;

        [HideInInspector]
        [SerializeField]
        public string LocalGameKey;

        [HideInInspector]
		[SerializeField]
		public Environment Environment;

        [HideInInspector]
        [SerializeField]
        public Orientation Orientation;

        [HideInInspector]
        [SerializeField]
        public ServerLocation ServerLocation;
    }
}