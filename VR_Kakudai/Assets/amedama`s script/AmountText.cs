using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AmountText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI SalesAmountText;
    private int SalesAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Amount( int Sales )
    {
        SalesAmount += Sales;

        SalesAmountText.text = SalesAmount.ToString();

    }
}
