using UnityEngine;

namespace Code.Movement.Calculators
{
    public class AircraftLiftCalculator
    {
        private readonly float _wingSpan;
        private readonly float _wingArea;

        private readonly float _aspectRatio;
        private readonly GameObject _gameObject;
        private readonly Rigidbody _rigidbody;

        public AircraftLiftCalculator(GameObject gameObject, float wingSpan = 14f, float wingArea = 80f)
        {
            _gameObject = gameObject;
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _wingSpan = wingSpan;
            _wingArea = wingArea;
            _aspectRatio = (wingSpan * wingSpan) / wingArea;
        }

        public void CalculateForces (float enginePower)
        {
            // *flip sign(s) if necessary*
            var localVelocity = _gameObject.transform.InverseTransformDirection(_rigidbody.velocity);
            var angleOfAttack = Mathf.Atan2(-localVelocity.y, localVelocity.z);

            // α * 2 * PI * (AR / AR + 2)
            var inducedLift = angleOfAttack * (_aspectRatio / (_aspectRatio + 2f)) * 2f * Mathf.PI;

            // CL ^ 2 / (AR * PI)
            var inducedDrag = (inducedLift * inducedLift) / (_aspectRatio * Mathf.PI);

            // V ^ 2 * R * 0.5 * A
            var velocity = _rigidbody.velocity;
            var pressure = velocity.sqrMagnitude * 1.2754f * 0.5f * _wingArea;

            var lift = inducedLift * pressure;
            var drag = (0.021f + inducedDrag) * pressure;

            // *flip sign(s) if necessary*
            var dragDirection = velocity.normalized;
            var liftDirection = Vector3.Cross(dragDirection, _gameObject.transform.right);

            // Lift + Drag = Total Force
            _rigidbody.AddForce(liftDirection * lift - dragDirection * drag);
            _rigidbody.AddForce(_gameObject.transform.forward * enginePower);
        }
    }
}