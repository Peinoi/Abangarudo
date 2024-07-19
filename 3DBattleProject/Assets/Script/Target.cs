using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private const float WALKING_FIRE = 0.08f, STANDING_FIRE = 0.04f, CROUCHING_FIRE = 0.02f, FINESIGHT_FIRE = 0.001f;
    [SerializeField]
    private GameObject go_TargetHUD;
    private float gunAccuracy=0.06f;
   
        public float GetAccuracy()
        {
            
            return gunAccuracy;
        }
      
    
}
