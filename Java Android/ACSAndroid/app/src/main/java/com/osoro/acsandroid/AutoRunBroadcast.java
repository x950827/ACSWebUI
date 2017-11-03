package com.osoro.acsandroid;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

public class AutoRunBroadcast extends BroadcastReceiver {

    @Override
    public void onReceive(Context context, Intent intent) {
        context.startService(new Intent(context, BackgroundService.class));
    }
}
