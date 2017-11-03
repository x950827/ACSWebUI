package com.osoro.acsandroid;

import android.app.ActivityManager;
import android.content.Context;
import android.content.Intent;
import android.database.Cursor;
import android.database.sqlite.SQLiteException;
import android.nfc.NfcAdapter;
import android.nfc.Tag;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.ImageView;
import android.widget.Toast;

import java.text.SimpleDateFormat;
import java.util.Date;

public class NFCActivity extends AppCompatActivity {
    private NfcAdapter nfcAdapter;
    DB db;
    PDB pdb;
    ImageView imageView;

    private boolean isMyServiceRunning(Class<?> serviceClass) {
        ActivityManager manager = (ActivityManager) getSystemService(Context.ACTIVITY_SERVICE);
        for (ActivityManager.RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
            if (serviceClass.getName().equals(service.service.getClassName())) {
                return true;
            }
        }
        return false;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_nfc);

        imageView = (ImageView) findViewById(R.id.iV);
        db = new DB(this);
        pdb = new PDB(this);
        db.open();
        pdb.open();

        if (!isMyServiceRunning(BackgroundService.class)) {
            Intent intent = new Intent(this, BackgroundService.class);
            startService(intent);
        }

        nfcAdapter = NfcAdapter.getDefaultAdapter(this);
        if (nfcAdapter == null) {
            showToast("NFC NOT supported on this devices!");
            finish();
        } else if (!nfcAdapter.isEnabled()) {
            showToast("NFC NOT Enabled!");
            finish();
        }
    }

    @Override
    protected void onResume() {
        super.onResume();

        Intent intent = getIntent();
        String action = intent.getAction();

        if (NfcAdapter.ACTION_TAG_DISCOVERED.equals(action)) {
            //Toast.makeText(this,
            //"onResume() - ACTION_TAG_DISCOVERED",
            //Toast.LENGTH_SHORT).show();

            Tag tag = intent.getParcelableExtra(NfcAdapter.EXTRA_TAG);
            if (tag == null) {
                //textViewInfo.setText("tag == null");
            } else {
                String tagInfo = tag.toString() + "\n";
                String code = new String();

                tagInfo += "\nTag Id: \n";
                byte[] tagId = tag.getId();
                tagInfo += "length = " + tagId.length + "\n";
                for (int i = 0; i < tagId.length; i++) {
                    tagInfo += Integer.toHexString(tagId[i] & 0xFF) + " ";
                    code += Integer.toHexString(tagId[i] & 0xFF);
                    //code += Integer.toString(tagId[i]);
                }
                tagInfo += "\n";

                String[] techList = tag.getTechList();
                tagInfo += "\nTech List\n";
                tagInfo += "length = " + techList.length + "\n";
                for (int i = 0; i < techList.length; i++) {
                    tagInfo += techList[i] + "\n ";
                }

                //textViewInfo.setText(tagInfo);
                //int decCode = Integer.parseInt(code, 16);
                findWorker(code);
            }
        } else {
            //Toast.makeText(this,
            //"onResume() : " + action,
            //Toast.LENGTH_SHORT).show();
        }
    }

    public void findWorker(String response) {
        Cursor cursor = db.findWorker(response);
        try {
            if (cursor != null && cursor.getCount() > 0) {
                try {
                    if (cursor.moveToNext()) {
                        imageView.setImageResource(R.mipmap.passage);
                        String fio = cursor.getString(cursor.getColumnIndex(DB.WORKER_COLUMN_FIO));
                        int id_worker = cursor.getInt(cursor.getColumnIndex(DB.WORKER_COLUMN_ID_WORKER));
                        //textView.setText(code);

                        Date date = new Date();
                        SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
                        Passage passage = new Passage(id_worker, format.format(date));
                        pdb.addPassage(passage);
                        showToast("Worker: " + fio + " is come/gone, Count: " + pdb.getCount());
                    } else {
                        //textView.setText("");
                        imageView.setImageResource(R.mipmap.notfound);
                        //Date date = new Date();
                        //SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
                        //Passage passage = new Passage(63, format.format(date));
                        //pdb.addPassage(passage);
                        showToast("Worker not found, Count: " + pdb.getCount());
                    }
                } finally {
                    try {
                        cursor.close();

                    } catch (Exception e) {

                    }
                }
            } else {
                imageView.setImageResource(R.mipmap.notfound);
                showToast("Worker not found, Count: " + pdb.getCount());
            }
        } catch (SQLiteException e) {
            showToast("findWorker: " + e.getMessage());
        }
    }

    public void showToast(String text) {
        Toast.makeText(this, text, Toast.LENGTH_LONG).show();
    }
}
