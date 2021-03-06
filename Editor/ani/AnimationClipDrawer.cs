﻿using System.Text.Ex;
using mulova.unicore;
using UnityEditor;
using UnityEngine;
using UnityEngine.Ex;
using Object = UnityEngine.Object;
using System.Ex;

namespace ani
{
    [CustomPropertyDrawer(typeof(AnimationClipListingAttribute))]
    public class AnimationClipDrawer : PropertyDrawerBase
	{
		protected override int GetLineCount(SerializedProperty p)
		{
			return 1;
		}

		protected override void OnGUI(SerializedProperty p, Rect r)
        {
            Component c = p.serializedObject.targetObject as Component;
			Animation anim = c.GetComponent<Animation>();
			AnimationClipListingAttribute attr = attribute as AnimationClipListingAttribute;
			if (!attr.varName.IsEmpty()) {
				Object o = c.GetFieldValue<Object>(attr.varName);
				if (o != null) {
					if (o is Animation) {
						anim = o as Animation;
					} else if (o is Component) {
						anim = (o as Component).GetComponent<Animation>();
					} else if (o is GameObject) {
						anim = (o as GameObject).GetComponent<Animation>();
					}
				}
			}
			if (anim != null) {
				AnimationClip[] clips = anim.GetAllClips().ToArray();
				AnimationClip a = p.objectReferenceValue as AnimationClip;
				if (PopupNullable(GetLineRect(0), p.name, ref a, clips)) {
					p.objectReferenceValue = a;
				}
			} else {
				EditorGUI.PropertyField(GetLineRect(0), p, new GUIContent(p.name));
			}
		}
	}
}

