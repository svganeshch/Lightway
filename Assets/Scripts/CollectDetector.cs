using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface ICollectable
{
    void OnCollect(Transform transform);
}

interface IRock
{
    bool CheckRock(GameObject gameObject);
    bool GetSprintState();
    bool GetJumpState();
    bool GetFinalRock();
    int GetMaxSoulCount();
}

public class CollectDetector : MonoBehaviour
{
    public LayerMask collectableMask;
    public GameObject collectHud;
    public GameObject JumpHud;
    public GameObject SprintHud;
    public GameObject ScoreHud;
    public float collectDistance = 10f;
    public AudioClip powerAudio;
    public AudioClip[] rockAudio;

    Animator animator;
    AudioSource audioSource;
    Camera mainCam;
    MenuController menuController;
    PlayerInput inputPlayer;
    Ray collectRay;
    RaycastHit collectHit;
    SoulsCounter soulsCounter;

    InputAction collectAction;
    InputAction sprintAction;
    InputAction jumpAction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        inputPlayer = GetComponent<PlayerInput>();
        menuController = FindObjectOfType<MenuController>();
        soulsCounter = FindObjectOfType<SoulsCounter>();
    }

    private void Start()
    {
        mainCam = Camera.main;

        collectHud.SetActive(false);
        ScoreHud.SetActive(false);

        collectAction = inputPlayer.actions["Collect"];
        sprintAction = inputPlayer.actions["Sprint"];
        jumpAction = inputPlayer.actions["Jump"];
    }

    private void Update()
    {
        collectRay.origin = mainCam.transform.position;
        collectRay.direction = mainCam.transform.forward;

        if (Physics.Raycast(collectRay, out collectHit, collectDistance, collectableMask))
        {
            var obj = collectHit.collider.gameObject;

            collectHud.SetActive(true);

            if (obj != null)
            {
                if (obj.TryGetComponent(out ICollectable collectObj))
                {
                    if (collectAction.WasPerformedThisFrame())
                    {
                        collectObj.OnCollect(transform);
                        soulsCounter.SetScore();
                        animator.Play("Power");
                        audioSource.PlayOneShot(powerAudio);
                    }
                }
                else if (obj.TryGetComponent(out IRock rockObj))
                {
                    if (collectAction.WasPerformedThisFrame())
                    {
                        if (rockObj.CheckRock(gameObject))
                        {
                            animator.Play("Praying");
                            audioSource.PlayOneShot(rockAudio[0]);

                            if (rockObj.GetSprintState())
                            {
                                sprintAction.Enable();
                                StartCoroutine(ShowHudWait(SprintHud));
                            }

                            if (rockObj.GetJumpState())
                            {
                                jumpAction.Enable();
                                StartCoroutine(ShowHudWait(JumpHud));
                            }

                            if (rockObj.GetFinalRock())
                            {
                                menuController.LoadGameOverScene();
                            }

                            soulsCounter.ResetScore();
                        }
                        else
                        {
                            audioSource.PlayOneShot(rockAudio[1]);
                            soulsCounter.PrepareHud(rockObj.GetMaxSoulCount());
                            StartCoroutine(ShowHudWait(ScoreHud));
                        }
                    }
                }
            }
            //Debug.Log(collectHit.collider.gameObject.name);
        }
        else
        {
            collectHud.SetActive(false);
        }
    }

    public IEnumerator ShowHudWait(GameObject hud)
    {
        hud.SetActive(true);
        yield return new WaitForSeconds(5);
        hud.SetActive(false);
    }
}
