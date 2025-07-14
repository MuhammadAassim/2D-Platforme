using UnityEngine;

// Ek abstract class banayi hai jiska naam hai Trap – isko inherit karke alag alag traps banayein gay
public abstract class Trap : MonoBehaviour
{
    // Yeh ek abstract method hai jo child class mein implement hoga – jab player trap ko touch karega to kya effect hoga
    public abstract void TriggerEffect(GameObject player);

    // Jab koi object trap ke trigger area mein enter kare
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Agar wo object player hai (tag se check kar rahay hain)
        if (collision.CompareTag("Player"))
        {
            // To player par trap ka effect apply kar do
            TriggerEffect(collision.gameObject);

        }
    }
}