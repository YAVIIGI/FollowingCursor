using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace YA7GI
{
    public class FollowingCursor : MonoBehaviour
    {
        [SerializeField]
        RectTransform refPointerRt;

        [SerializeField]
        Vector2 pivot = new Vector2(0.5f, 0.5f);

        [SerializeField]
        float showSpeed = 0.5f;

        [SerializeField]
        float moveSpeedRate = 0.1f;

        RectTransform pointerRt;

        float startTime = 0.0f;

        bool isNotShown = false;

        GameObject currentTarget = null;

        // Start is called before the first frame update
        void Start()
        {
            pointerRt = Instantiate(refPointerRt.gameObject).GetComponent<RectTransform>();
            pointerRt.transform.SetParent(this.transform);
            pointerRt.anchoredPosition = Vector2.zero;
        }

        // Update is called once per frame
        void Update()
        {
            var preIsNotShown = isNotShown;
            var eventSystem = EventSystem.current;
            if (eventSystem)
            {
                if (eventSystem.currentSelectedGameObject)
                {
                    Vector3 pos;
                    RectTransform rt = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
                    pos = rt.position + new Vector3((pivot.x - 0.5f) * rt.rect.width, (pivot.y - 0.5f) * rt.rect.height);

                    if (this.GetComponent<CanvasGroup>().alpha == 0)
                    {
                        pointerRt.position = pos;
                    }
                    else
                    {
                        pointerRt.position = Vector3.Lerp(pointerRt.position, pos, moveSpeedRate);
                    }
                    isNotShown = false;
                }
                else
                {
                    isNotShown = true;
                }
            }
            else
            {
                isNotShown = true;
            }

            // Debug.Log("fixedTime:" + Time.fixedTime);

            if(preIsNotShown != isNotShown)
            {
                startTime = Time.fixedTime;
            }

            if (isNotShown)
            {
                this.GetComponent<CanvasGroup>().alpha = 1.0f - ((Time.fixedTime - startTime)) * showSpeed;
                if (this.GetComponent<CanvasGroup>().alpha < 0)
                    this.GetComponent<CanvasGroup>().alpha = 0;
            }
            else
            {
                this.GetComponent<CanvasGroup>().alpha = (Time.fixedTime - startTime) * showSpeed;
                if (this.GetComponent<CanvasGroup>().alpha > 1)
                    this.GetComponent<CanvasGroup>().alpha = 1;
            }
        }
    }
}
