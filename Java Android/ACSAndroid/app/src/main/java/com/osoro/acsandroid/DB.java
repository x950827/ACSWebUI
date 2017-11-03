package com.osoro.acsandroid;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;


public class DB {
    private static final String DB_NAME = "database";
    private static final int DB_VERSION = 1;
    private static final String WORKERS_TABLE = "workers";


    public static final String WORKER_COLUMN_ID = "_id";
    //public static final String COLUMN_IMG = "img";
    public static final String WORKER_COLUMN_ID_WORKER = "worker_id";
    public static final String WORKER_COLUMN_ACTIVE = "active";
    public static final String WORKER_COLUMN_TEMPORARY = "temporary";
    //public static final String WORKER_COLUMN_ID_USER = "id_user";
    public static final String WORKER_COLUMN_FIO = "fio";
    public static final String WORKER_COLUMN_WORK_POSITION = "work_position";
    public static final String WORKER_COLUMN_DATE_START = "date_start";
    public static final String WORKER_COLUMN_DATE_END = "date_end";
    public static final String WORKER_COLUMN_KEY_CODE = "key_code";

    private static final String WORKERS_DB_CREATE =
            "create table " + WORKERS_TABLE + "(" +
                    WORKER_COLUMN_ID + " integer primary key autoincrement, " +
                    WORKER_COLUMN_ID_WORKER + " integer, " +
                    WORKER_COLUMN_ACTIVE + " integer, " +
                    WORKER_COLUMN_TEMPORARY + " integer, " +
                    //WORKER_COLUMN_ID_USER + " integer, " +
                    WORKER_COLUMN_FIO + " text, " +
                    WORKER_COLUMN_WORK_POSITION + " text, " +
                    WORKER_COLUMN_DATE_START + " text, " +
                    WORKER_COLUMN_DATE_END + " text, " +
                    WORKER_COLUMN_KEY_CODE + " text" +
                    ");";

    private static final String WORKERS_DB_DELETE =
            "DROP TABLE IF EXISTS " + WORKERS_TABLE;


    private final Context mCtx;


    private DBHelper mDBHelper;
    private SQLiteDatabase mDB;

    public DB(Context ctx) {
        mCtx = ctx;
    }

    // открыть подключение
    public void open() {
        mDBHelper = new DBHelper(mCtx, DB_NAME, null, DB_VERSION);
        mDB = mDBHelper.getWritableDatabase();
    }

    // закрыть подключение
    public void close() {
        if (mDBHelper != null) mDBHelper.close();
    }

    // получить все данные из таблицы WORKERS_TABLE
    public Cursor getAllData() {
        return mDB.query(WORKERS_TABLE, null, null, null, null, null, null);
    }

    // добавить запись в WORKERS_TABLE
    public String addRec(final Worker worker) {
        /*Cursor cursor = mDB.rawQuery("select * from " + WORKERS_TABLE + " where " + WORKER_COLUMN_ID_WORKER + " like ?", new String[]{Integer.toString(worker.worker_id)});
        if (cursor != null) {
            if (cursor.moveToNext()) {
                return "exist";
            } else {*/
        try {
            ContentValues cv = new ContentValues();
            cv.put(WORKER_COLUMN_ID_WORKER, worker.worker_id);
            cv.put(WORKER_COLUMN_ACTIVE, worker.active);
            cv.put(WORKER_COLUMN_TEMPORARY, worker.temporary);
            //cv.put(WORKER_COLUMN_ID_USER, worker.id_user);
            cv.put(WORKER_COLUMN_FIO, worker.fio);
            cv.put(WORKER_COLUMN_WORK_POSITION, worker.work_position);
            cv.put(WORKER_COLUMN_DATE_START, worker.date_start);
            cv.put(WORKER_COLUMN_DATE_END, worker.date_end);
            cv.put(WORKER_COLUMN_KEY_CODE, worker.key_code);
            mDB.insert(WORKERS_TABLE, null, cv);
            return "added";
        } catch (Exception e) {
            return "error";
        }
        //}
        //}
        //return "null";
    }

    public int getWorkersCount() {
        Cursor cursor = mDB.rawQuery("select * from " + WORKERS_TABLE + " where " + WORKER_COLUMN_ID, null);
        if (cursor.moveToFirst()) {
            int first = cursor.getInt(cursor.getColumnIndex(WORKER_COLUMN_ID));
            cursor.moveToLast();
            int last = cursor.getInt(cursor.getColumnIndex(WORKER_COLUMN_ID));
            return last - first + 1;
        }
        return 0;
    }

    // удалить запись из WORKERS_TABLE
    public void delRec(long id) {
        mDB.delete(WORKERS_TABLE, WORKER_COLUMN_ID + " = " + id, null);
    }

    public Cursor findWorker(String code) {
        return mDB.rawQuery("select * from " + WORKERS_TABLE + " where " + WORKER_COLUMN_KEY_CODE + " like ?", new String[]{code});
    }

    public void deleteDB(){
        mDB.delete(WORKERS_TABLE, null, null);
    }

    // класс по созданию и управлению БД
    private class DBHelper extends SQLiteOpenHelper {

        public DBHelper(Context context, String name, SQLiteDatabase.CursorFactory factory,
                        int version) {
            super(context, name, factory, version);
        }

        // создаем и заполняем БД
        @Override
        public void onCreate(SQLiteDatabase db) {
            db.execSQL(WORKERS_DB_CREATE);

        }

        @Override
        public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
            db.execSQL(WORKERS_DB_DELETE);
            onCreate(db);
        }
    }
}
