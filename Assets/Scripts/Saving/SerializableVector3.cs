using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    class SerializableVector3
    {
        private float x, y, z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}