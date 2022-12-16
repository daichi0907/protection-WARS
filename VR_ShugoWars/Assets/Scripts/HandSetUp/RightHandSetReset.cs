using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandSetReset : MonoBehaviour
{
    #region define

    #endregion

    #region serialize field

    #endregion

    #region field
    private OVRSkeleton _OVRSkeleton;
    #endregion

    #region property

    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _OVRSkeleton = GameObject.Find("RightOVRHandPrefab").GetComponent<OVRSkeleton>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "IndexFinger")
        {
            _OVRSkeleton.IsInitialized = false;
        }
    }
    #endregion

    #region public function

    #endregion

    #region private function

    #endregion
}
