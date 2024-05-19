using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Repositories.Common
{
    public class RepositoriesScriptOrder
    {
        static RepositoriesScriptOrder() {
            MonoScript[] scripts = (MonoScript[]) Resources.FindObjectsOfTypeAll(typeof(MonoScript));
            int order = -2;
            foreach (MonoScript script in scripts)
            {
                if (script == null || script.GetClass() == null) continue;

                if(script.GetClass().IsConcrete() && typeof(IRepositoryLifecycle).IsAssignableFrom(script.GetClass())){
                    MonoImporter.SetExecutionOrder(script, order);
                }
            }
        }
    }
}
