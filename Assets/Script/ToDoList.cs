using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ToDoList : MonoBehaviour
{
    public TMP_InputField taskInputField;
    public Button addTaskButton;
    public Transform taskListParent;
    public GameObject taskPrefab;



    // Görevlerin listesini tutmak için
    private List<string> taskList = new List<string>();

    // Görevlerin tarihlerini tutacak bir liste
    private List<string> taskDateList = new List<string>();



    public TMP_Text prefabDateText;
    void Start()
    {
        // Görevleri yükle
        // Görevleri yükle
        LoadTasks();
        addTaskButton.onClick.AddListener(AddTask);


    }

    public void AddTask()
    {
        if (string.IsNullOrWhiteSpace(taskInputField.text)) return;

        GameObject newTask = Instantiate(taskPrefab, taskListParent);
        Debug.Log("Prefab Created: " + newTask);


        TMP_Text taskText = newTask.GetComponentInChildren<TMP_Text>();
        if (taskText == null)
        {
            Debug.LogError("TaskText not found in taskPrefab!");
            return;
        }
        taskText.text = taskInputField.text;




        Toggle toggle = newTask.GetComponentInChildren<Toggle>();
        if (toggle == null)
        {
            Debug.LogError("Toggle not found in taskPrefab!");
            return;
        }
        toggle.onValueChanged.AddListener((isChecked) => ToggleTaskCompletion(taskText, isChecked));



        Button deleteButton = newTask.GetComponentInChildren<Button>();

        deleteButton.onClick.AddListener(() => DeleteTask(newTask));

        taskInputField.text = string.Empty;

        TMP_Text dateText = newTask.transform.Find("DateText")?.GetComponent<TMP_Text>();
        if (dateText != null)
        {
            dateText.text = DateTime.Now.ToString("dd.MM.yyyy");
        }

        // Görev eklemeden önce kaydet
        SaveTasks();

    }
    // Görevlerin ve tarihlerin kaydedilmesi
    public void SaveTasks()
    {
        // Görevleri ve tarihleri JSON formatında kaydediyoruz
        string taskJson = JsonUtility.ToJson(new TaskListWrapper { tasks = taskList, dates = taskDateList });

        // JSON'u PlayerPrefs'te sakla
        PlayerPrefs.SetString("taskList", taskJson);
        PlayerPrefs.Save();
    }

    // Kaydedilen görevlerin yüklenmesi
    public void LoadTasks()
    {
        if (PlayerPrefs.HasKey("taskList"))
        {
            string taskJson = PlayerPrefs.GetString("taskList");

            // JSON'u tekrar listeye dönüştür
            TaskListWrapper taskListWrapper = JsonUtility.FromJson<TaskListWrapper>(taskJson);

            // Yüklenen görevleri ekle
            for (int i = 0; i < taskListWrapper.tasks.Count; i++)
            {
                string task = taskListWrapper.tasks[i];
                string date = taskListWrapper.dates[i];

                // Prefab oluştur ve görevi ekle
                GameObject newTask = Instantiate(taskPrefab, taskListParent);
                TMP_Text taskText = newTask.GetComponentInChildren<TMP_Text>();
                taskText.text = task;

                TMP_Text dateText = newTask.transform.Find("DateText")?.GetComponent<TMP_Text>();
                if (dateText != null)
                {
                    dateText.text = date;  // Kaydedilen tarihi göster
                }

                // Toggle ve silme butonlarını yeniden bağla
                Toggle toggle = newTask.GetComponentInChildren<Toggle>();
                if (toggle != null)
                {
                    toggle.onValueChanged.AddListener((isChecked) => ToggleTaskCompletion(taskText, isChecked));
                }

                Button deleteButton = newTask.GetComponentInChildren<Button>();
                deleteButton.onClick.AddListener(() => DeleteTask(newTask));
            }
        }
    }


    // üstünü çizme toggle işaretleyince
    public void ToggleTaskCompletion(TMP_Text taskText, bool isChecked)
    {
        if (isChecked)
        {
            taskText.fontStyle = FontStyles.Strikethrough;
        }
        else
        {
            taskText.fontStyle = FontStyles.Normal;
        }
    }

    public void DeleteTask(GameObject task)
    {
        Destroy(task);
        Debug.Log("Delete Button Clicked!");
        SaveTasks();

    }





}
// JSON dönüşümü için Wrapper sınıfı
[System.Serializable]
public class TaskListWrapper
{
    public List<string> tasks;  // Görev metinlerini tutar
    public List<string> dates;  // Görev tarihlerini tutar
}