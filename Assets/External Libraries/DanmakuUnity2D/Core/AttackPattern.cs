using UnityEngine;
using UnityUtilLib;
using System.Collections;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	/// <summary>
	/// An abstract class that defines the basic functionality of a DanmakuUnity Attack Pattern.
	/// Derived classes of AbstractAttackPattern are used to define and control the various intricate patterns seen in danmaku games.
	/// </summary>
	public abstract class AttackPattern : CachedObject, IPausable {
		#region IPausable implementation
		public bool Paused {
			get;
			set;
		}
		#endregion

		/// <summary>
		/// The DanmakuField that all bullets fired by this pattern will end up within. <br>
		/// This MUST be set to a non-null value before firing any bullets.
		/// <see cref="DanmakuField"/>
		/// </summary>
		/// <value>The AttackPattern's target danmaku field</value>
		public DanmakuField TargetField;

		/// <summary>
		/// Helper method to quickly get the angle needed to directly fire at the player in the AttacKPattern's target field
		/// </summary>
		/// <returns>The angle needed to fire directly toward the player.</returns>
		/// <param name="position">The position to evaluate the angle to the player from.</param>
		/// <param name="coordSys">The cordinate system used to evaluate the true location of the source location</param>
		protected float AngleToPlayer(Vector2 position, DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.World) {
			return TargetField.AngleTowardPlayer(transform.position, coordSys);
		}

		/// <summary>
		/// The Main Loop of the AttackPattern, called once every frame during the AttackPattern's execution
		/// </summary>
		protected abstract void MainLoop();

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Danmaku2D.AttackPattern"/> is active.
		/// Setting this to false on a currently executing AttackPattern will terminate its execution immediately.
		/// </summary>
		/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
		public bool Active {
			get;
			set;
		}
		
		/// <summary>
		/// Gets a value indicating whether this instance is finished.
		/// </summary>
		/// <value><c>true</c> if this instance is finished; otherwise, <c>false</c>.</value>
		protected abstract bool IsFinished { get; }

		/// <summary>
		/// An overridable function that is called every time the AttackPattern starts its execution.
		/// Use this for setup of various execution related variables
		/// </summary>
		protected virtual void OnInitialize() {
		}

		/// <summary>
		/// An overridable function that is called every time the AttackPattern finishes its execution.
		/// Use this for cleanup of various execution related variables
		/// </summary>
		protected virtual void OnFinalize() {
		}

		/// <summary>
		/// Starts the execution of this AttackPattern
		/// </summary>
		public virtual void Fire () {
			if (!Active) {
				StartCoroutine (Execute ());
			} else {
				print("Tried Executing Already Running Attack Pattern");
			}
		}
		
		private IEnumerator Execute() {
			Active = true;
			OnInitialize ();
			while(!IsFinished && Active) {
				MainLoop();
				yield return UtilCoroutines.WaitForUnpause(this);
			}
			OnFinalize ();
			Active = false;
		}

		/// <summary>
		/// Helper method for subclasses to quickly spawn projectiles.
		/// </summary>
		/// <returns>The projectile spawned with the given parameters</returns>
		/// <param name="projectileType">The defining characteristics behind this projectile</param>
		/// <param name="location">The location the bullet is to be spawned at. Expected value varies with the provided CoordinateSystem</param>
		/// <param name="rotation">The rotation the bullet is to be spawned with.</param>
		/// <param name="coordSys">The Coordinate system the location is to be spawned using</param>
		protected Projectile SpawnProjectile(ProjectilePrefab projectileType,
		                                     Vector2 location,
		                                     float rotation,
		                                     DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			return TargetField.SpawnProjectile (projectileType, location, rotation, coordSys);
		}

		/// <summary>
		/// Helper method for subclasses to quickly firing straight moving bullets.
		/// </summary>
		/// <returns>The projectile spawned with the given parameters</returns>
		/// <param name="projectileType">The defining characteristics behind this projectile</param>
		/// <param name="location">The location the bullet is to be fired from. Expected value varies with the provided CoordinateSystem</param>
		/// <param name="rotation">The rotation the bullet is to be fired with.</param>
		/// <param name="coordSys">The Coordinate system the location is to be spawned using</param>
		protected LinearProjectile FireLinearBullet(ProjectilePrefab projectileType, 
		                                      Vector2 location, 
		                                      float rotation, 
		                                      float velocity,
		                                      DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			LinearProjectile linearProjectile = new LinearProjectile (velocity);
			Projectile bullet = Projectile.Get (projectileType, TargetField.WorldPoint(location, coordSys), rotation, TargetField);
			bullet.Activate ();
			bullet.AddController(linearProjectile);
			return linearProjectile;
		}

		/// <summary>
		/// Helper method for subclasses to quickly firing bullet that move along curved paths.
		/// </summary>
		/// <returns>The projectile spawned with the given parameters</returns>
		/// <param name="projectileType">The defining characteristics behind this projectile</param>
		/// <param name="location">The location the bullet is to be fired from. Expected value varies with the provided CoordinateSystem</param>
		/// <param name="rotation">The rotation the bullet is to be fired with.</param>
		/// <param name="coordSys">The Coordinate system the location is to be spawned using</param>
		protected CurvedProjectile FireCurvedBullet(ProjectilePrefab projectileType,
		                                      Vector2 location,
		                                      float rotation,
		                                      float velocity,
		                                      float angularVelocity,
		                                      DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			CurvedProjectile curvedProjectile = new CurvedProjectile (velocity, angularVelocity);
			Projectile bullet = Projectile.Get (projectileType, TargetField.WorldPoint(location, coordSys), rotation, TargetField);
			bullet.Activate ();
			bullet.AddController(curvedProjectile);
			return curvedProjectile;
		}

		/// <summary>
		/// Helper method for subclasses to quickly firing bullets with custom behavior.
		/// </summary>
		/// <returns>The projectile spawned with the given parameters</returns>
		/// <param name="projectileType">The defining characteristics behind this projectile</param>
		/// <param name="location">The location the bullet is to be fired from. Expected value varies with the provided CoordinateSystem</param>
		/// <param name="rotation">The rotation the bullet is to be fired with.</param>
		/// <param name="coordSys">The Coordinate system the location is to be spawned using</param>
		protected void FireControlledBullet(ProjectilePrefab projectileType, 
		                                    Vector2 location, 
		                                    float rotation, 
		                                    IProjectileController controller,
		                                    DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			Projectile bullet = Projectile.Get (projectileType, TargetField.WorldPoint(location, coordSys), rotation, TargetField);
			bullet.Activate ();
			bullet.AddController(controller);
		}
	}
}