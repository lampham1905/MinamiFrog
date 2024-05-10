using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lam
{
    public enum TypeFrog
    {
        youth, elder, golbin
    }
    public class IFrog : MonoBehaviour
    {
        public TypeFrog typeFrog;
        public FrogMove frogMove;
    }
}
