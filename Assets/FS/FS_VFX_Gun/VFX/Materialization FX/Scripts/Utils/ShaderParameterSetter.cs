using UnityEngine;

namespace Assets.MaterializationFX.Scripts.Utils
{
	internal sealed class ShaderParameterSetter : MonoBehaviour
	{
		public string ShaderName = "QFX/MFX/Materialization_World";
		public string ParameterName = "_WorldPos";
		public Vector3 ParameterOffset;

		public GameObject TargetObjects;
		public bool ChangeChilds; 
		private Renderer[] _rends;

		private void Start()
		{
			_rends = !ChangeChilds ? new[] { TargetObjects.GetComponent<Renderer>() } : TargetObjects.GetComponentsInChildren<Renderer>();
			foreach (var rend in _rends)
				rend.material.shader = Shader.Find(ShaderName);
		}

		private void Update()
		{
			foreach (var rend in _rends)
				rend.material.SetVector(ParameterName, transform.position + ParameterOffset);
		}
	}
}