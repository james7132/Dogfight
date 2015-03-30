using UnityEngine;
using UnityUtilLib;
using System.Collections;

namespace Danmaku2D {
	public class PlayerGrazeHitbox : MonoBehaviour, IDanmakuCollider{

		private DanmakuPlayer player;

		void Start() {
			player = GetComponentInParent<DanmakuPlayer> ();
			if (player == null) {
				Debug.LogError("PlayerGrazeHitbox should be on a child object of a GameObject with an Avatar sublcass script");
			}
		}

		#region IProjectileCollider implementation
		public void OnProjectileCollision (Danmaku proj) {
			//throw new System.NotImplementedException ();
		}
		#endregion

		//TODO: FIX

		void OnTriggerExit2D(Collider2D other) {
	//		if (player != null) {
	//			Projectile proj = other.GetComponent<Projectile>();
	//			if(proj != null) {
	//				player.Graze(proj);
	//			}
	//		}
		}

	//	void OnBulletCollision(ProjectileData other) {
	//		if (player != null) {
	//			player.Graze();
	//		}
	//	}
	}
}