using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public GameObject robot;
    public Transform bubble;
    public TextMeshProUGUI bubbleText;
    public Transform buttonParent;
    public GameObject buttonPrefab;
    public Button exitButton;

    private AudioSource _robotAudioSource;
    private readonly Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    private readonly float _bubbleTimer = 1.5f;
    
    private readonly List<string> _names = new List<string>()
    {
        "Ariadna", "Arlet", "Arnau", "Aya", "Bella", "Dana", "Daniel", "Iune", "Jatai", "Leo", "Luca", "Martina", "Oliver", "Ona", "Sara", "Sofía", "Thiago", "Tian", "Vega", "Vincenzo"
    };

    private void Awake()
    {
        _audioClips.Add("jasocaqui", Resources.Load<AudioClip>("Sounds/robot-jasocaqui"));
        _audioClips.Add("holaatots", Resources.Load<AudioClip>("Sounds/robot-holaatots"));
        _audioClips.Add("comesteu", Resources.Load<AudioClip>("Sounds/robot-comesteu"));
        _audioClips.Add("moltbe", Resources.Load<AudioClip>("Sounds/robot-moltbe"));
        _audioClips.Add("comusdieu", Resources.Load<AudioClip>("Sounds/robot-comusdieu"));
        _audioClips.Add("adeuamics", Resources.Load<AudioClip>("Sounds/robot-adeuamics"));
        
        _audioClips.Add("Ariadna", Resources.Load<AudioClip>("Sounds/robot-child-ariadna"));
        _audioClips.Add("Arlet", Resources.Load<AudioClip>("Sounds/robot-child-arlet"));
        _audioClips.Add("Arnau", Resources.Load<AudioClip>("Sounds/robot-child-arnau"));
        _audioClips.Add("Aya", Resources.Load<AudioClip>("Sounds/robot-child-aya"));
        _audioClips.Add("Bella", Resources.Load<AudioClip>("Sounds/robot-child-bela"));
        _audioClips.Add("Dana", Resources.Load<AudioClip>("Sounds/robot-child-dana"));
        _audioClips.Add("Daniel", Resources.Load<AudioClip>("Sounds/robot-child-daniel"));
        _audioClips.Add("Iune", Resources.Load<AudioClip>("Sounds/robot-child-iune"));
        _audioClips.Add("Jatai", Resources.Load<AudioClip>("Sounds/robot-child-jatai"));
        _audioClips.Add("Leo", Resources.Load<AudioClip>("Sounds/robot-child-leo"));
        _audioClips.Add("Luca", Resources.Load<AudioClip>("Sounds/robot-child-luca"));
        _audioClips.Add("Martina", Resources.Load<AudioClip>("Sounds/robot-child-martina"));
        _audioClips.Add("Oliver", Resources.Load<AudioClip>("Sounds/robot-child-oliver"));
        _audioClips.Add("Ona", Resources.Load<AudioClip>("Sounds/robot-child-ona"));
        _audioClips.Add("Sara", Resources.Load<AudioClip>("Sounds/robot-child-sara"));
        _audioClips.Add("Sofía", Resources.Load<AudioClip>("Sounds/robot-child-sofia"));
        _audioClips.Add("Thiago", Resources.Load<AudioClip>("Sounds/robot-child-thiago"));
        _audioClips.Add("Tian", Resources.Load<AudioClip>("Sounds/robot-child-tian"));
        _audioClips.Add("Vega", Resources.Load<AudioClip>("Sounds/robot-child-vega"));
        _audioClips.Add("Vincenzo", Resources.Load<AudioClip>("Sounds/robot-child-vincenzo"));
    }

    private void Start()
    {
        bubbleText.text = String.Empty;
        bubble.gameObject.SetActive(false);
        foreach (Transform child in buttonParent.transform)
        {
            Destroy(child.gameObject);
        }
        exitButton.gameObject.SetActive(false);
        
        _robotAudioSource = robot.GetComponent<AudioSource>();

        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        for (var holaCounts = 0; holaCounts < 3; holaCounts++)
        {
            bubbleText.text = "Acampamcha!!!";
            bubble.gameObject.SetActive(true);
            yield return new WaitForSeconds(_bubbleTimer);
            bubble.gameObject.SetActive(false);
            if (holaCounts == 2)
                yield return new WaitForSeconds(.5f);
            else
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        StartCoroutine(RobotPopIn(.2f));
        yield return new WaitForSeconds(.2f);
        RobotSpeak("jasocaqui");
        bubbleText.text = "Ja soc aqui!!!";
        bubble.gameObject.SetActive(true);
        yield return new WaitForSeconds(_bubbleTimer);
        RobotSpeak("holaatots");
        bubbleText.text = "Hola a tots!";
        bubble.gameObject.SetActive(true);
        yield return new WaitForSeconds(_bubbleTimer);
        RobotSpeak("comesteu");
        bubbleText.text = "Com esteu?";
        bubble.gameObject.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        RobotSpeak("moltbe");
        bubbleText.text = "Molt be!";
        bubble.gameObject.SetActive(true);
        yield return new WaitForSeconds(_bubbleTimer);
        RobotSpeak("comusdieu");
        bubbleText.text = "I com us dieu?";
        bubble.gameObject.SetActive(true);
        yield return new WaitForSeconds(_bubbleTimer);
        foreach (var childName in _names)
        {
            var nameButton = Instantiate(buttonPrefab, buttonParent, true);
            nameButton.GetComponent<RectTransform>().transform.localScale = Vector3.one;
            nameButton.GetComponentInChildren<TextMeshProUGUI>().text = childName;
            nameButton.GetComponent<Button>().onClick.AddListener(() => RobotSpeak(childName));
        }
        exitButton.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(SayGoodbye()));
        exitButton.gameObject.SetActive(true);
    }

    private void RobotSpeak(string soundName)
    {
        _robotAudioSource.clip = _audioClips.TryGetValue(soundName, out AudioClip clip) ? clip : null;
        _robotAudioSource.Play();
    }

    private IEnumerator SayGoodbye()
    {
        bubbleText.text = String.Empty;
        bubble.gameObject.SetActive(false);
        foreach (Transform child in buttonParent.transform)
        {
            Destroy(child.gameObject);
        }
        exitButton.gameObject.SetActive(false);
        RobotSpeak("adeuamics");
        bubbleText.text = "Adeu amics!";
        bubble.gameObject.SetActive(true);
        yield return new WaitForSeconds(_bubbleTimer);
        StartCoroutine(RobotPopOut(.5f));
        bubble.gameObject.SetActive(false);
        yield return new WaitForSeconds(.5f);
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

    }

    private IEnumerator RobotPopIn(float duration)
    {
        var elapsedTime = 0f;
        
        var origin = robot.transform.position;
        var destination = new Vector3(origin.x, -1.5f, origin.z);
        
        while (elapsedTime < duration)
        {
            robot.transform.position = Vector3.Lerp(origin, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator RobotPopOut(float duration)
    {
        var elapsedTime = 0f;
        
        var origin = robot.transform.position;
        var destination = new Vector3(origin.x, -8f, origin.z);
        
        while (elapsedTime < duration)
        {
            robot.transform.position = Vector3.Lerp(origin, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
