using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Mozgási sebességek")]
    public float horizontalSpeed = 5f; // W, A, S, D
    public float verticalSpeed = 3f;   // Q, E

    void Update()
    {
        // Előre / hátra
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * horizontalSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * horizontalSpeed * Time.deltaTime);
        }

        // Balra / jobbra
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * horizontalSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime);
        }

        // Fel / le
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime);
        }
    }
}