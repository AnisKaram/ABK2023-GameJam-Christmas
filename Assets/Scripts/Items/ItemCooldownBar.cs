using UnityEngine;

public class ItemCooldownBar : MonoBehaviour
{
    [SerializeField]
    private ItemBase _itemScript;

    [SerializeField]
    private Transform _filledBar, _emptyBarLeftPoint;

    private float _barFillDuration;
    private float _timeElapsed = 0;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _filledBar.position = _emptyBarLeftPoint.position;
        _filledBar.localScale = new Vector3(0, 1, 1);

        _barFillDuration = _itemScript.GetRespawnDelay();
        _timeElapsed = 0;
    }

    private void Update()
    {
        _filledBar.localPosition = new Vector3(Mathf.Lerp(_emptyBarLeftPoint.localPosition.x, 0, _timeElapsed / _barFillDuration), 0, 0);
        _filledBar.localScale = new Vector3(Mathf.Lerp(0, 1, _timeElapsed / _barFillDuration), 1, 1);

        _timeElapsed += Time.deltaTime;
    }

    private void LateUpdate()
    {
        //transform.LookAt(_mainCamera.transform);

        transform.rotation = _mainCamera.transform.rotation;
        transform.Rotate(0, 180, 0);
    }
}
