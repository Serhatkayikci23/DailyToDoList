using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskListWrapper
{
    public List<string> tasks;
    public List<string> dates;
}

public class DataManager : MonoBehaviour
{
    private const string SaveKey = "taskList";

  
    public void SaveTasks(List<string> tasks, List<string> dates)
    {
        TaskListWrapper wrapper = new TaskListWrapper
        {
            tasks = tasks,
            dates = dates
        };

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Görevler kaydedildi: " + json);
    }

   
    public TaskListWrapper LoadTasks()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            return new TaskListWrapper
            {
                tasks = new List<string>(),
                dates = new List<string>()
            };
        }

        string json = PlayerPrefs.GetString(SaveKey);
        TaskListWrapper wrapper = JsonUtility.FromJson<TaskListWrapper>(json);
        Debug.Log("Kayıt yüklendi: " + json);
        return wrapper;
    }

}