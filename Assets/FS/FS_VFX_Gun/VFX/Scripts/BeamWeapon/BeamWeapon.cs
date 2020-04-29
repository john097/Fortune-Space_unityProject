using UnityEngine;

namespace Assets.Scripts.BeamWeapon
{
    internal sealed class BeamWeapon : MonoBehaviour
    {
        public GameObject Beam;
        public GameObject ImpactSparks;
        public GameObject ImpactFlash;
        public AnimationCurve WidthOverTime = AnimationCurve.Linear(0, 1, 1, 1);

        public float MaxDistance = 1000;
        public LayerMask LayerMask = ~0;

        private LineRenderer _lineRenderer;
        private GameObject _laserBeam;
        private GameObject _impactFlash;
        private GameObject _impactSparks;
        private float _colorAnimationStartTime;

        private void Start()
        {
            InstantiateBeam();
        }

        private void InstantiateBeam()
        {
            if (Beam == null)
                return;
            _laserBeam = Instantiate(Beam, transform.position, transform.rotation);
            _laserBeam.transform.parent = transform;
            _lineRenderer = _laserBeam.GetComponent<LineRenderer>();
            _colorAnimationStartTime = Time.time;
        }

        private void InstantiateImpactEffect(Vector3 hitPoint, Vector3 hitNormal)
        {
            if (ImpactSparks == null)
                return;
            _impactSparks = Instantiate(ImpactSparks, hitPoint, new Quaternion());
            _impactSparks.transform.LookAt(hitPoint + hitNormal);
            _impactSparks.transform.parent = transform;
        }

        private void InstantiateImpactFlash(Vector3 hitPoint)
        {
            if (ImpactFlash == null)
                return;
            _impactFlash = Instantiate(ImpactFlash, hitPoint, new Quaternion());
            _impactFlash.transform.parent = transform;
        }

        private void Update()
        {
            if (_laserBeam == null)
                return;

            var time = Time.time - _colorAnimationStartTime;

            SetWidthOverTime(time);

            _laserBeam.transform.rotation = transform.rotation;

            RaycastHit hit;

            var ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out hit, MaxDistance, LayerMask))
            {
                var hitPosition = hit.point;
                hitPosition += (transform.position - hit.point).normalized * 0.1f;

                SetLineRendererPosition(transform.position, hitPosition);

                if (_impactSparks != null)
                {
                    _impactSparks.transform.position = hitPosition;
                    _impactFlash.transform.position = hitPosition;
                }
                else
                {
                    InstantiateImpactEffect(hitPosition, hit.normal);
                    InstantiateImpactFlash(hitPosition);
                }
            }
            else
            {
                SetLineRendererPosition(transform.position, transform.position + transform.forward * MaxDistance);

                if (_impactSparks != null)
                {
                    Destroy(_impactSparks.gameObject);
                    Destroy(_impactFlash.gameObject);
                    _impactSparks = null;
                    _impactFlash = null;
                }
            }
        }

        private void SetWidthOverTime(float time)
        {
            var width = WidthOverTime.Evaluate(time);
            _lineRenderer.startWidth = width;
            _lineRenderer.endWidth = width;
        }

        private void SetLineRendererPosition(Vector3 startPosition, Vector3 endPosition)
        {
            _lineRenderer.SetPosition(0, startPosition);
            _lineRenderer.SetPosition(1, endPosition);
        }
    }
}