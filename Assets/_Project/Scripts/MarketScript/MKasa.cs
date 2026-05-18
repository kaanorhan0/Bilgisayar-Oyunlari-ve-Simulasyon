using UnityEngine;

public class MKasa : MonoBehaviour
{
    public void AlisverisiBitir()
    {
        MGameManager manager = Object.FindFirstObjectByType<MGameManager>();

        if (manager != null)
        {
            manager.AlisverisiTamamla();
        }
    }
}