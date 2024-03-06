using System;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathFromFile(saveFile);
            Debug.Log("Saving to " + path);
            using (FileStream fileStream = File.Open(path, FileMode.Create))
            {
                GameObject player = GameObject.FindWithTag("Player");
                byte[] buffer = SerializeVector(player.transform.position);
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromFile(saveFile);
            Debug.Log("Loading from " + path);
            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                Vector3 position = DeserializeVector(buffer);
                
                GameObject player = GameObject.FindWithTag("Player");
                player.GetComponent<NavMeshAgent>().Warp(position);
            }
        }

        private byte[] SerializeVector(Vector3 vector)
        {
            byte[] bytes = new byte[12];
            BitConverter.GetBytes(vector.x).CopyTo(bytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(bytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(bytes, 8);
            return bytes;
        }

        private Vector3 DeserializeVector(byte[] buffer)
        {
            Vector3 result;
            result.x = BitConverter.ToSingle(buffer, 0);
            result.y = BitConverter.ToSingle(buffer, 4);
            result.z = BitConverter.ToSingle(buffer, 8);
            return result;
        }

        private string GetPathFromFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}