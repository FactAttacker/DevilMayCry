using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class CinematicEndingManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundMg
    {
        public AudioSource _audio;
        public enum Type
        {
            Walking,
            BreakBuiling,
            Boos_2
        }
        [System.Serializable]
        public class SoundItem
        {
            public Type type;
            public AudioClip clip;
        }
        public SoundItem[] soundItems;
    }
    [SerializeField]
    [Header("----- Sound ----")]
    public SoundMg soundMg;

    [Header("----- Camera ----")]
    [SerializeField]
    Camera[] cameras;
    [SerializeField]
    GameObject defaultPostProcessing;
    [SerializeField]
    GameObject[] cameraPos;

    [Header("----- Player ----")]
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject playerCimeticObj;
    [SerializeField]
    Transform playerPosTr;
    Animator playerAnim;
    PostProcessVolume postProVolume;
    Player playerMg;

    [Header("----- Boss ----")]
    [SerializeField]
    GameObject boss;
    Animator bossAnim;
    [SerializeField]
    GameObject fog;

    [SerializeField]
    GameObject[] glass;

    public PostProcessVolume FadeOutProfile;
    float aniSpeed = 0f;

    void Start()
    {
        FadeOutProfile.weight = 1;
        player.TryGetComponent(out playerMg);
        player.TryGetComponent(out playerAnim);
        StartCoroutine(CoScenario());
    }

    IEnumerator CoScenario()
    {
        AudioSource audioSource = VoiceSoundManager.instatnce.SetBGMChange(VoiceSoundManager.BGMType.ENDING);
        if (FadeInOutController.instance != null)
        {
            yield return new WaitUntil(() => !FadeInOutController.instance.isFade);
            yield return new WaitUntil(() => !FadeInOutController.instance.fadeImg.gameObject.activeSelf);
        }

        //칼 꺼낸상태
        playerMg.katana.transform.localEulerAngles = new Vector3(66f, -230f, -60f);
        playerMg.katana.transform.SetParent(playerMg.rightHand.transform, false);
        playerMg.katana.transform.localPosition = new Vector3(0.12f, -0.136f, 0.5f);
        playerMg.outPutSword = !playerMg.outPutSword;

        //시간 느리게
        Time.timeScale = 0.1f;
        playerAnim.SetTrigger("thirdAttack");

        //페이드 아웃
        //while (true)
        //{
        //    FadeOutProfile.weight -= Time.deltaTime * 5;
        //    if (FadeOutProfile.weight <= 0) break;
        //    yield return null;
        //}
        FadeOutProfile.gameObject.SetActive(false);
       
        //aniSpeed = playerAnim.speed;
        //playerAnim.speed = 0f;

        boss.TryGetComponent(out bossAnim);

        // 칼 베기 애니메이션 
        if (postProVolume == null) cameras[0].TryGetComponent(out postProVolume);

        //보이스
        VoiceSoundManager.instatnce.OnDanteVoice("Dante-Haaaaaaaa");
        

        float timeSpeed = 2f;
        postProVolume.weight = 0;
        while (postProVolume.weight <= 1f)
        {
            postProVolume.weight += Time.deltaTime * timeSpeed;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);
        defaultPostProcessing.SetActive(true);

        //칼 집어넣기
        //playerAnim.SetTrigger("outPutSword");
        Time.timeScale = 1f;
        //bossCinematic.story[0].isNext = true;
        bossAnim.SetTrigger("Death");
        
        yield return new WaitForSeconds(2f);
        fog.SetActive(true);

        float time = 1;
        float amount = 1;
        Transform camera1 = cameras[1].transform;
        while (time > 0)
        {
            time -= Time.deltaTime;
            cameras[1].transform.position = (Random.insideUnitSphere * amount) + cameraPos[0].transform.position;
            amount -= amount * 0.05f;
            yield return null;
        }
        cameras[1].transform.position = camera1.position;
        yield return new WaitForSeconds(1f);
        fog.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        cameras[1].gameObject.SetActive(false);
        cameras[2].gameObject.SetActive(true);

        //플레이어 -> 
        player.SetActive(false);
        playerCimeticObj.SetActive(true);

        //멈춤
        playerCimeticObj.TryGetComponent(out Animator playerAim);
        playerAim.SetTrigger("Stop");
        playerCimeticObj.TryGetComponent(out Scenemachine_Player playerCinematic);

        //playerCinematic.OnInputSword();
        playerAim.SetTrigger("walkWithinput");
        yield return new WaitForSeconds(0.8f);

        //칼 집어넣기
        yield return null;
        VoiceSoundManager.instatnce.OnDanteVoice("Dante-SweetDreams");
        playerCinematic.katana.transform.SetParent(playerCinematic.swordCase.transform, false);
        playerCinematic.katana.transform.localPosition = new Vector3(0.01705508f, -0.4062368f, -0.143f);
        playerCinematic.katana.transform.localEulerAngles = new Vector3(0f, -180f, 20f);
        yield return new WaitForSeconds(2f);

        //BGM
        audioSource.Play();

        float timeTurn = 0f;
        while (timeTurn < 1.3f)
        {
            timeTurn += Time.deltaTime;
            if(!playerAim.GetBool("Turn")) playerAim.SetTrigger("Turn");
            float lerpAngle = Mathf.LerpAngle(playerCimeticObj.transform.eulerAngles.y, 200, 0.5f * Time.deltaTime);
            //transform.Rotate(0f, angle, 0.0f);
            //transform.eulerAngles = new Vector3(0, rotationy, 0);
            playerCimeticObj.transform.eulerAngles = new Vector3(0, lerpAngle, 0);
            yield return null;
        }

        cameras[2].gameObject.SetActive(false);
        cameras[3].gameObject.SetActive(true);

        playerCimeticObj.transform.position = playerPosTr.position;
        playerCimeticObj.transform.rotation = playerPosTr.rotation;
        playerAim.SetTrigger("NotTurn");
        bool isWalk = true;
        while (isWalk)
        {
            playerAim.SetBool("AloneLeg", true);
            playerCimeticObj.transform.position = Vector3.MoveTowards(playerCimeticObj.transform.position, playerCinematic.destination.position, playerCinematic.walkSpeed * Time.deltaTime);
            if(playerCimeticObj.transform.position.z > playerCinematic.destination.position.z / 2)
            {
                Time.timeScale = 0.2f;
                isWalk = false;
            }
            yield return null;
        }

        //점수 및 로비가기 창 키기
        foreach (GameObject go in glass)
        {
            go.SetActive(true);
            AudioSource audio = go.GetComponentInChildren<AudioSource>();
            audio.time = audio.clip.length / 3f;
            yield return new WaitForSeconds(0.1f);
        }
        Time.timeScale = 0f;
    }

}
