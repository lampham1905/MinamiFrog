using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lam
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class PathFollowing : MonoBehaviour
    {
        public float speed = 5f; 
        private List<Vector3> path = new List<Vector3>(); 
        private int targetIndex = 0;
        public List<Transform> listPath = new List<Transform>();
        public float distance = 0;
        public float time;
        public float timeCurrent = 0;
        private void Start()
        {
            Vector3 temp;
            for (int i = 0; i < listPath.Count; i++)
            {
                path.Add(listPath[i].position);
            }

            for (int i = 0; i < listPath.Count - 1; i++)
            {
                temp = listPath[i].position;
                distance += Vector3.Distance(temp, listPath[i + 1].position);
            }
            time = distance / speed;
            StartFollowingPath();
        }

        IEnumerator FollowPath(List<Vector3> path)
        {
            for (int i = 0; i < path.Count; i++)
            { 
                while (transform.position != path[i])
                {
                    transform.position = Vector3.MoveTowards(transform.position, path[i], speed * Time.deltaTime);
                    timeCurrent += Time.deltaTime;
                    yield return null;
                }
            }
        }

     
        public void StartFollowingPath()
        {
            StartCoroutine(FollowPath(path));
        }

      
    }
}
