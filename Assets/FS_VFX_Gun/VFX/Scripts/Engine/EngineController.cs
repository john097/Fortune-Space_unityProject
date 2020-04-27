using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	internal sealed class EngineController : MonoBehaviour
	{
		private const float SpeedIntencity = 1;

		public ParticleSystem[] EngineParticleSystems;
		public float[] MaxParticleSystemsAlpha;

		private readonly List<Material> _enginePsMaterials = new List<Material>();
		private float _speed;
		private bool _isButtonHold;

		private void Awake()
		{
			foreach (var engineParticleSystem in EngineParticleSystems)
				_enginePsMaterials.Add(engineParticleSystem.GetComponent<Renderer>().material);

			UpdateColorBySpeed();
		}

		private void Update()
		{
			var speed = SpeedIntencity * Time.deltaTime;

			if (Input.GetKeyDown(KeyCode.W))
				_isButtonHold = true;
			else if (Input.GetKeyUp(KeyCode.W))
				_isButtonHold = false;

			if (_isButtonHold)
				_speed += speed;
			else
				_speed -= speed;

			_speed = Mathf.Clamp01(_speed);

			UpdateColorBySpeed();
		}

		private void UpdateColorBySpeed()
		{
			for (int i = 0; i < _enginePsMaterials.Count; i++)
			{
				var tintColor = _enginePsMaterials[i].GetColor("_TintColor");

				tintColor.a = Mathf.Clamp(_speed, 0, MaxParticleSystemsAlpha[i]);

				// ReSharper disable once CompareOfFloatsByEqualityOperator
				// sparks
				if (_enginePsMaterials[i].HasProperty("_ColorEdge"))
				{
					tintColor.r = Mathf.Clamp(_speed, 0, MaxParticleSystemsAlpha[i]);
					tintColor.g = Mathf.Clamp(_speed, 0, MaxParticleSystemsAlpha[i]);
					tintColor.b = Mathf.Clamp(_speed, 0, MaxParticleSystemsAlpha[i]);
				}

				_enginePsMaterials[i].SetColor("_TintColor", tintColor);
			}
		}
	}
}
