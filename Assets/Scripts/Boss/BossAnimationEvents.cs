using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    /// <summary> 카메라 흔들림을 구현하는 OnShake 함수를 애니메이션으로 제어할 수 있도록 변환한 함수 </summary>
    /// <param name="timeAndAmount"> time과 amount를 쉼표(,)를 이용하여 구분  </param>
    public void OnCameraShake(string timeAndAmount)
    {
        string[] temps = timeAndAmount.Split(',');
        CameraManager.instance.OnShake(float.Parse(temps[0]), float.Parse(temps[1]));
    }

    public void OnEffect(string _effectName)
    {
        GameObject effectClone = BossSystem.Instance.BossEffectManager.GetEffectToName(_effectName);
        effectClone.SetActive(true);
    }

    public void OnEffect(string _effectName, Transform _parent)
    {
        GameObject effectClone = BossSystem.Instance.BossEffectManager.GetEffectToName(_effectName);
        effectClone.SetActive(true);
        effectClone.transform.SetParent(_parent);
    }

    public void OnEffect(string _effectName, Vector3 _position, Vector3 _rotation)
    {
        GameObject effectClone = BossSystem.Instance.BossEffectManager.GetEffectToName(_effectName);
        effectClone.SetActive(true);
        effectClone.transform.position = _position;
        effectClone.transform.eulerAngles = _rotation;
    }

    public void OnEffect(string _effectName, Vector3 _position, Vector3 _rotation, Transform _parent)
    {
        GameObject effectClone = BossSystem.Instance.BossEffectManager.GetEffectToName(_effectName);
        effectClone.SetActive(true);
        effectClone.transform.position = _position;
        effectClone.transform.eulerAngles = _rotation;
        effectClone.transform.SetParent(_parent);
    }

    public void OnEffect(string _effectName, Vector3 _position, Quaternion _rotation)
    {
        GameObject effectClone = BossSystem.Instance.BossEffectManager.GetEffectToName(_effectName);
        effectClone.SetActive(true);
        effectClone.transform.position = _position;
        effectClone.transform.rotation = _rotation;
    }
    

    public void OnEffect(string _effectName, Vector3 _position, Quaternion _rotation, Transform _parent)
    {
        GameObject effectClone = BossSystem.Instance.BossEffectManager.GetEffectToName(_effectName);
        effectClone.SetActive(true);
        effectClone.transform.position = _position;
        effectClone.transform.rotation = _rotation;
        effectClone.transform.SetParent(_parent);
    }
}
