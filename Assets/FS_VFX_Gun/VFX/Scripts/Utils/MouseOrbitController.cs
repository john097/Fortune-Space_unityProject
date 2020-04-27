using UnityEngine;

namespace Assets.Scripts.Utils
{
	internal sealed class MouseOrbitController : MonoBehaviour
	{
		public Transform Target;
		public float Distance = 5.0f;
		public float XSpeed = 120.0f;
		public float YSpeed = 120.0f;

		public float YMinLimit = 20f;
		public float YMaxLimit = 80f;

		public float DistanceMin = .5f;
		public float DistanceMax = 15f;

		private float _x;
		private float _y;

		private void Start()
		{
			var angles = transform.eulerAngles;
			_x = angles.y;
			_y = angles.x;
		}

		private void LateUpdate()
		{
			if (!Input.GetMouseButton(0))
				return;

			_x += Input.GetAxis("Mouse X") * XSpeed * Distance * 0.02f;
			_y -= Input.GetAxis("Mouse Y") * YSpeed * 0.02f;

			_y = ClampAngle(_y, YMinLimit, YMaxLimit);

			var rotation = Quaternion.Euler(_y, _x, 0);

			Distance -= Input.GetAxis("Mouse ScrollWheel") * 5;

			var negDistance = new Vector3(0.0f, 0.0f, -Distance);
			var position = rotation * negDistance + Target.position;

			transform.rotation = rotation;
			transform.position = position;
		}

		private static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360F)
				angle += 360F;
			if (angle > 360F)
				angle -= 360F;
			return Mathf.Clamp(angle, min, max);
		}
	}
}
