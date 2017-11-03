package com.osoro.acsandroid;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

public class Passage {
    public final int skip_id;
    public final String date;


    public Passage(int id_worker, String date) {
        this.skip_id = id_worker;
        this.date = date;
    }

    public static JSONObject toJsonObject(final Passage passage) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("date", passage.date);
            jsonObject.put("skip_id", passage.skip_id);

        } catch (JSONException e) {
            e.printStackTrace();
        }
        return jsonObject;
    }

    public static JSONObject toJsonArray(ArrayList<Passage> passages) {

        JSONObject jsonObject = new JSONObject();
        JSONArray jsonArray = new JSONArray();
        try {
            for (Passage passage :
                    passages) {
                if (passage != null)
                    jsonArray.put(toJsonObject(passage));
            }
            jsonObject.put("method", "updateTable");
            jsonObject.put("history", jsonArray);

        } catch (JSONException e) {
            e.printStackTrace();
        }
        return jsonObject;
    }
}
