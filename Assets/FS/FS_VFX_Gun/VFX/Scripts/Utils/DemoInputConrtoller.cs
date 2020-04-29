using UnityEngine;

namespace Assets.Scripts.Utils
{
    internal sealed class DemoInputConrtoller : MonoBehaviour
    {
        private const string SpaceButton = "space";
        private const string LightButton = "f";
        private DemoPrefabController _demoPrefabController;

        public Light Light;

        public void EnableLigh()
        {
            Light.enabled = true;
        }

        public void DisableLight()
        {
            Light.enabled = false;
        }

        private void Start()
        {
            _demoPrefabController = GetComponent<DemoPrefabController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(SpaceButton))
                _demoPrefabController.Next();

            if (Input.GetKeyDown(LightButton))
                Light.enabled = !Light.enabled;
        }
    }
}