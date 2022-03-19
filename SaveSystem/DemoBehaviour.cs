using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
	public class DemoBehaviour : MonoBehaviour
	{
		private const string NAMES_SAVE_KEY = "NamesSaveKey";
		[SerializeField] private List<string> _names = new List<string>();

		[ContextMenu("SaveData")]
		private void Save()
		{
			ZPrefs.Set<List<string>>(NAMES_SAVE_KEY, _names, true);
			Debug.Log("--> ZPrefs :: Names Collection Is Saved In PlayerPrefs...");
		}

		[ContextMenu("LoadData")]
		private void LoadData()
		{
			var savedNames = ZPrefs.Get<List<string>>(NAMES_SAVE_KEY, null, true);

			if (savedNames == null)
			{
				Debug.Log("--> ZPrefs :: No Names Found In Prefs....");
				return;
			}

			foreach (var savedName in savedNames)
			{
				Debug.Log($"--> ZPrefs :: Name: {savedName}");
			}
		}
	}
}