using UnityEngine;

namespace Assets.Scripts.BlasterWeapon
{
	[RequireComponent(typeof(TrailRenderer))]
	internal sealed class BlasterBullet : MonoBehaviour
	{
		[HideInInspector]
		public float Speed = 1;
		[HideInInspector]
		public float LifeTime = 2f;
		[HideInInspector]
		public float LifeTimeAfterCollision = 1f;
		[HideInInspector]
		public bool DestroyOnCollision = true;

		public GameObject ImpactEffect;

		private Transform _transform;
		private bool _isCollisionDetected;

		private void Start()
		{
			_transform = transform;
			Destroy(gameObject, LifeTime);
		}

		private void Update()
		{
			if (_isCollisionDetected)
				return;

			_transform.position += _transform.forward * Speed * Time.deltaTime;

			RaycastHit hit;
			if (!Physics.Raycast(_transform.position, _transform.forward, out hit, Speed * Time.deltaTime * 2))
				return;

			_isCollisionDetected = true;
			_transform.position = hit.point;

			if (DestroyOnCollision)
				Destroy(gameObject, LifeTimeAfterCollision);

			if (ImpactEffect == null)
				return;

			var impactEffect = Instantiate(ImpactEffect, hit.point, new Quaternion());
			impactEffect.transform.LookAt(_transform.position + hit.normal);
			Destroy(impactEffect, LifeTime);
		}
	}
}
