using FriendsGamesTools;
using System.Linq;
using UnityEngine;

namespace HcUtils
{

    public class SimpleRagdoll : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody[] rigidbodies;

        [SerializeField] private Transform hips;
        [SerializeField]
        private Collider[] colliders;

        [SerializeField] private Transform baseTransform;
        [SerializeField]
        private Transform[] baseBarts;

        public bool Active { private set; get; } = false;

        public Vector3 HipsPosition => hips.localPosition;
        [ContextMenu("Set")]
        public void Set()
        {
            
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            colliders = GetComponentsInChildren<Collider>();
            
            foreach (var col in colliders)
            {
                col.enabled = false;
            }

            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = true;
            }
            baseBarts = new Transform[rigidbodies.Length];

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                var rb = baseTransform.GetComponentsInChildren<Transform>().Find(x=>x.name== rigidbodies[i].name);
                if (rb) baseBarts[i] = rb.transform;
            }
            PrefabUtils.SetPrefabDirty();
        }

        public void Activate(Vector3 forceToAdd)
        {
            gameObject.SetActive(true);
            Active = true;
            foreach (var col in colliders)
            {
                col.enabled = true;
            }

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].transform.SetPositionAndRotation(baseBarts[i].position, baseBarts[i].rotation);

                rigidbodies[i].isKinematic = false;

                rigidbodies[i].AddForce(forceToAdd, ForceMode.VelocityChange);
            }
        }

    }
}
