using UnityEngine;
using Oculus.Interaction;

public class CustomGrabbable : OVRGrabbable
{
    public System.Action OnGrabBegin; // つかんだときのイベント
    public System.Action OnGrabEnd;   // 離したときのイベント

    // グラブの開始時に呼び出される
    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        OnGrabBegin?.Invoke(); // イベントをトリガー
    }

    // グラブの終了時に呼び出される
    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        OnGrabEnd?.Invoke(); // イベントをトリガー
    }
}
