using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures
{
    public class SliceObject : MonoBehaviour
    {
        [field: SerializeField] public ForceApplier LeftPartForceApplier { get; private set; }
        [field: SerializeField] public ForceApplier RightPartForceApplier { get; private set; }
        [field: SerializeField] public GravitationApplier GravitationApplier { get; private set; }
        [field: SerializeField] public TorqueApplier TorqueApplier { get; private set; }
        [field: SerializeField] public ForceApplier ForceApplier { get; private set; }
        [field: SerializeField] public ParticleSystemController ParticleSystemController { get; private set; }
    
    

        public void Slice(Vector2 sliceVector, float sliceForce)
        {
            ResetForces();
            RotateObject(sliceVector);
            AddSlicePartsForces(sliceVector, sliceForce);
            PlayParticles();
        }

        private void PlayParticles()
        {
            ParticleSystemController.PlayAll();
        }

        private void AddSlicePartsForces(Vector2 sliceVector, float sliceForce)
        {
            Vector2 perpendicularVector = Vector2.Perpendicular(sliceVector) * sliceForce;
            Vector2 up = Vector2.zero;
            Vector2 down = Vector2.zero;


            if (perpendicularVector.y >= 0)
            {
                up = perpendicularVector;
                down = -perpendicularVector;
            }
            else
            {
                up = -perpendicularVector;
                down = perpendicularVector;
            }

            if (LeftPartForceApplier.transform.position.y > RightPartForceApplier.transform.position.y)
            {
                LeftPartForceApplier.AddForce(up);
                RightPartForceApplier.AddForce(down);
            }
            else
            {
                LeftPartForceApplier.AddForce(down);
                RightPartForceApplier.AddForce(up);
            }
        }

        private void ResetForces()
        {
            TorqueApplier.Clear();
            ForceApplier.Clear();
            GravitationApplier.Clear();
        }

        private void RotateObject(Vector2 sliceVector)
        {
            float zAngle = Vector2.Angle(Vector2.up, sliceVector);
            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.z = zAngle;
            transform.localEulerAngles = eulerAngles;
        }
    }
}