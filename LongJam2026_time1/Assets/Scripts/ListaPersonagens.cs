using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ListaPersonagens : MonoBehaviour
{
    // Lista com os Bichos
    public List<FishSO> todosOsIndividuos = new List<FishSO>();

    // O [ContextMenu] cria uma opcoo no menu do componente
    [ContextMenu("Carregar Todos os Individuos")]


    public void CarregarAutomaticamente()
    {
#if UNITY_EDITOR
        todosOsIndividuos.Clear();

        // Busca o ID dos Bichos
        string[] guids = AssetDatabase.FindAssets("t:FishSO");

        foreach (string guid in guids)
        {
            // Converte o ID em caminho de pasta (ex: Assets/SO/Peixe.asset)
            string caminho = AssetDatabase.GUIDToAssetPath(guid);
            
            // Carrega o arquivo
            FishSO asset = AssetDatabase.LoadAssetAtPath<FishSO>(caminho);
            
            todosOsIndividuos.Add(asset);
        }

        // Avisa ao Unity que o objeto mudou e precisa salvar a lista
        EditorUtility.SetDirty(this);
        Debug.Log($"Sucesso! {todosOsIndividuos.Count} indivíduos encontrados e adicionados.");
#endif
    }

    public FishSO SortearBicho(string tribo)
    {
        List<FishSO> candidatos = todosOsIndividuos.FindAll(x => x.fishTribe.ToString().ToLower() == tribo.ToLower());

        if(candidatos.Count > 0)
        {
            return candidatos[Random.Range(0, candidatos.Count)];
        }

        Debug.LogError("Nenhum bicho encontrado para a tribo: " + tribo);
        return null;
    }



}
