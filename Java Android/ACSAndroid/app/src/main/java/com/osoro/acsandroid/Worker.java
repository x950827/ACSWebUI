package com.osoro.acsandroid;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

public class Worker {
    public final int worker_id;
    public final int active;
    public final int temporary;
    //public final int id_user;
    public final String fio;
    public final String work_position;
    public final String date_start;
    public final String date_end;
    public final String key_code;

    public Worker(int worker_id,
                  int active,
                  int temporary,
                  //int id_user,
                  String fio,
                  String work_position,
                  String date_start,
                  String date_end,
                  String key_code) {
        this.worker_id = worker_id;
        this.active = active;
        this.temporary = temporary;
        //this.id_user = id_user;
        this.fio = fio;
        this.work_position = work_position;
        this.date_start = date_start;
        this.date_end = date_end;
        this.key_code = key_code;
    }

    public static Worker fromJson(final JSONObject json) {
        final int worker_id = json.optInt("worker_id", 0);
        final int active = json.optInt("active", 0);
        final int temporary = json.optInt("temporary", 0);
        //final int id_user = json.optInt("id_user", 0);
        final String fio = json.optString("fio", "");
        final String work_position = json.optString("work_position", "");
        final String date_start = json.optString("date_start", "");
        final String date_end = json.optString("date_end", "");
        final String key_code = json.optString("key_code", "");
        return new Worker(worker_id, active, temporary, fio, work_position, date_start, date_end, key_code);
    }

    public static ArrayList<Worker> fromJson(final JSONArray json) {
        final ArrayList<Worker> workers = new ArrayList<Worker>();

        for (int i = 0; i < json.length(); ++i) {
            try {
                final Worker worker = fromJson(json.getJSONObject(i));
                if (worker != null) workers.add(worker);
            } catch (final JSONException ignore) {
            }
        }
        return workers;
    }

}
