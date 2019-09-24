using UnityEditor;

namespace mulova.audio
{
    [CustomEditor(typeof(AudioTrigger))]
	public class AudioTriggerInspector : Editor {
		private AudioTriggerInspectorImpl impl;
		private AudioTrigger trigger;
		private AudioGroup[] groups;
		private AudioGroup selectedGroup;
//		private string[] clips = new string[0];
		
		void OnEnable() {
			impl = new AudioTriggerInspectorImpl(target as AudioTrigger);
		}

		public override void OnInspectorGUI() {
			impl.DrawManageTableGUI();
			impl.OnInspectorGUI();
		}
	}
}
