﻿using System;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using mulova.comunity;
using UnityEngine.Ex;

namespace mulova.effect {
	/// <summary>
	/// Pool for automatically released effect after lifetime
	/// </summary>
	public class EffectPool : LogBehaviour
	{
		private string url;
		private AssetInstancePool pool;

		public void Init(string url) {
			Assert.IsNull(pool);
			this.url = url;
			pool = gameObject.FindComponent<AssetInstancePool>();
			pool.Init(new AssetPool<GameObject>(url));
			EventRegistry.RegisterListener(url, ReleaseEffect);
		}
		
		public void Get<E>(string id, Action<E> callback) where E: Effect{
			if (pool != null) {
				pool.Get(id, (i,o)=> {
					E e = o.FindComponent<E>();
					e.effectId = id;
					e.poolId = url;
					callback(e);
				});
			} else {
				log.Error("Init() is not called");
				callback(null);
			}
		}
		
		public void ShowEffect(string effectName, Transform parent, Action callback) {
			Get<Effect>(effectName, e=> {
				e.gameObject.SetLayer(parent.gameObject.layer);
				e.transform.SetParent(parent, false);
				e.Play(callback);
			});
		}
		
		private void ReleaseEffect(object o) {
			Effect e = o as Effect;
			pool.Put(e.effectId, e.gameObject);
		}
		
		void OnDestroy()
		{
			EventRegistry.DeregisterListener(url, ReleaseEffect);
		}
	}
}
