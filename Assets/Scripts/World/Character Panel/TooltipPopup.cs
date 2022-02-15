using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;

public class TooltipPopup : MonoBehaviour
{
    [SerializeField] private RectTransform _popupObject;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _padding;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        FollowCursor();
    }

    void FollowCursor()
    {
        if (!_popupObject.gameObject.activeSelf) { return; }

            Vector3 newPos = Input.mousePosition + _offset;
            newPos.z = 0f;
            float rightEdgeToScreenEdgeDistance = Screen.width - (newPos.x + _popupObject.rect.width / 2) - _padding;
            if (rightEdgeToScreenEdgeDistance < 0)
            {
                newPos.x += rightEdgeToScreenEdgeDistance;
            }
            float leftEdgeToScreenEdgeDistance = 0 - (newPos.x - _popupObject.rect.width / 2) + _padding;
            if (leftEdgeToScreenEdgeDistance > 0)
            {
                newPos.x += leftEdgeToScreenEdgeDistance;
            }
            float topEdgeToScreenEdgeDistance = Screen.height - (newPos.y + _popupObject.rect.height) - _padding;
            if (topEdgeToScreenEdgeDistance < 0)
            {
                newPos.y += topEdgeToScreenEdgeDistance;
            }
            _popupObject.transform.position = newPos;
    }

    public void DisplayInfo(Weapons wp, Vector3 position)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_popupObject);

        StringBuilder builder = new StringBuilder();
        builder.Append(wp.GetTooltipText());
        _infoText.text = builder.ToString();

        _popupObject.gameObject.SetActive(true);
    }

    public void HideInfo()
    {
        _popupObject.gameObject.SetActive(false);
    }
}
