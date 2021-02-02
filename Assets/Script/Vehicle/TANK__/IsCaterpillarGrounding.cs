using UnityEngine;

public class IsCaterpillarGrounding : MonoBehaviour
{ 
    public bool IsGround;
    public Vector3 Positioning;
    public float DetectionDistance, DetectionInterval;
    public uint CheckCnt;

    void Update()
    {
        IsGround = false;
        for (int y = 0; y < CheckCnt; y++)
        {
            Ray ray = new Ray();
            RaycastHit hit; 
            var startPos = transform.position + transform.InverseTransformVector(Positioning) + transform.forward * DetectionInterval * y / (CheckCnt);
            if (Physics.Raycast(startPos, -transform.up, out hit, DetectionDistance))
                if (transform.root != hit.transform.root)
                {
                    IsGround = true;
                    return;
                } 
        }  
    }
     
    private void OnDrawGizmos()
    {
        Update();
        Gizmos.color = Color.blue;
        for (int y = 0; y < CheckCnt; y++)
        { 
            var startPos = transform.position + transform.InverseTransformVector(Positioning) + transform.forward * DetectionInterval * y / (CheckCnt);
            var endPos = startPos - transform.up * (DetectionDistance - 0.1f); 
            Gizmos.DrawWireSphere(startPos, 0.1f); 
            Gizmos.DrawWireSphere(endPos, 0.1f); 
            Gizmos.DrawLine ( startPos, endPos );
        }
    }

}
