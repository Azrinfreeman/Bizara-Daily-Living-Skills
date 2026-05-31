using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidebarController : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform sidebarRect;
    public List<RectTransform> contentElements;
    
    [Header("Settings")]
    public float animationDuration = 0.3f;
    public float sidebarOpenAnchorX = 0.22f; // The max X anchor when open
    
    private bool isCollapsed = false;
    private Coroutine animationCoroutine;

    public void ToggleSidebar()
    {
        isCollapsed = !isCollapsed;
        
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        
        animationCoroutine = StartCoroutine(AnimateSidebar());
    }

    private IEnumerator AnimateSidebar()
    {
        float elapsedTime = 0f;
        
        // Sidebar targets
        Vector2 sidebarStartPos = sidebarRect.anchoredPosition;
        float sidebarTargetX = isCollapsed ? -sidebarRect.rect.width : 0f;
        Vector2 sidebarTargetPos = new Vector2(sidebarTargetX, sidebarStartPos.y);

        // Content targets
        float contentStartAnchor = contentElements.Count > 0 && contentElements[0] != null ? contentElements[0].anchorMin.x : sidebarOpenAnchorX;
        float contentTargetAnchor = isCollapsed ? 0f : sidebarOpenAnchorX;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);
            float curveT = Mathf.SmoothStep(0f, 1f, t);

            // Animate Sidebar
            if (sidebarRect != null)
            {
                sidebarRect.anchoredPosition = Vector2.Lerp(sidebarStartPos, sidebarTargetPos, curveT);
            }

            // Animate Content Anchors
            foreach (var contentRect in contentElements)
            {
                if (contentRect != null)
                {
                    Vector2 minAnchor = contentRect.anchorMin;
                    minAnchor.x = Mathf.Lerp(contentStartAnchor, contentTargetAnchor, curveT);
                    contentRect.anchorMin = minAnchor;
                }
            }

            yield return null;
        }

        // Finalize values
        if (sidebarRect != null)
        {
            sidebarRect.anchoredPosition = sidebarTargetPos;
        }
        
        foreach (var contentRect in contentElements)
        {
            if (contentRect != null)
            {
                Vector2 minAnchor = contentRect.anchorMin;
                minAnchor.x = contentTargetAnchor;
                contentRect.anchorMin = minAnchor;
            }
        }
    }

    public void GoToScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
