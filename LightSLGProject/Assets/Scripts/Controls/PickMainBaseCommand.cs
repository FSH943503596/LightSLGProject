/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-13-11:43:41
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class PickMainBaseCommand:SimpleCommand
{
    public override void Execute(INotification notification)
    {
        SendNotification(GlobalSetting.Msg_PickMainBase, notification.Body);
    }}

