using UnityEngine;

public enum CameraCtrlMode
{
    User,
    Resetting,
    ProgramCtrl
}

public class CameraMove : MonoBehaviour
{
    public CameraCtrlMode Mode
    {
        get => mode;
        set
        {
            mode = value;
            switch (mode)
            {
                case CameraCtrlMode.User:
                    break;

                case CameraCtrlMode.Resetting:
                    resetT = 0;
                    RTY = transform.rotation.ToEuler();
                    break;

                case CameraCtrlMode.ProgramCtrl:

                    break;
            }
        }
           
            
    }

    CameraCtrlMode mode;

    public Transform target;
    public float sensivirity;      // R
    public float distance;         // R
    public bool IsReset;           
    public float upArea, downArea; // R
    public float ResetSpeed;    
    public float CenterCorrection; // R

    float resetT;
    Transform cameras;
    float yd;
    Vector2 pp;
    Vector3 RTY;

    void Start()
    {
        cameras = transform.Find("Main Camera");
    }


    public void RESET_()
    {
        IsReset = true;
        resetT = 0;
        RTY = transform.rotation.ToEuler();
    }
    float cc;
    [SerializeField, Range(0, 120)]
    float CamYAngleLimit;
    [SerializeField, Range(0, 1)]
    float resetPower;
    public float ffL = 0;

    public Vector2 Power, KEY;

    void Update()
    {
        if (target == null) return;
        var vv = Time.deltaTime * sensivirity * Key.JoyStickR.Get * sensivirity;
        transform.position = target.position;
      //  transform.rotation = target.rotation;
        KEY = Key.JoyStickR.GetRAW;

        // if (Mathf.Abs(Key.JoyStickR.GetRAW.x)  < 0.001f )
        // {
        //  transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, resetPower);
        //}
        //  else
        cc += vv.x * Power.x;
        if (cc > CamYAngleLimit) cc = CamYAngleLimit;
        if (cc < -CamYAngleLimit) cc = -CamYAngleLimit; 
        yd += vv.y * Power.y;
        yd = yd < downArea ? downArea : (yd > upArea ? upArea : yd);
        //  float vw = (cc < 0 ? 360 - cc : cc) / 360 + aa;
        float cw = cc / CamYAngleLimit;
        cameras.position = transform.position + Vector3.up * (yd + CenterCorrection) - Vector3.forward * distance + Vector3.right * distance * cc / CamYAngleLimit;//
        // cameras.position = transform.position + Vector3.up * (yd + CenterCorrection) + new Vector3((Mathf.Cos(vw) * distance), 0, Mathf.Sin(vw) * distance);
        cameras.LookAt(target.position + Vector3.up * CenterCorrection);
    }
    public float aa;
}
