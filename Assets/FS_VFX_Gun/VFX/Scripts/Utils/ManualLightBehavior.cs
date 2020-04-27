using UnityEngine;

namespace Assets.Scripts.Utils
{
	[RequireComponent(typeof(Light))]
	internal sealed class ManualLightBehavior : MonoBehaviour
	{
		public AnimationCurve LightCurve;
		public float GraphScaleX, GraphScaleY;

		private bool _isLightAnimationStarted;
		private float _startTime;
		private Light _lightSource;

		public void Play()
		{
			_startTime = Time.time;
			_isLightAnimationStarted = true;
			_lightSource.enabled = true;
		}

		private void Start()
		{
			_lightSource = GetComponent<Light>();
		}

		private void Update()
		{
			if (!_isLightAnimationStarted)
				return;

			var time = Time.time - _startTime;

			if (time > GraphScaleX)
			{
				//_startTime = Time.time;
				_isLightAnimationStarted = false;
			}

			var eval = LightCurve.Evaluate(time / GraphScaleX) / GraphScaleY;

			_lightSource.intensity = eval;
		}
	}
}
