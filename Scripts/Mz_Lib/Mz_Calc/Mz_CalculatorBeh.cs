using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class Mz_CalculatorBeh: MonoBehaviour
{
	private const string devideby0 = "devide by zero";
	private const string error = "error";
	
	public Transform middleCenter;
	public GameObject effectSpriteObj;
	private GameObject effectSprite_Instance;
	private tk2dAnimatedSprite effectSprite; 
	
//    public List<GameObject> themeCalcLists = new List<GameObject>(4);
//    public GUISkin calc_Skin;
//    private GameObject calc;

    private string domainText = "0";
//	private string domainTempValue = "0";
	private string recycleDomainText = "";

	private string operanceText = string.Empty;
    private string nextJobOperation = string.Empty;
	private string recycleOperation = "";

    private string rangeText = string.Empty;
//	private string rangeTempValue = "0";
	private string recycleRangeText = "";

    private string resultText = "0";
	private bool _isMinus = false; 
	private bool _isEnter = false;

	public tk2dTextMesh result_Textmesh;
	
    private float keyValue;
	
	
	
    // Use this for initialization
    void Start ()
	{
//		calc = Instantiate (themeCalcLists[Main.ThemeIndex]) as GameObject;
        //calc.transform.parent = middleCenter;
        //calc.transform.localPosition = Vector3.zero;
		
//		Mz_ResizeScale.CalculationScale(calc.transform);
    }

    public int GetDisplayResultTextToInt() {
        int temp_value = int.Parse(resultText);
        return temp_value;
    }

    // Update is called once per frame
    void Update()
    {
		
    }

    void ReDrawResult()
    {
/*		if(result_Textmesh) 
		{
			if(resultText != string.Empty && resultText != devideby0 && resultText != error)
            {
                if (operanceText == string.Empty)
                {
                    #region <!-- Domain text data.

                    if (!resultText.Contains("."))
                    {
                        //<!-- Domain text data.
                        if (resultText.Length >= 4 && resultText.Length < 7)
                        {
                            domainTempValue = resultText.Insert(resultText.Length - 3, ",");
                        }
                        else if (resultText.Length >= 7 && resultText.Length < 10)
                        {
                            string temp = resultText.Insert(resultText.Length - 3, ",");
                            domainTempValue = temp.Insert(resultText.Length - 6, ",");
                        }
                        else if (resultText.Length >= 10 && resultText.Length < 13)
                        {
                            string temp = resultText.Insert(resultText.Length - 3, ",");
                            string temp_2 = temp.Insert(resultText.Length - 6, ",");
                            domainTempValue = temp_2.Insert(resultText.Length - 9, ",");
                        }
                        else if (resultText.Length >= 13 && resultText.Length < 16)
                        {
                            string temp = resultText.Insert(resultText.Length - 3, ",");
                            string temp_2 = temp.Insert(resultText.Length - 6, ",");
                            string temp_3 = temp_2.Insert(resultText.Length - 9, ",");
                            domainTempValue = temp_3.Insert(resultText.Length - 12, ",");
                        }
                        else if (resultText.Length >= 16)
                        {
                            string temp = resultText.Insert(resultText.Length - 3, ",");
                            string temp_2 = temp.Insert(resultText.Length - 6, ",");
                            string temp_3 = temp_2.Insert(resultText.Length - 9, ",");
							string temp_4 = temp_3.Insert(resultText.Length - 12, ",");
                            domainTempValue = temp_4.Insert(resultText.Length - 15, ",");
                        }
                        else
                        {
                            domainTempValue = resultText;
                        }
						
						//<!-- Display result text.
                        result_Textmesh.text = domainTempValue;
                    }
					else if(resultText.Contains(".")) 
					{
						string[] newResultText = resultText.Split('.');
						string newDomainText_Integer = newResultText[0]; 
						string newFloatingPoint = newResultText[1];
						Debug.LogWarning ("Contain(.)");
						
                        //<!-- Domain Data.
                        if (newDomainText_Integer.Length >= 4 && newDomainText_Integer.Length < 7)
                        {
                            domainTempValue = newDomainText_Integer.Insert(newDomainText_Integer.Length - 3, ",");
                        }
                        else if (newDomainText_Integer.Length >= 7 && newDomainText_Integer.Length < 10)
                        {
                            string temp = newDomainText_Integer.Insert(newDomainText_Integer.Length - 3, ",");
                            domainTempValue = temp.Insert(newDomainText_Integer.Length - 6, ",");
                        }
                        else if (newDomainText_Integer.Length >= 10 && newDomainText_Integer.Length < 13)
                        {
                            string temp = newDomainText_Integer.Insert(newDomainText_Integer.Length - 3, ",");
                            string temp_2 = temp.Insert(newDomainText_Integer.Length - 6, ",");
                            domainTempValue = temp_2.Insert(newDomainText_Integer.Length - 9, ",");
                        }
                        else if (newDomainText_Integer.Length >= 13 && newDomainText_Integer.Length < 16)
                        {
                            string temp = newDomainText_Integer.Insert(newDomainText_Integer.Length - 3, ",");
                            string temp_2 = temp.Insert(newDomainText_Integer.Length - 6, ",");
                            string temp_3 = temp_2.Insert(newDomainText_Integer.Length - 9, ",");
                            domainTempValue = temp_3.Insert(newDomainText_Integer.Length - 12, ",");
                        }
                        else if (newDomainText_Integer.Length >= 16)
                        {
                            string temp = newDomainText_Integer.Insert(newDomainText_Integer.Length - 3, ",");
                            string temp_2 = temp.Insert(newDomainText_Integer.Length - 6, ",");
                            string temp_3 = temp_2.Insert(newDomainText_Integer.Length - 9, ",");
                            string temp_4 = temp_3.Insert(newDomainText_Integer.Length - 12, ",");
                            domainTempValue = temp_4.Insert(newDomainText_Integer.Length - 15, ",");
                        }
                        else
                        {
                            domainTempValue = newDomainText_Integer;
                        }
						
						if(_isEnter) {
							char[] zero = {'0'};
							if(newFloatingPoint.EndsWith("0")) {
	//							newFloatingPoint = newFloatingPoint.Remove(newFloatingPoint.Length -1);
								newFloatingPoint = newFloatingPoint.TrimEnd(zero);
								Debug.LogWarning("Trim");
								Debug.LogWarning("newFloatingPoint ::" + newFloatingPoint);
								
								if(newFloatingPoint != "0" && newFloatingPoint != string.Empty) {								
									domainTempValue = domainTempValue + "." + newFloatingPoint;
								}
								else {
									domainTempValue = domainTempValue;
								}
							}
							else 
							{
								domainTempValue = domainTempValue + "." + newFloatingPoint;
							}
						}
						else {
							domainTempValue = domainTempValue + "." + newFloatingPoint;
						}
						
						//<!-- Display result text.
                        result_Textmesh.text = domainTempValue;
                    }

                    #endregion
                }
                else if(operanceText != string.Empty)
                {
                    #region <!-- Manage Domain text data.

                    //<!-- Domain Data. -->//
                    if (!domainText.Contains("."))
                    {
                        if (domainText.Length >= 4 && domainText.Length < 7)
                        {
                            domainTempValue = domainText.Insert(domainText.Length - 3, ",");
                        }
                        else if (domainText.Length >= 7 && domainText.Length < 10)
                        {
                            string temp = domainText.Insert(domainText.Length - 3, ",");
                            domainTempValue = temp.Insert(domainText.Length - 6, ",");
                        }
                        else if (domainText.Length >= 10 && domainText.Length < 13)
                        {
                            string temp = domainText.Insert(domainText.Length - 3, ",");
                            string temp_2 = temp.Insert(domainText.Length - 6, ",");
                            domainTempValue = temp_2.Insert(domainText.Length - 9, ",");
                        }
                        else if (domainText.Length >= 13 && domainText.Length < 16)
                        {
                            string temp = domainText.Insert(domainText.Length - 3, ",");
                            string temp_2 = temp.Insert(domainText.Length - 6, ",");
                            string temp_3 = temp_2.Insert(domainText.Length - 9, ",");
                            domainTempValue = temp_3.Insert(domainText.Length - 12, ",");
                        }
                        else if (domainText.Length >= 16)
                        {
                            string temp = domainText.Insert(domainText.Length - 3, ",");
                            string temp_2 = temp.Insert(domainText.Length - 6, ",");
                            string temp_3 = temp_2.Insert(domainText.Length - 9, ",");
                            string temp_4 = temp_3.Insert(domainText.Length - 12, ",");
                            domainTempValue = temp_4.Insert(domainText.Length - 15, ",");
                        }
                        else
                        {
                            domainTempValue = domainText;
                        }
						
						//<!-- Display result text.
                        result_Textmesh.text = domainTempValue;
                    }
					else if(domainText.Contains(".")) 
					{
						string[] newSplitDomainText = domainText.Split('.');
						string newDomainText_Integer = newSplitDomainText[0]; 
						string newFloatingPoint = newSplitDomainText[1];
						
                        //<!-- Domain Data.
                        if (newDomainText_Integer.Length >= 4 && newDomainText_Integer.Length < 7)
                        {
                            domainTempValue = newDomainText_Integer.Insert(newDomainText_Integer.Length - 3, ",");
                        }
                        else if (newDomainText_Integer.Length >= 7 && newDomainText_Integer.Length < 10)
                        {
                            string temp = newDomainText_Integer.Insert(newDomainText_Integer.Length - 3, ",");
                            domainTempValue = temp.Insert(newDomainText_Integer.Length - 6, ",");
                        }
                        else if (newDomainText_Integer.Length >= 10 && newDomainText_Integer.Length < 13)
                        {
                            string temp = newDomainText_Integer.Insert(newDomainText_Integer.Length - 3, ",");
                            string temp_2 = temp.Insert(newDomainText_Integer.Length - 6, ",");
                            domainTempValue = temp_2.Insert(newDomainText_Integer.Length - 9, ",");
                        }
                        else if (newDomainText_Integer.Length >= 13 && newDomainText_Integer.Length < 16)
                        {
                            string temp = newDomainText_Integer.Insert(newDomainText_Integer.Length - 3, ",");
                            string temp_2 = temp.Insert(newDomainText_Integer.Length - 6, ",");
                            string temp_3 = temp_2.Insert(newDomainText_Integer.Length - 9, ",");
                            domainTempValue = temp_3.Insert(newDomainText_Integer.Length - 12, ",");
                        }
                        else if (newDomainText_Integer.Length >= 16)
                        {
                            string temp = newDomainText_Integer.Insert(newDomainText_Integer.Length - 3, ",");
                            string temp_2 = temp.Insert(newDomainText_Integer.Length - 6, ",");
                            string temp_3 = temp_2.Insert(newDomainText_Integer.Length - 9, ",");
                            string temp_4 = temp_3.Insert(newDomainText_Integer.Length - 12, ",");
                            domainTempValue = temp_4.Insert(newDomainText_Integer.Length - 15, ",");
                        }
                        else
                        {
                            domainTempValue = newDomainText_Integer;
                        }
						
						char[] zero = {'0'};
						if(newFloatingPoint.EndsWith("0")) {
//							newFloatingPoint = newFloatingPoint.Remove(newFloatingPoint.Length -1);
							newFloatingPoint = newFloatingPoint.TrimEnd(zero);
							Debug.LogWarning("Trim");
							Debug.LogWarning("newFloatingPoint ::" + newFloatingPoint);
							
							if(newFloatingPoint != "0" && newFloatingPoint != string.Empty) {								
								domainTempValue = domainTempValue + "." + newFloatingPoint;
							}
							else {
								domainTempValue = domainTempValue;
							}
						}
						else {
							domainTempValue = domainTempValue + "." + newFloatingPoint;
						}
						
						//<!-- Display result text.
                        result_Textmesh.text = domainTempValue;
                    }

                    #endregion

					#region <!-- Manage Range text data.
					
					//<!-- Range Data.
					if(rangeText != string.Empty && !rangeText.Contains("."))
					{
						if(rangeText.Length >= 4 && rangeText.Length < 7) 
						{
							rangeTempValue = rangeText.Insert(rangeText.Length - 3, ",");
							result_Textmesh.text = domainTempValue + operanceText + rangeTempValue;
						}
						else if(rangeText.Length >= 7) 
						{
							string temp = rangeText.Insert(rangeText.Length - 3, ",");
							rangeTempValue = temp.Insert(rangeText.Length - 6, ",");
							
							result_Textmesh.text = domainTempValue + operanceText + rangeTempValue;
						}
						else {
							rangeTempValue = rangeText;
							result_Textmesh.text = domainTempValue + operanceText + rangeTempValue;
						}
					}
                    else if (rangeText != string.Empty && rangeText.Contains(".")) 
                    {
                        string[] newRangeSplitData = rangeText.Split('.');
                        string range_Integer = newRangeSplitData[0];
                        string range_FloatingPoint = newRangeSplitData[1];

                        if (range_Integer.Length >= 4 && range_Integer.Length < 7)
                        {
                            rangeTempValue = range_Integer.Insert(range_Integer.Length - 3, ",");
                        }
                        else if (range_Integer.Length >= 7)
                        {
                            string temp = range_Integer.Insert(range_Integer.Length - 3, ",");
                            rangeTempValue = temp.Insert(range_Integer.Length - 6, ",");
                        }
                        else
                        {
                            rangeTempValue = range_Integer;
                        }

                        //<!-- Display Data. -->
                        result_Textmesh.text = domainTempValue + operanceText + rangeTempValue + "." + range_FloatingPoint;
                    }
                    else if (rangeText == string.Empty)
                    {
                        result_Textmesh.text = domainTempValue + operanceText;
                    }
					
					#endregion
                }
			}
			else {
				result_Textmesh.text = resultText;
			}
		}
*/

        double numericalFormat = double.Parse(resultText);

        result_Textmesh.text = numericalFormat.ToString("N0");
        result_Textmesh.Commit();
		_isEnter = false;
    }

    public void GetInput(string key)
    {
        switch (key)
        {
            case "0":
                keyValue = 0;
                EnterKeyMechanism();
                break;
            case "1":
                keyValue = 1;
                EnterKeyMechanism();
                break;
            case "2":
                keyValue = 2;
                EnterKeyMechanism();
                break;
            case "3":
                keyValue = 3;
                EnterKeyMechanism();
                break;
            case "4":
                keyValue = 4;
                EnterKeyMechanism();
                break;
            case "5":
                keyValue = 5;
                EnterKeyMechanism();
                break;
            case "6":
                keyValue = 6;
                EnterKeyMechanism();
                break;
            case "7":
                keyValue = 7;
                EnterKeyMechanism();
                break;
            case "8":
                keyValue = 8;
                EnterKeyMechanism();
                break;
            case "9":
                keyValue = 9;
                EnterKeyMechanism();
                break;
            //<!-- <Exception form other case, It has more different.> -->
            case "dot": 
				if(operanceText == string.Empty) 
				{
					if(domainText == "0" && domainText.Length < 9) 
					{
						domainText = "0.";
					}
					else if(domainText != "0" && !domainText.Contains(".") && domainText.Length < 9)
					{
						domainText = domainText + ".";
					}
					
					resultText = domainText;
					result_Textmesh.text = resultText;
					ReDrawResult();
				}
				else if(operanceText != string.Empty) 
				{
					if(rangeText == "0" || rangeText == string.Empty && rangeText.Length < 9) 
					{
						rangeText = "0.";
					}
					else if(rangeText != "0" && !rangeText.Contains(".") && rangeText.Length < 9)
					{
						rangeText = rangeText + ".";
					}
					
					resultText = domainText + operanceText + rangeText;
					result_Textmesh.text = resultText;
					ReDrawResult();
				}
                break;
            case "add":
                if (resultText.Length >= 26) {
					return;
				}
				nextJobOperation = "+";
				CheckOperance();
                break;
            case "minus":
                if (resultText.Length >= 26) {
					return;
				}
				nextJobOperation = "-";
				CheckOperance();
                break;
            case "multi":
                if (resultText.Length >= 26) {
					return;
				}
				nextJobOperation = "x";
				CheckOperance();
                break;
            case "divide":
                if (resultText.Length >= 26) {
					return;
				}
				nextJobOperation = "/";
				CheckOperance();
                break;
            case "c":
				this.ClearCalcMechanism();
                break;
            case "equal":
                if (resultText.Length >= 26) {
					return;
				}

				//<!-- Calculation.
				_isEnter = true;
				string result =  CalculationProcessing();
			
				if(result != string.Empty) 
				{
					if(result.Contains("-")) {
						_isMinus = true;
					}
					else {
						_isMinus = false;
					}
				
					resultText = result;
					ClearOperation();
					ReDrawResult();
				
					if(effectSprite_Instance == null) 
					{
						effectSprite_Instance = Instantiate(effectSpriteObj) as GameObject;
                        //effectSprite_Instance.transform.parent = calc.transform;
                        effectSprite_Instance.transform.parent = this.transform;
						effectSprite_Instance.transform.localPosition = new Vector3(80, 110, 0);
						
//						Mz_ResizeScale.CalculationScale(effectSprite_Instance.transform);
					
						effectSprite = effectSprite_Instance.GetComponent<tk2dAnimatedSprite>();
						effectSprite.animationCompleteDelegate = animationCompleteDelegate;
					}
				}
                break;
		case "backspace":
			if(operanceText == string.Empty) 
			{
				if(domainText != "0" && domainText.Length != 0) 
				{
					domainText = domainText.Remove(domainText.Length - 1);
					
					resultText = domainText;
					result_Textmesh.text = resultText;
					ReDrawResult();
				}
			}
			else if(operanceText != string.Empty) 
			{
				if(rangeText != "0" && rangeText.Length != 0) 
				{
					rangeText = rangeText.Remove(rangeText.Length - 1);
					
					resultText = domainText + operanceText + rangeText;
					result_Textmesh.text = resultText;
					ReDrawResult();
				}
			}
			break;
		default:
			break;
        }
    }
    
    public void animationCompleteDelegate(tk2dAnimatedSprite sprite, int id)  {
    	Debug.LogWarning("Wow !!");
		Destroy(effectSprite_Instance);
    }
	
	public void ClearCalcMechanism() {		
		resultText = "0";
        ClearOperation();
		ReDrawResult();
	}
	
    private void EnterKeyMechanism()
    {
        if (resultText.Length >= 26) 
			return;

        if (operanceText == string.Empty) {
			if(domainText != "0" && domainText.Length < 9) {
            	domainText = domainText + keyValue;
			}
			else if(domainText.Length < 9) {
				domainText = keyValue.ToString();
			}
			
			resultText = domainText;
			result_Textmesh.text = resultText;
			ReDrawResult();
        }
        else if (operanceText != string.Empty) {
			if(rangeText == "0") {
				rangeText = keyValue.ToString();
			}
			else if(rangeText.Length < 9) {
            	rangeText = rangeText + keyValue;
			}
			
        	resultText = domainText + operanceText + rangeText;
			result_Textmesh.text = resultText;
			ReDrawResult();
        }
    }
	
	private void CheckOperance ()
	{	
		if(resultText == "0") {
			//<!-- Store operation as first job.
			operanceText = nextJobOperation;
			nextJobOperation = string.Empty;
			
			resultText = domainText + operanceText;	
			result_Textmesh.text = resultText;
			ReDrawResult();	
		}
        else if (resultText != "0" && resultText != devideby0 && resultText != error)
		{
			if(_isMinus && resultText.Contains("-") && !resultText.Contains("+") && !resultText.Contains("x") && !resultText.Contains("/"))
			{
				Debug.LogWarning("no have operation in job.");
				//<!-- Store operation as first job.
				operanceText = nextJobOperation;
				nextJobOperation = string.Empty;
				
				domainText = resultText;
				
				resultText = domainText + operanceText;
				result_Textmesh.text = resultText;
				ReDrawResult();
			}			
			else if(!_isMinus && !resultText.Contains("-") && !resultText.Contains("+") && !resultText.Contains("x") && !resultText.Contains("/"))
			{
				Debug.LogWarning("no have operation in job.");
				//<!-- Store operation as first job.
				operanceText = nextJobOperation;
				nextJobOperation = string.Empty;
				
				domainText = resultText;
				
				resultText = domainText + operanceText;
				result_Textmesh.text = resultText;
				ReDrawResult();
			}
			else    //<!-- Have operation in job.
			{ 
				Debug.LogWarning("Have operation in job");
				
				if (rangeText == string.Empty) {				
                    operanceText = nextJobOperation;
                    nextJobOperation = string.Empty;

                    ReDrawResult();
                }
				else if(rangeText != string.Empty) {		//rangeText != "0".
					//<!-- Calculation.
					resultText = CalculationProcessing();
					
					domainText = resultText;
					rangeText = string.Empty;
					
					//<!-- Store operation after processing first job
					operanceText = nextJobOperation;
					nextJobOperation = string.Empty;
					
					if(domainText != devideby0 && domainText != error) {
						resultText = domainText + operanceText;
						ReDrawResult();
					}
					else {
						Debug.LogError("Err");
						resultText = domainText;
						ReDrawResult();
					}
				}
			}
		}
		else {
			operanceText = string.Empty;
		}
	}
    
    /// <summary>
    /// Clear Domain, Range and Operator.
    /// </summary>
    private void ClearOperation()
    {
		recycleDomainText = resultText;
		recycleOperation = operanceText;
		recycleRangeText = rangeText;
		
        domainText = "0";
//        domainTempValue = "0";
		
        operanceText = string.Empty;
		
        rangeText = string.Empty;
//        rangeTempValue = "0";
    }
	
	/// <summary>
	/// Calculations the processing.
	/// </summary>
	/// <returns>
	/// The processor.
	/// </returns>
	private string CalculationProcessing() 
	{
		CultureInfo ci = new CultureInfo("en-us");
		
		if (operanceText != string.Empty && rangeText != string.Empty && domainText != devideby0 && domainText != error)
        {			
            if (operanceText == "+")
            {
                double a = double.Parse(domainText);
                double b = double.Parse(rangeText);

                double r = Mz_Core_Calculator.ADD(a, b);

                return r.ToString("f7", ci);
            }
            else if (operanceText == "-")
            {
                double a = double.Parse(domainText);
                double b = double.Parse(rangeText);

                double r = Mz_Core_Calculator.Minus(a, b);

                return r.ToString("f7", ci);
            }
            else if (operanceText == "x")
            {
                double a = double.Parse(domainText);
                double b = double.Parse(rangeText);

               	double r = Mz_Core_Calculator.Multiply(a, b);

                return r.ToString("f7", ci);
            }
            else if (operanceText == "/")
            {
				try {
                	double a = double.Parse(domainText);
                	double b = double.Parse(rangeText);
					
					if (b != 0) {
                    	double r = Mz_Core_Calculator.Divide(a, b);
                   		return r.ToString("f7", ci);
                	}
                	else
						return devideby0;
				}
				catch {
					return Mz_CalculatorBeh.error;
				}
            }
        }
        else if (operanceText == string.Empty)
        {
            if (_isEnter)
            {
                if (recycleOperation == "+")
                {
                    double a = double.Parse(recycleDomainText);
                    double b = double.Parse(recycleRangeText);

                    double r = Mz_Core_Calculator.ADD(a, b);
				
					operanceText = recycleOperation;
					rangeText = recycleRangeText;

                    return r.ToString("f7", ci);
                }
                else if (recycleOperation == "-")
                {
                    double a = double.Parse(recycleDomainText);
                    double b = double.Parse(recycleRangeText);

                    double r = Mz_Core_Calculator.Minus(a, b);
				
					operanceText = recycleOperation;
					rangeText = recycleRangeText;

                    return r.ToString("f7", ci);
                }
                else if (recycleOperation == "x")
                {
                    double a = double.Parse(recycleDomainText);
                    double b = double.Parse(recycleRangeText);

                    double r = Mz_Core_Calculator.Multiply(a, b);
				
					operanceText = recycleOperation;
					rangeText = recycleRangeText;

                    return r.ToString("f7", ci);
                }
                else if (recycleOperation == "/")
                {
					try {
	                    double a = double.Parse(recycleDomainText);
	                    double b = double.Parse(recycleRangeText);
	
	                    if (b != 0)
	                    {
	                        double r = Mz_Core_Calculator.Divide(a, b);
					
							operanceText = recycleOperation;
							rangeText = recycleRangeText;
	
	                        return r.ToString("f7", ci);
	                    }
	                    else
	                        return devideby0;
					}
					catch {
						return Mz_CalculatorBeh.error;
					}
                }
            }
        }
        else if (rangeText == string.Empty)
        {
            return string.Empty;
        }
		
		return error;
	}
}
