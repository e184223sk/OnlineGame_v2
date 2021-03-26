#if UNITY_EDITOR
public class AudioVolumeChecker : UnityEngine.MonoBehaviour
{
    void OnDrawGizmos() { if (!UnityEditor.EditorApplication.isPlaying) AudioSystem.UPDATE__(); }
}
#endif
