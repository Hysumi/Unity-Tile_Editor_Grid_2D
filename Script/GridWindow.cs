using UnityEngine;
using UnityEditor;

public class GridWindow : EditorWindow { //Cria uma janela no editor
    Grid grid;
    
    public void init() //Primeira chamada da janela
    {
        grid = (Grid)FindObjectOfType(typeof(Grid)); //Procura o objeto que tenha o Grid
    }
	
    void OnGUI() //Vai tratar da GUI nessa janela nova
    {
        grid.color = EditorGUILayout.ColorField(grid.color, GUILayout.Width(200));
    }
}
