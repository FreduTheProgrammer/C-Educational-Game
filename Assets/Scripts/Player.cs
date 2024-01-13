using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    // Allows you to hold down a key for movement.
    [SerializeField] private bool isRepeatedMovement = false;
    // Time in seconds to move between one grid position and the next.
    [SerializeField] private float moveDuration = 0.1f;
    // The size of the grid
    [SerializeField] private float gridSize = 1f;

    private bool isMoving = false;

    public LayerMask solidObjectsLayer;
    [SerializeField] private TextMeshProUGUI signText;
    public LayerMask signLayer;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    // Update is called once per frame
    private void Update()
    {
        // Only process on move at a time.
        if (!isMoving)
        {
            
            // Accomodate two different types of moving.
            System.Func<KeyCode, bool> inputFunction;
            if (isRepeatedMovement)
            {
                // GetKey repeatedly fires.
                inputFunction = Input.GetKey;
            }
            else
            {
                // GetKeyDown fires once per keypress
                inputFunction = Input.GetKeyDown;
            }

            // If the input function is active, move in the appropriate direction.
            if (inputFunction(KeyCode.W))
            {

                animator.SetFloat("moveX", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("moveY", Input.GetAxisRaw("Vertical"));

                Vector2 startPosition = transform.position;
                Vector2 endPosition = startPosition + (Vector2.up * gridSize);
                if (IsWalkable(endPosition))
                {
                    CheckIfSign(endPosition);
                    StartCoroutine(Move(Vector2.up));
                }
            }
            else if (inputFunction(KeyCode.S))
            {

                animator.SetFloat("moveX", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("moveY", Input.GetAxisRaw("Vertical"));

                Vector2 startPosition = transform.position;
                Vector2 endPosition = startPosition + (Vector2.down * gridSize);
                if (IsWalkable(endPosition))
                {
                    CheckIfSign(endPosition);
                    StartCoroutine(Move(Vector2.down));
                }
            }
            else if (inputFunction(KeyCode.A))
            {
                GetComponent<SpriteRenderer>().flipX = false;
                animator.SetFloat("moveX", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("moveY", Input.GetAxisRaw("Vertical"));

                Vector2 startPosition = transform.position;
                Vector2 endPosition = startPosition + (Vector2.left * gridSize);
                if (IsWalkable(endPosition))
                {
                    CheckIfSign(endPosition);
                    StartCoroutine(Move(Vector2.left));
                }
            }
            else if (inputFunction(KeyCode.D))
            {
                GetComponent<SpriteRenderer>().flipX = true;
                animator.SetFloat("moveX", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("moveY", Input.GetAxisRaw("Vertical"));

                Vector2 startPosition = transform.position;
                Vector2 endPosition = startPosition + (Vector2.right * gridSize);
                if (IsWalkable(endPosition))
                {
                    CheckIfSign(endPosition);
                    StartCoroutine(Move(Vector2.right));
                }
            }
        }
    }

    // Smooth movement between grid positions.
    private IEnumerator Move(Vector2 direction)
    {
        // Record that we're moving so we don't accept more input.
        isMoving = true;
        animator.SetBool("isMoving", isMoving);
        // Make a note of where we are and where we are going.
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);
        // Smoothly move in the desired direction taking the required time.
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;
            transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }

        // Make sure we end up exactly where we want.
        transform.position = endPosition;

        // We're no longer moving so we can accept another move input.
        isMoving = false;
        animator.SetBool("isMoving", isMoving);
    }
    private bool IsWalkable(Vector3 TargetPosition)
    {
   
        if (Physics2D.OverlapCircle(TargetPosition, 0.2f, solidObjectsLayer) != null) return false;

        return true;

    }
    private void CheckIfSign(Vector3 TargetPosition)
    {

        if (Physics2D.OverlapPoint(TargetPosition, signLayer) != null)
        {
            Collider2D[] collision = new Collider2D[1];
            ContactFilter2D filter = new();
            filter.SetLayerMask(signLayer);
            Physics2D.OverlapPoint(TargetPosition, filter, collision);
            signText.text = collision[0].gameObject.GetComponent<Sign>().text;
            signText.gameObject.SetActive(true);
        }
        else
        {
            signText.gameObject.SetActive(false);
        }
    }
}
