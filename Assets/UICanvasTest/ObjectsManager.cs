using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ObjectsManager : MonoBehaviour
{
    public Transform CharacterParent;
    public GameObject CharacterPrefab;

    public Transform OverlayTagCanvas;
    public GameObject OverlayPrefab;
    private List<RectTransform> _overlayTags;

    public Transform WorldTagCanvas;
    public GameObject WorldTagPrefab;
    private List<Transform> _worldTags;

    public Transform SpriteTagParent;
    public GameObject SpriteTagPrefab;
    private List<Transform> _spriteTags;

    public TMP_InputField InputFieldCharacterSize;
    public TMP_Text CurFPSText;
    public TMP_Text AvgFPSText;

    private bool _useWorldTag;
    private bool _useOverlayTag;
    private bool _useSpriteTag;
    private bool _lookAtCamera;

    private float _timer;
    private int _timeCount;
    private float _fpsSum;
    private float _msSum;
    private Transform mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main.transform;
    }
    public void SetCharacter()
    {
        int size = 0;
        try
        {
            size = int.Parse(InputFieldCharacterSize.text);
        }
        catch
        {
            return;
        }

        var childCount = CharacterParent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(CharacterParent.GetChild(i).gameObject);
        }

        int count = 0;
        while (count < size)
        {
            Instantiate(CharacterPrefab, CharacterParent);
            count++;
        }

        EmptyOutAll();
        _useWorldTag = false;
        _useOverlayTag = false;
        _useSpriteTag = false;
    }
    private void EmptyOutAll()
    {
        EmptyOut(ref _worldTags);
        EmptyOut(ref _overlayTags);
        EmptyOut(ref _spriteTags);
    }
    public void Set10000Character()
    {
        InputFieldCharacterSize.text = "10000";
        SetCharacter();
    }
    public void Set5000Character()
    {
        InputFieldCharacterSize.text = "5000";
        SetCharacter();
    }
    public void SetWorldTag(bool lookAtCamera)
    {
        EmptyOutAll();

        _worldTags = new List<Transform>();
        for (int i = 0; i < CharacterParent.childCount; i++)
        {
            var child = CharacterParent.GetChild(i);
            var tag = Instantiate(WorldTagPrefab, WorldTagCanvas);
            tag.transform.position = child.position;
            _worldTags.Add(tag.GetComponent<Transform>());
        }
        _useWorldTag = true;
        _useOverlayTag = false;
        _useSpriteTag = false;
        _lookAtCamera = lookAtCamera;
    }
    public void SetOverlayTag()
    {
        EmptyOutAll();

        _overlayTags = new List<RectTransform>();
        for (int i = 0; i < CharacterParent.childCount; i++)
        {
            var child = CharacterParent.GetChild(i);
            var tag = Instantiate(OverlayPrefab, OverlayTagCanvas);
            var tagRtf = tag.GetComponent<RectTransform>();
            var screenValue = Camera.main.WorldToScreenPoint(child.position);
            tagRtf.position = screenValue;
            _overlayTags.Add(tagRtf);
        }
        _useWorldTag = false;
        _useOverlayTag = true;
        _useSpriteTag = false;
        _lookAtCamera = false;
    }
    public void SetSpriteTag(bool lookAtCamera)
    {
        EmptyOutAll();

        _spriteTags = new List<Transform>();
        for (int i = 0; i < CharacterParent.childCount; i++)
        {
            var child = CharacterParent.GetChild(i);
            var tag = Instantiate(SpriteTagPrefab, SpriteTagParent);
            tag.transform.position = child.position;
            _spriteTags.Add(tag.GetComponent<Transform>());
        }
        _useWorldTag = false;
        _useOverlayTag = false;
        _useSpriteTag = true;
        _lookAtCamera = lookAtCamera;
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene("UICanvasTestScene");
    }

    private void EmptyOut<T>(ref List<T> target) where T : Component
    {
        if (target != null)
        {
            while (0 < target.Count)
            {
                Destroy(target[0].gameObject);
                target.RemoveAt(0);
            }
            target.Clear();
        }
    }

    private Vector3 _height = new Vector3(0, 0.1f, 0);
    private void Update()
    {
        DisplayFPS();

        if (_useWorldTag && _lookAtCamera)
        {
            for (int i = 0; i < CharacterParent.childCount; i++)
            {
                var target = CharacterParent.GetChild(i);
                var position = target.position + _height;
                _worldTags[i].SetPositionAndRotation(position, Quaternion.LookRotation(position - mainCamera.position));
            }
        }
        if (_useWorldTag && !_lookAtCamera)
        {
            for (int i = 0; i < CharacterParent.childCount; i++)
            {
                var target = CharacterParent.GetChild(i);
                _worldTags[i].position = target.position + _height;
            }
        }
        if (_useOverlayTag)
        {
            for (int i = 0; i < CharacterParent.childCount; i++)
            {
                var target = CharacterParent.GetChild(i);
                var screenValue = Camera.main.WorldToScreenPoint(target.position + _height);
                _overlayTags[i].position = screenValue;
            }
        }
        if (_useSpriteTag && _lookAtCamera)
        {
            for (int i = 0; i < CharacterParent.childCount; i++)
            {
                var target = CharacterParent.GetChild(i);
                var position = target.position + _height;
                _spriteTags[i].SetPositionAndRotation(position, Quaternion.LookRotation(position - mainCamera.position));
            }
        }
        if (_useSpriteTag && !_lookAtCamera)
        {
            for (int i = 0; i < CharacterParent.childCount; i++)
            {
                var target = CharacterParent.GetChild(i);
                _spriteTags[i].position = target.position + _height;
            }
        }
    }

    private void DisplayFPS()
    {
        var curFps = 1.0f / Time.deltaTime;
        var textFps = $"{curFps:N1}".PadRight(5);
        var curMs = Time.deltaTime * 1000.0f;
        var textMs = $"{curMs:N1}".PadRight(5);
        CurFPSText.text = $"CurFPS: {textFps} ({textMs}ms)";
        if (_timer <= 0)
        {
            if (_timeCount != 0)
            {
                var avgFps = $"{(_fpsSum / (float)_timeCount):N1}".PadRight(5);
                var avgMs = $"{(_msSum / (float)_timeCount):N1}".PadRight(5);
                AvgFPSText.text = $"AvgFPS: {avgFps} ({avgMs}ms)";
            }
            _fpsSum = 0;
            _msSum = 0;
            _timeCount = 0;
            _timer = 2;
        }
        else
        {
            _fpsSum += curFps;
            _msSum += curMs;
            _timeCount++;
            _timer -= Time.deltaTime;
        }
    }
}
