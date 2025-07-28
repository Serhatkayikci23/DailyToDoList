using UnityEngine;
using TMPro; 
using UnityEngine.UI; 

public class ToDoList : MonoBehaviour
{
    public TMP_InputField taskInputField; 
    public Button addTaskButton;          
    public Transform taskListParent;     
    public GameObject taskPrefab;         

    void Start()
    {
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
    }

    // Üstünü çizme fonksiyonu
    void ToggleTaskCompletion(TMP_Text taskText, bool isChecked)
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
}
}