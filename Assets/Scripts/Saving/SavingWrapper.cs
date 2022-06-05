using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }    
        }
        
        void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}
