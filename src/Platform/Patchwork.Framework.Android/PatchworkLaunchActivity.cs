﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Patchwork.Framework;
using Patchwork.Framework.Messaging;
using Shin.Framework.Logging.Loggers;
using Shin.Framework.Logging.Native;

namespace Patchwork.Framework
{
    [Activity(MainLauncher = true)]
    public class PatchworkLaunchActivity : PatchworkActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
{
            base.OnCreate(savedInstanceState);
            var token = (Application.Context as PatchworkApplication)?.Token.Token ?? CancellationToken.None;
            var win = Core.Window.CurrentWindow ?? Core.Window.CreateWindow();
            Title = win.Title;
            Core.RunAsync(token);
            Finish();
        }

        /// <inheritdoc />
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Core.MessagePump.Push(new PlatformMessage(MessageIds.Quit));

            //(Application.Context as PatchworkApplication)?.Token.Cancel();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            //Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}