package com.osoro.acsandroid;

import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Handler;
import android.os.IBinder;
import android.os.Looper;
import android.widget.ImageView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.Timer;
import java.util.TimerTask;

public class BackgroundService extends Service {
    public BackgroundService() {
    }

    String uniqueID;
    Timer timer;
    TimerTask timerTask;
    DownloadWorkersAsync task;
    DB db;
    PDB pdb;
    Boolean isAuth = false;
    String cookies;
    ImageView imageView;

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public void onCreate() {
        super.onCreate();
        db = new DB(this);
        pdb = new PDB(this);
        timer = new Timer();
        timerTask = new GetWorkers();
        task = new DownloadWorkersAsync();
        showToast("System is running");
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        if ((flags & START_FLAG_RETRY) == 0) {
            db.open();
            pdb.open();

            uniqueID = "35" +
                    Build.BOARD.length() % 10 + Build.BRAND.length() % 10 +
                    Build.DEVICE.length() % 10 + Build.USER.length() % 10 +
                    Build.DISPLAY.length() % 10 + Build.HOST.length() % 10 +
                    Build.ID.length() % 10 + Build.MANUFACTURER.length() % 10 +
                    Build.MODEL.length() % 10 + Build.PRODUCT.length() % 10 +
                    Build.TAGS.length() % 10 + Build.TYPE.length() % 10;

            timer.schedule(timerTask, 0, 20000);
        }
        return Service.START_STICKY;
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
    }


    private class DownloadWorkersAsync extends AsyncTask<String, Void, String> {
        @Override
        protected String doInBackground(String... urls) {
            URL url = null;
            HttpURLConnection conn = null;
            String output = "";
            try {
                url = new URL(urls[0]);
                conn = (HttpURLConnection) url.openConnection();
                // Modification
                conn.connect();
                BufferedReader br = new BufferedReader(new InputStreamReader((conn.getInputStream())));
                output = br.readLine();


                JSONObject jObject;
                if (output == null)
                    return null;

                try {
                    output = output.replace("&quot;", "\"");
                    jObject = new JSONObject(output);

                    JSONArray jsonArray = jObject.getJSONArray("worker");
                    ArrayList<Worker> workers = Worker.fromJson(jsonArray);
                    for (int i = 0; i < workers.size(); i++) {
                        db.addRec(workers.get(i));
                    }
                } catch (JSONException e) {
                    showToast(e.getMessage());
                }
                return "sucsess";

            } catch (MalformedURLException e) {
                e.printStackTrace();
            } catch (IOException e) {
                e.printStackTrace();
            }
            return "Error";
        }

        // onPostExecute displays the results of the AsyncTask.
        @Override
        protected void onPostExecute(String result) {
            //workersResponse = result.replace(" null", "");

            showToast(result);

        }
    }

    class GetWorkers extends TimerTask {
        @Override
        public void run() {
            /*String resp = db.addRec(new Worker(0, 0, 0, 0, "fio", "prog", "2017", "2017", "4d091a204e80"));
            int count = db.getWorkersCount();
            showToast("New worker " + resp + ", Count: " + count);*/
            //task.cancel(true);
            //task.execute("http://websocket.osora.ru:86/index.php/?123=1");

            new Thread(new Runnable() {
                @Override
                public void run() {
                    if (isAuth)
                        setAndGetDB();
                    else auth();

                }
            }).run();

        }
    }

    public void auth() {
        URL url = null;
        HttpURLConnection conn = null;
        String output = "";

        try {
            url = new URL("http://osora.ru/scanner/API/auth?code=" + uniqueID);
            conn = (HttpURLConnection) url.openConnection();
            conn.setUseCaches(true);
            conn.connect();
            cookies = conn.getHeaderField("Set-Cookie");//.substring(0, cookies.indexOf(";"));
            isAuth = true;
            setAndGetDB();

            //BufferedReader br = new BufferedReader(new InputStreamReader((conn.getInputStream())));
            //output = br.readLine();



            /*JSONObject jObject;
            if (output == null)
                return;

            try {
                output = output.replace("&quot;", "\"").substring(0, output.indexOf(",\"data")) + "}";

                jObject = new JSONObject(output);


                isAuth = jObject.optBoolean("success");
                showToast(Boolean.toString(isAuth));
                if (isAuth == true)
                    setAndGetDB();

            } catch (JSONException e) {
                showToast(e.getMessage());
            }*/
        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void setAndGetDB() {
        URL url = null;
        HttpURLConnection conn = null;
        String output = "";


        try {
            url = new URL("http://osora.ru/scanner/API/getData");
            conn = (HttpURLConnection) url.openConnection();
            conn.setRequestProperty("Cookie", cookies);
            conn.connect();
            BufferedReader br = new BufferedReader(new InputStreamReader((conn.getInputStream())));
            output = br.readLine();
            int inCount = 0;
            int outCount;
            String response;


            JSONObject jObject;
            if (output == null)
                return;

            try {
                output = output.replace("&quot;", "\"");
                jObject = new JSONObject(output);

                JSONObject data = jObject.optJSONObject("data");
                JSONArray jsonArray = data.getJSONArray("workers");
                ArrayList<Worker> workers = Worker.fromJson(jsonArray);
                if (workers.size() != 0)
                    db.deleteDB();
                for (int i = 0; i < workers.size(); i++) {
                    response = db.addRec(workers.get(i));
                    if (response == "added")
                        inCount++;
                }
                outCount = db.getWorkersCount();
                if (inCount == 0)
                    showToast("No new workers added");
                else showToast(inCount + " workers from server is added, Summary count: " + outCount);


            } catch (JSONException e) {
                showToast(e.getMessage());
            }
        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        try {
            url = new URL("http://osora.ru/scanner/API/setData");
            conn = (HttpURLConnection) url.openConnection();
            conn.setRequestProperty("Cookie", cookies);
            conn.setRequestMethod("POST");
            //conn.setRequestProperty("Content-Type", "application/json;charset=UTF-8");
            //conn.setRequestProperty("Accept", "application/json");
            conn.setDoOutput(true);
            conn.setDoInput(true);

            if (pdb.getAllPassages() == null) {
                showToast("No passages to delivery");
                return;
            }
            JSONObject jsonParam = Passage.toJsonArray(pdb.getAllPassages());
            String str = "data=" + jsonParam.toString();
            //List nameValueParams = new ArrayList(1);
            OutputStream os = conn.getOutputStream();
            BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(os, "UTF-8"));
            bw.write(str);
            bw.flush();
            bw.close();
            os.close();
            conn.connect();




            BufferedReader br = new BufferedReader(new InputStreamReader((conn.getInputStream())));
            String resp = br.readLine();
            JSONObject jsonObject = new JSONObject(resp);
            String status = jsonObject.optString("success");
            if (status.contains("true")) {
                showToast(pdb.getCount() + " Passages is delivered");
                pdb.deletePassagesTable();

            } else showToast("Passages isnt delivered");




            conn.disconnect();

        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }


    public void showToast(final String text) {
        final Context context = this;

        new Handler(Looper.getMainLooper()).post(new Runnable() {
            @Override
            public void run() {
                Toast.makeText(context, text, Toast.LENGTH_LONG).show();

            }
        });
    }
}
