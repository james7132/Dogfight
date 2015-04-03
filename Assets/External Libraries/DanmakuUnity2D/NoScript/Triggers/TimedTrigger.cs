using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {

	[AddComponentMenu("Danmaku 2D/Triggers/Timed Trigger")]
	public class TimedTrigger : DanmakuTrigger, IPausable {

		#region IPausable implementation
		public bool Paused {
			get;
			set;
		}
		#endregion

		[SerializeField]
		private FrameCounter delay;

		public void Update() {
			if(!Paused) {
				if(delay.Tick()) {
					Trigger();
				}
			}
		}

	}

}
