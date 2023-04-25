using TMPro;
using UnityEngine;

public class CounterView : MonoBehaviour
{
    [SerializeField] TMP_Text textComp;

	int number = 0;

    public void Increment() {
        number++;

        UpdateView();
    }

    public void Decrement() {
        number--;

        UpdateView();
    }

    void UpdateView() {
        textComp.text = number.ToString();
    }
}
