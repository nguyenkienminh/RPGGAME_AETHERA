using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private float defaultNameFontsize;

    public void ShowToolTip(string _description, string _name, int _price)
    {
        skillDescription.text = _description;
        skillName.text = _name;
        skillCost.text = "Cost: " + _price; 

        ApplyGradient(_price);

        AdjustPosition();
        AdjustFontSize(skillName);
        gameObject.SetActive(true);
    }

    private void ApplyGradient(int price)
    {
        VertexGradient gradient;

        if (price < 100)
        {
            gradient = new VertexGradient(Color.green, Color.green, Color.green, Color.green);
        }
        else if (price < 500)
        {
            gradient = new VertexGradient(Color.yellow, new Color(1f, 0.5f, 0f), Color.yellow, new Color(1f, 0.5f, 0f)); 
        }
        else if (price < 1000)
        {
            gradient = new VertexGradient(Color.red, new Color(1f, 0f, 1f), Color.red, new Color(1f, 0f, 1f)); 
        }
        else if (price < 1500)
        {
            gradient = new VertexGradient(new Color(1f, 0.3f, 0.3f), new Color(1f, 0.7f, 0.7f), new Color(1f, 0.3f, 0.3f), new Color(1f, 0.7f, 0.7f)); 
        }
        else if (price < 2000)
        {
            gradient = new VertexGradient(Color.cyan, Color.blue, Color.cyan, Color.blue); 
        }
        else
        {
            gradient = new VertexGradient(Color.white, Color.magenta, Color.white, Color.magenta); 
        }

        skillCost.colorGradient = gradient;
        skillCost.enableVertexGradient = true;
    }

    public void HideToolTip()
    {
        skillName.fontSize = defaultNameFontsize;
        gameObject.SetActive(false);
    }
}
