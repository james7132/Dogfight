﻿using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {
	public abstract class AbstractMovementPattern : PausableGameObject {
		
		/// <summary>
		/// The destroy on end.
		/// </summary>
		[SerializeField]
		private bool destroyOnEnd;
		public bool DestroyOnEnd {
			get {
				return destroyOnEnd;
			}
			set {
				destroyOnEnd = value;
			}
		}

		public void StartMovement() {
			StartCoroutine (MoveImpl ());
		}

		private IEnumerator MoveImpl() {
			yield return StartCoroutine(Move());
			if(destroyOnEnd) {
				Destroy (gameObject);
			}
		}

		protected abstract IEnumerator Move();
	}
}