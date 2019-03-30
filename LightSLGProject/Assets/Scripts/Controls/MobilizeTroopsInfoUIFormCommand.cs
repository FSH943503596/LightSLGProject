/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-12-16:35:35
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class MobilizeTroopsInfoUIFormCommand:SimpleCommand
{
    public override void Execute(INotification notification)
    {
        SendNotification(GlobalSetting.Msg_InitMobilizeTroopsMediator, notification.Body);
    }}

