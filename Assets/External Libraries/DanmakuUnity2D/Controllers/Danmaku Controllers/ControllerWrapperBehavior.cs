using UnityEngine;
using System.Collections;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D.ProjectileControllers {

	/// <summary>
	/// An abstract generic superclass to mirror the functionality of any implementor of IPorjectileGroupController in a ProjectileControlBehavior.
	/// It can be used with other ProjectileControlBehavior implementations; however it is best used with IProjectileGroupController implementations that don't derive from ProjectileControlBehavior.
	/// </summary>
	public abstract class ControllerWrapperBehavior<T> : ProjectileControlBehavior where T : IDanmakuController {

		private T projectileController;
	
		/// <summary>
		/// Gets the underlying IProjectileGroupController.
		/// </summary>
		/// <value>The underlying controller.</value>
		public T Controller {
			get {
				return projectileController;
			}
		}

		public override void Awake () {
			ProjectileGroup = new DanmakuGroup ();
			if (projectileController == null) {
				projectileController = CreateController ();
				if (projectileController == null) {
					throw new System.NotImplementedException(GetType().ToString() + " does not implement CreateController() properly");
				}
				ProjectileGroup.AddController(projectileController);
			}
		}

		/// <summary>
		/// Creates the underlying controller.
		/// This function is only called if the serialized controller is null or if the type of the controller is not serialziable.
		/// </summary>
		/// <returns>The underlying controller.</returns>
		protected abstract T CreateController ();

		#region implemented abstract members of ProjectileControlBehavior
		public sealed override void UpdateProjectile (Danmaku projectile, float dt) {
			projectileController.UpdateProjectile(projectile, dt);
		}
		#endregion
		
	}
}