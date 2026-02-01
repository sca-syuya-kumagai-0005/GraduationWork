using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DirectionMove : MonoBehaviour
{
    enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    [SerializeField] Direction direction;
    [SerializeField] float moveDistance;
    [SerializeField] float moveTime;
    Vector2 addDirectiom;
    [SerializeField] RectTransform targetRect;
    Vector2 defaultPosition;

    private void Awake()
    {
        defaultPosition = targetRect.anchoredPosition;
        switch (direction)
        {
            case Direction.LEFT:
                addDirectiom = Vector2.left;
                break;
            case Direction.RIGHT:
                addDirectiom = Vector2.right;
                break;
            case Direction.UP:
                addDirectiom = Vector2.up;
                break;
            case Direction.DOWN:
                addDirectiom = Vector2.down;
                break;
            default:
                Debug.LogError("•ûŒü‚ª–¢’è‹`‚Å‚·");
                break;
        }
    }

    public void PointerEnter()
    {
        targetRect.DOKill();
        targetRect.anchoredPosition = defaultPosition;
        targetRect.DOAnchorPos(addDirectiom * moveDistance + defaultPosition, moveTime).SetLoops(-1, LoopType.Yoyo);
    }

    public void PointerExit()
    {
        targetRect.DOKill();
        targetRect.DOAnchorPos(defaultPosition, moveTime);
    }
}
