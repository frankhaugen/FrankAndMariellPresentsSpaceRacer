using UnityEngine;
using UnityEngine.InputSystem;

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
        
        
         //************************************************************
 // Use PhysX rigidbody to update forces acting on a plane
 //************************************************************
 // public void UpdateForces()
 // {
 //     var elapsed = Time.deltaTime;
 //
 //     //************************************************************
 //     // Update Roll & Yaw
 //     //************************************************************
 //     float ax = 0;
 //     float ay = 0;
 //     
 //     //************************************************************
 //     // Accelerometer controls, cater to specific situations
 //     //************************************************************
 //     int torqueX;
 //     if ( InputSystem.GetDevice<Joystick>().enabled )
 //     {
 //             var torqueY = 0.1f * FlightStick.NormalizedX * FlightManager.Data.Handling;
 //             var torqueX = 400 * FlightStick.NormalizedY * (1-FlightManager.Data.Stability);
 //             var torqueX = 400 * FlightStick.NormalizedY * (1-FlightManager.Data.Stability);
 //         
 //         EngineThrust += 25 * ThrustStick.NormalizedY;
 //         if( EngineThrust <= 0 )
 //         {
 //             EngineThrust = 0;
 //         }
 //         else if ( EngineThrust >= MaxThrust )
 //         {
 //             EngineThrust = MaxThrust;    
 //         }        
 //     }
 //     //************************************************************
 //     //4 Major forces on Plane: Thrust, Lift, Weight, Drag
 //     //************************************************************
 //     Vector3 airSpeedVector = -rigidbody.velocity;
 //     
 //     float angleOfAttack = -Mathf.Deg2Rad * Vector3.Dot( rigidbody.velocity, transform.up);
 //     float slipAoA = -Mathf.Deg2Rad * Vector3.Dot( rigidbody.velocity, transform.right);
 //     float speedWeight = AirDensity * ((CurrentSpeed*CurrentSpeed)/2);
 //     float weightCo = Weight;    
 //     
 //     sideSlipCoefficient = 0.001f * slipAoA * speedWeight;
 //     liftCoefficient = WingArea * angleOfAttack * speedWeight;
 //     
 //     //************************************************************
 //     //drag goes from x -> 3x * % 
 //     //angle goes from -1 -> 1
 //     //************************************************************
 //     float horizonAngle = Vector3.Dot( transform.forward, Vector3.up);
 //     dragCoefficient = 2*AirDrag + 2 * AirDrag * horizonAngle;
 //     thrustCoefficient = EngineThrust;
 //     AoA = Vector3.Dot( transform.up, Vector3.up);
 //
 //     //************************************************************
 //     // Pitch from lift, yaw & roll from sideslip
 //     //************************************************************
 //     rollCoEfficient = -(torqueY+ay) * RollRate * speedWeight;
 //     pitchCoEfficient =  (torqueX+ax) * ElevatorRate * speedWeight;    
 //     yawCoEfficient = 0;
 //     
 //     pitchCoEfficient += liftCoefficient * 0.001f;
 //     yawCoEfficient += -sideSlipCoefficient * 2f;
 //     rollCoEfficient += -sideSlipCoefficient * 0.1f;
 //     
 //     //************************************************************
 //     // Calc vectors now we have coefficients
 //     //************************************************************
 //     WeightVector = Vector3.down * weightCo;
 //     
 //     ThrustForceVector = transform.forward * thrustCoefficient;        
 //     LiftVector = transform.up * liftCoefficient;
 //     DragVector = airSpeedVector * dragCoefficient;
 //     SideSlip = Vector3.right * sideSlipCoefficient;
 //     
 //     RollTorque = Vector3.forward * rollCoEfficient;
 //     YawTorque = Vector3.up * yawCoEfficient;        
 //     PitchTorque = Vector3.right * pitchCoEfficient;
 //     
 //     //************************************************************
 //     // Apply forces
 //     //************************************************************
 //     rigidbody.AddForce(ThrustForceVector, ForceMode.Force);
 //     rigidbody.AddForce(DragVector, ForceMode.Force);
 //     rigidbody.AddForce(LiftVector, ForceMode.Force);
 //     rigidbody.AddForce(WeightVector, ForceMode.Force);
 //     rigidbody.AddForce(SideSlip, ForceMode.Force);
 //     
 //     rigidbody.AddRelativeTorque(RollTorque);
 //     rigidbody.AddRelativeTorque(YawTorque);
 //     rigidbody.AddRelativeTorque(PitchTorque);
 //     
 //     //************************************************************
 //     // Instrumentation data & debug output
 //     //************************************************************
 //     VelocityVector = rigidbody.velocity;
 //     
 //     PlaneLift = liftCoefficient;
 //     //AoA = angleOfAttack;
 //     RotX = transform.rotation.eulerAngles.x;
 //     RotY = transform.rotation.eulerAngles.y;
 //     RotZ = transform.rotation.eulerAngles.z;
 //     torqueX = 0;
 //     torqueY = 0;
 //     CurrentSpeed = VelocityVector.magnitude;
 //     CurrentFuel -= Time.deltaTime * Weight/5 * EngineThrust/MaxThrust * 0.3f * FuelConsumption;
 // }
    }
}