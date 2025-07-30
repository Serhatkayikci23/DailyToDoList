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

    public DataManager dataManager; 

    private List<string> taskList = new List<string>();
    private List<string> taskDateList = new List<string>();

    void Start()
    {
        addTaskButton.onClick.AddListener(AddTask);
        LoadTasks();
    }

    public void AddTask()
    {
        if (string.IsNullOrWhiteSpace(taskInputField.text)) return;

        string taskText = taskInputField.text;
        string taskDate = DateTime.Now.ToString("dd.MM.yyyy");

        
        GameObject newTask = Instantiate(taskPrefab, taskListParent);

       
        TMP_Text taskTMP = newTask.GetComponentInChildren<TMP_Text>();
        if (taskTMP != null)
            taskTMP.text = taskText;

        
        TMP_Text dateTMP = newTask.transform.Find("DateText")?.GetComponent<TMP_Text>();
        if (dateTMP != null)
            dateTMP.text = taskDate;

        
        Toggle toggle = newTask.GetComponentInChildren<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener((isChecked) => ToggleTaskCompletion(taskTMP, isChecked));
        }

        
        Button deleteButton = newTask.GetComponentInChildren<Button>();
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(() => DeleteTask(newTask, taskText));
        }

        
        taskList.Add(taskText);
        taskDateList.Add(taskDate);
        dataManager.SaveTasks(taskList, taskDateList);

       
        taskInputField.text = string.Empty;
    }

    public void ToggleTaskCompletion(TMP_Text taskText, bool isChecked)
    {
        if (isChecked)
            taskText.fontStyle = FontStyles.Strikethrough;
        else
            taskText.fontStyle = FontStyles.Normal;
    }

    public void DeleteTask(GameObject taskObj, string taskText)
    {
        int index = taskList.IndexOf(taskText);
        if (index >= 0)
        {
            taskList.RemoveAt(index);
            taskDateList.RemoveAt(index);
        }
        Destroy(taskObj);

        dataManager.SaveTasks(taskList, taskDateList);
    }

    private void LoadTasks()
    {
        var wrapper = dataManager.LoadTasks();

        taskList = wrapper.tasks ?? new List<string>();
        taskDateList = wrapper.dates ?? new List<string>();

        for (int i = 0; i < taskList.Count; i++)
        {
            GameObject newTask = Instantiate(taskPrefab, taskListParent);

            TMP_Text taskTMP = newTask.GetComponentInChildren<TMP_Text>();
            if (taskTMP != null)
                taskTMP.text = taskList[i];

            TMP_Text dateTMP = newTask.transform.Find("DateText")?.GetComponent<TMP_Text>();
            if (dateTMP != null)
                dateTMP.text = taskDateList[i];

            Toggle toggle = newTask.GetComponentInChildren<Toggle>();
            if (toggle != null)
            {
                toggle.onValueChanged.AddListener((isChecked) => ToggleTaskCompletion(taskTMP, isChecked));
            }

            Button deleteButton = newTask.GetComponentInChildren<Button>();
            if (deleteButton != null)
            {
                string task = taskList[i]; 
                deleteButton.onClick.AddListener(() => DeleteTask(newTask, task));
            }
        }
    }
}