﻿using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D.ProjectileControllers {

	public class LinearProjectileControl : ControllerWrapperBehavior<LinearProjectile> {
		[SerializeField]
		private LinearProjectile controller;

		#region implemented abstract members of ControllerWrapperBehavior
		protected override LinearProjectile CreateController () {
			return controller;
		}
		#endregion
	}

}

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {
	
	/// <summary>
	/// A ProjectileController or ProjectileGroupController for creating bullets that move along a straight path.
	/// </summary>
	[System.Serializable]
	public class LinearProjectile : IProjectileController {
		
		[SerializeField]
		private float velocity = 5;

		[SerializeField]
		private float acceleration = 0;

		[SerializeField]
		private float capSpeed;

		/// <summary>
		/// Gets or sets the velocity of the controlled Projectile(s)
		/// </summary>
		/// <value>The velocity of the controlled Projectile(s) in absolute world coordinates per second</value>
		public float Velocity {
			get {
				return velocity;
			}
			set {
				velocity = value;
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Danmaku2D.ProjectileControllers.LinearProjectile"/> class.
		/// </summary>
		/// <value>The velocity of the controlled Projectile(s) in absolute world coordinates per second</value>
		public LinearProjectile (float velocity, float acceleration = 0f, float capSpeed = float.NaN) : base() {
			Velocity = velocity;
			SetAcceleration (acceleration, capSpeed);
		}

		public void SetAcceleration(float accel, float cap) {
			if(!float.IsNaN(cap)) {
				if(Util.Sign(accel) == Util.Sign(cap - velocity)) {
					acceleration = accel;
					capSpeed = cap;
				} else {
					acceleration = 0f;
					velocity = cap;
				}
			} else {
				acceleration = 0f;
				capSpeed = 0f;
			}
		}
		
		#region IProjectileController implementation
		
		public virtual void UpdateProjectile (Projectile projectile, float dt) {
			float currentVelocity = velocity;
			if (acceleration != 0) {
				currentVelocity += acceleration * projectile.Time;
				if(acceleration < 0 && currentVelocity < capSpeed) {
					currentVelocity = capSpeed;
				} else if(acceleration > 0 && currentVelocity > capSpeed) {
					currentVelocity = capSpeed;
				}
			}

			if (currentVelocity != 0) {
				Vector2 direction = projectile.direction;
				float change = currentVelocity * dt;
				projectile.Position.x += direction.x * change;
				projectile.Position.y += direction.y * change;
			}
		}
		
		#endregion
	}
}

