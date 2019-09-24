using System;
using UnityEngine;
using mulova.comunity;

namespace mulova.effect {
	/// <summary>
	/// Effect needs to call Release() when it is over.
	/// </summary>
	public abstract class Effect : LogBehaviour, IReleasable
	{
		public string poolId;
		public string effectId;

		/// <summary>
		/// NOTE: call Release() right before invoking callback
		/// </summary>
		/// <param name="callback">Callback.</param>
		public abstract void Play(Action callback);

		void OnDisable() {
			Release();
		}
		
		public void Release() {
			EventRegistry.SendEvent(poolId, this);
		}
	}
}


