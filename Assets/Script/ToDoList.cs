using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToDoList : MonoBehaviour
{
   
    public TMP_InputField inputField;

    
    public GameObject taskItemPrefab;

    public Transform taskListParent;

    public Button addTaskButton;

    private void Start()
    {

        addTaskButton.onClick.AddListener(CreateTask);

    }

        private void CreateTask()
    {
        string taskText = inputField.text.Trim(); 

        if (string.IsNullOrEmpty(taskText)) return; 

        
        GameObject newTask = Instantiate(taskItemPrefab, taskListParent);

        
        TMP_Text taskTextComponent = newTask.transform.Find("TaskText").GetComponent<TMP_Text>();
        Toggle completeToggle = newTask.transform.Find("CompleteToggle").GetComponent<Toggle>();
        Button deleteButton = newTask.transform.Find("DeleteButton").GetComponent<Button>();

       
        taskTextComponent.text = taskText;

       
       completeToggle.onValueChanged.AddListener(ToggleChange);
        void ToggleChange(bool isOn)
{
   if (isOn)
{
    taskTextComponent.fontStyle = FontStyles.Strikethrough;
}
else
{
    taskTextComponent.fontStyle = FontStyles.Normal;
}
}
        deleteButton.onClick.AddListener(() =>
        {
            Destroy(newTask);
        });

        
        inputField.text = "";
    }
}   