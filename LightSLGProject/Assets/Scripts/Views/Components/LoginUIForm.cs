/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-28-10:40:27
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using SUIFW;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIForm : BaseUIForm
{
    [SerializeField] private Button _BtnLogin;
    [SerializeField] private Button _BtnExit;

    public Button btnLogin { get => _BtnLogin;}
    public Button btnExit { get => _BtnExit;}
}

