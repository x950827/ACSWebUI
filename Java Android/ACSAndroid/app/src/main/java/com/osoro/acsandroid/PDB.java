package com.osoro.acsandroid;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

import java.util.ArrayList;

public class PDB {
    private static final String DB_NAME = "pdatabase";
    private static final int DB_VERSION = 1;
    private static final String PASSAGES_TABLE = "passages";

    public static final String PASSAGE_COLUMN_ID = "_id";
    public static final String PASSAGE_COLUMN_ID_WORKER = "id_worker";
    public static final String PASSAGE_COLUMN_DATE = "date";

    private static final String PASSAGES_DB_CREATE =
            "create table " + PASSAGES_TABLE + "(" +
                    PASSAGE_COLUMN_ID + " integer primary key autoincrement, " +
                    PASSAGE_COLUMN_ID_WORKER + " integer, " +
                    PASSAGE_COLUMN_DATE + " text" +
                    ");";


    private final Context mCtx;


    private PDBHelper mDBHelper;
    private SQLiteDatabase mDB;

    public PDB(Context ctx) {
        mCtx = ctx;
    }

    // открыть подключение
    public void open() {
        mDBHelper = new PDBHelper(mCtx, DB_NAME, null, DB_VERSION);
        mDB = mDBHelper.getWritableDatabase();
    }

    // закрыть подключение
    public void close() {
        if (mDBHelper != null) mDBHelper.close();
    }

    // получить все данные из таблицы WORKERS_TABLE
    public Cursor getAllData() {
        return mDB.query(PASSAGES_TABLE, null, null, null, null, null, null);
    }

    public ArrayList<Passage> getAllPassages() {
        ArrayList<Passage> passages = new ArrayList<>();
        Cursor cursor = getAllData();
        while (cursor.moveToNext()) {
            int id_worker = cursor.getInt(cursor.getColumnIndex(PASSAGE_COLUMN_ID_WORKER));
            String date = cursor.getString(cursor.getColumnIndex(PASSAGE_COLUMN_DATE));
            if (date != null)
                passages.add(new Passage(id_worker, date));
        }
        if (passages.size() == 0)
            return null;
        return passages;
    }

    public void addPassage(final Passage passage) {
        ContentValues cv = new ContentValues();
        cv.put(PASSAGE_COLUMN_ID_WORKER, passage.id_worker);
        cv.put(PASSAGE_COLUMN_DATE, passage.date);
        mDB.insert(PASSAGES_TABLE, null, cv);
    }

    public int getLastPassageIndex() {
        Cursor cursor = mDB.rawQuery("select * from " + PASSAGES_TABLE + " where " + PASSAGE_COLUMN_ID, null);
        if (cursor.moveToLast()) {
            return cursor.getInt(cursor.getColumnIndex(PASSAGE_COLUMN_ID));
        }
        return 0;
    }

    public int getCount() {
        Cursor cursor = mDB.rawQuery("select * from " + PASSAGES_TABLE + " where " + PASSAGE_COLUMN_ID, null);
        cursor.moveToNext();
        int first = cursor.getInt(cursor.getColumnIndex(PASSAGE_COLUMN_ID));
        if (cursor.moveToLast()) {
            int last = cursor.getInt(cursor.getColumnIndex(PASSAGE_COLUMN_ID));
            return last - first + 1;
        }
        return 0;
    }

    // удалить запись из WORKERS_TABLE
    public void delRec(long id) {
        mDB.delete(PASSAGES_TABLE, PASSAGE_COLUMN_ID_WORKER + " = " + id, null);
    }

    public void deletePassagesTable() {
        mDB.delete(PASSAGES_TABLE, null, null);
    }

    // класс по созданию и управлению БД
    private class PDBHelper extends SQLiteOpenHelper {

        public PDBHelper(Context context, String name, SQLiteDatabase.CursorFactory factory,
                         int version) {
            super(context, name, factory, version);
        }

        // создаем и заполняем БД
        @Override
        public void onCreate(SQLiteDatabase db) {
            db.execSQL(PASSAGES_DB_CREATE);

        }

        @Override
        public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        }
    }

}
