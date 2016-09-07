using UnityEngine;

public class TileSet : ScriptableObject {
    //ScriptableObject: Guardam informações e não precisam estar atrelados a um GameObject na cena.
    //Podem salvar Assets no proejto.
    //São usados para armazenar dados, mas também para ajudar a serializar objetos e pode ser instanciado na cena.

    public Transform[] prefabs = new Transform[0];

}
