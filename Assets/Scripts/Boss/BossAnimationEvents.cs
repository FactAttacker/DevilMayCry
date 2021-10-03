using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    /// <summary> ī�޶� ��鸲�� �����ϴ� OnShake �Լ��� �ִϸ��̼����� ������ �� �ֵ��� ��ȯ�� �Լ� </summary>
    /// <param name="timeAndAmount"> time�� amount�� ��ǥ(,)�� �̿��Ͽ� ����  </param>
    public void OnCameraShake(string timeAndAmount)
    {
        string[] temps = timeAndAmount.Split(',');
        CameraManager.instance.OnShake(float.Parse(temps[0]), float.Parse(temps[1]));
    }
}
