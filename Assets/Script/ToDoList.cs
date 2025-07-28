using UnityEngine;
using TMPro; // TextMesh Pro'yu kullanıyoruz
using UnityEngine.UI; // UI elemanlarını kullanmak için

public class ToDoList : MonoBehaviour
{
    public TMP_InputField taskInputField; // Görev yazılacak InputField
    public Button addTaskButton;          // Ekleme Butonu
    public Transform taskListParent;      // Görevlerin listeleneceği alan (ScrollView'in Content kısmı)
    public GameObject taskPrefab;         // Görev objesinin prefab'ı (her görev için)

    void Start()
    {
        // Ekle butonuna tıklanınca AddTask fonksiyonunu çalıştır
        addTaskButton.onClick.AddListener(AddTask);
    }

    // Görev ekleme fonksiyonu
    public void AddTask()
    {
        // Eğer input boşsa işlem yapma
        if (string.IsNullOrWhiteSpace(taskInputField.text)) return;

        // Yeni görev prefab'ını oluştur
        GameObject newTask = Instantiate(taskPrefab, taskListParent);
        Debug.Log("Prefab Created: " + newTask);

        // Görev metnini al ve yerleştir
        TMP_Text taskText = newTask.GetComponentInChildren<TMP_Text>();
        if (taskText == null)
        {
            Debug.LogError("TaskText not found in taskPrefab!");
            return;
        }
        taskText.text = taskInputField.text;

        // Görevin üstünü çizme Toggle'ını ekle
        Toggle toggle = newTask.GetComponentInChildren<Toggle>();
        if (toggle == null)
        {
            Debug.LogError("Toggle not found in taskPrefab!");
            return;
        }
        toggle.onValueChanged.AddListener((isChecked) => ToggleTaskCompletion(taskText, isChecked));

        // Görev eklemesini tamamladıktan sonra input field'ı temizle
        taskInputField.text = string.Empty;
    }

    // Üstünü çizme fonksiyonu
    void ToggleTaskCompletion(TMP_Text taskText, bool isChecked)
    {
        if (isChecked)
        {
            taskText.fontStyle = FontStyles.Strikethrough; // Üstünü çiz
        }
        else
        {
            taskText.fontStyle = FontStyles.Normal; // Normal metin
        }
    }
}