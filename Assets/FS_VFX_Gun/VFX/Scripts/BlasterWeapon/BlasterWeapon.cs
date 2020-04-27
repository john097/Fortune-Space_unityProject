using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.BlasterWeapon
{
	internal sealed class BlasterWeapon : MonoBehaviour
	{
		public GameObject Bullet;
		public ParticleSystem MuzzleFlashPs;
		public ManualLightBehavior ManualLightBehavior;
		public float BulletSpeed = 1;
		public float LifeTime = 2f;
		public float LifeTimeAfterCollision = 1f;
		public float Duration;
		public bool DestroyOnCollision = true;


		private bool _isEnabled;
		private ParticleSystem[] _muzzleFlashParticleSystems;

		private void Awake()
		{
			MuzzleFlashPs.Stop(withChildren: true);
			_muzzleFlashParticleSystems = MuzzleFlashPs.GetComponentsInChildren<ParticleSystem>();
		}

		private void Start()
		{
			InvokeRepeating("Fire", 1f, Duration);
		}

		private void OnEnable()
		{
			_isEnabled = true;
			EnableParticleSystems(_isEnabled);
		}

		private void OnDisable()
		{
			_isEnabled = false;
			EnableParticleSystems(_isEnabled);
		}

		private void Fire()
		{
			if (!_isEnabled)
				return;

			ManualLightBehavior.Play();

			MuzzleFlashPs.Play(withChildren: true);

			InstantiateBullet(Bullet);
		}

		private void InstantiateBullet(GameObject bullet)
		{
			var bulletGo = Instantiate(bullet, transform.position, transform.rotation);
			var blasterBullet = bulletGo.GetComponent<BlasterBullet>();

			blasterBullet.Speed = BulletSpeed;
			blasterBullet.LifeTime = LifeTime;
			blasterBullet.LifeTimeAfterCollision = LifeTimeAfterCollision;
			blasterBullet.DestroyOnCollision = DestroyOnCollision;

			Destroy(bulletGo, LifeTime);
		}

		private void EnableParticleSystems(bool isEnabled)
		{
			foreach (var particleSystems in _muzzleFlashParticleSystems)
			{
				var particleSystemsEmission = particleSystems.emission;
				particleSystemsEmission.enabled = isEnabled;
			}
		}
	}
}
