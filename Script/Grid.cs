using UnityEngine;

public class Grid : MonoBehaviour {

    public float width = 32.0f; //Largura do Grid
    public float height = 32.0f; //Altura do Grid

    public Color color = Color.white; //Cor do Grid

    public Transform tilePrefab;

    public TileSet tileSet;

    public float SceneViewWindowY = 800.0f; //Tamanho (possível) da SceneView Window Y
    public float SceneViewWindowX = 1200.0f; //Tamanho (possível) da SceneView Window X
    public float MaxGridLenght = 1000000.0f;

    // Ajuda a colocar objetos importantes mais rápidos na cena.
    // Usa a posição do mouse relativa a Scene View
    void OnDrawGizmos()
    {
        Vector3 position = Camera.current.transform.position;
        Gizmos.color = this.color;

        #region Montando o Grid

        for (float y = position.y - SceneViewWindowY; y < position.y + SceneViewWindowY; y += height)
        {
            Gizmos.DrawLine(new Vector3(-MaxGridLenght, Mathf.Floor(y / height) * height, 0.0f),
                            new Vector3(MaxGridLenght, Mathf.Floor(y / height) * height, 0.0f));
        }

        for (float x = position.x - SceneViewWindowX; x < position.x + SceneViewWindowX; x += width)
        {
            Gizmos.DrawLine(new Vector3(Mathf.Floor(x / width) * width, -MaxGridLenght, 0.0f),
                            new Vector3(Mathf.Floor(x / width) * width, MaxGridLenght, 0.0f));
        }

        #endregion

    }
}
